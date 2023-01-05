using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot {
    /// <summary>
    /// 与MEMotivationChanger通过事件系统通讯，用于完成机器人的自主导航任务
    /// </summary>
    public class MTMoveBase : MonoBehaviourPun {
        private const float ACCEPT_NAVIGATION_ERROR = 0.5F;
        private MERobotIdentifier identity;
        private readonly List<Vector2> navigationPoints = new();

        private void Awake() {
            identity = GetComponent<MERobotIdentifier>();
        }

        private void Update() {
            if (photonView.IsMine && navigationPoints.Count > 0) {  // 当有导航任务时
                var pos = new Vector2(transform.position.x, transform.position.z);
                if (Vector2.Distance(navigationPoints[0], pos) < ACCEPT_NAVIGATION_ERROR) {  // 如果已经到达目标点
                    navigationPoints.RemoveAt(0);
                    if (navigationPoints.Count == 0) {  // 没有更多目标点了，让小车停在原地
                        SendCancel();
                    } else {  // 发送下一个目标点
                        Events.Invoke(Events.M_ROBOT_MOTIVATION_CHANGE, new object[] {identity.id, 1, navigationPoints[0]});
                    }
                }
            }
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_NAVIGATION, OnCommanded);
            Events.AddListener(Events.M_ROBOT_CONTROL, OnControlled);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_NAVIGATION, OnCommanded);
            Events.RemoveListener(Events.M_ROBOT_CONTROL, OnControlled);
        }

        private void OnCommanded(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                navigationPoints.Clear();  // 有可能正在执行导航，此时要清空之前的任务
                navigationPoints.Add(new Vector2(transform.position.x, transform.position.z));
                for (var i = 2; i < (int) args[1] + 2; i++) {
                    navigationPoints.Add((Vector2) args[i]);
                }
            }
        }

        private void OnControlled(object[] args) {
            if (identity.id == (int) args[0] && navigationPoints.Count > 0 && args[1] != null) {  // 被控制时终止任务
                navigationPoints.Clear();
                SendCancel();
            }
        }

        /// <summary>
        /// 发送要求小车原地停止的事件
        /// </summary>
        private void SendCancel() {
            Events.Invoke(Events.M_ROBOT_MOTIVATION_CHANGE, new object[] {identity.id, 0, Vector2.zero});
        }
    }
}