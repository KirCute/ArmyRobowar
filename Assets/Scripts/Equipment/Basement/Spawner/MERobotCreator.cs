using System;
using System.Collections.Generic;
using Equipment.Robot.Body;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Spawner {
    /// <summary>
    /// 用于生成小车的脚本，当其长方体碰撞箱内已经有其它机器人时，会阻断新的机器人的生成进程
    /// </summary>
    public class MERobotCreator : MonoBehaviourPun {
        private const float SPAWN_WAIT = 1.0F;
        
        private static int nextRobotID = -1;  // 最后一辆机器人的id，用来保证全场机器人id不重复
        private MEBaseFlag identity;
        private int crowd;  // 生成点当前的机器人数，理论上最大为1，当不为0时阻塞小车生成
        private readonly List<int> creatingIds = new();  // 正在生成中的机器人id
        private float wait;  // 生成小车后添加延迟，避免刚刚解阻塞马上所有阻塞中的机器人全部生成

        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
            nextRobotID = -1;
        }

        private void Update() {
            if (!Summary.isGameStarted || creatingIds.Count == 0 || Summary.team.teamColor != identity.flagColor) return;
            var robot = Summary.team.robots[creatingIds[0]];
            wait = Mathf.Max(wait - Time.deltaTime, 0.0f);
            if (wait > .0f || PhotonNetwork.Time - robot.createTime < robot.template.makingTime || crowd > 0) return;
            // 抵达此处时满足机器人生成条件
            if (Summary.isTeamLeader) {
                PhotonNetwork.Instantiate(robot.template.prefabName, transform.position, transform.rotation, 0,
                    new object[] {creatingIds[0], Summary.team.teamColor}
                );
                Events.Invoke(Events.F_ROBOT_CREATED, new object[] {Summary.team.teamColor, robot.id});
            }

            creatingIds.RemoveAt(0);
            wait = SPAWN_WAIT;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<MTRobotHurt>() != null) crowd++;
        }

        private void OnTriggerExit(Collider other) {
            if (other.GetComponent<MTRobotHurt>() != null) crowd--;
        }

        private void OnEnable() {
            Events.AddListener(Events.M_CREATE_ROBOT, OnRobotCreating);
            Events.RemoveListener(Events.F_BASE_DESTROYED, OnConquered);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CREATE_ROBOT, OnRobotCreating);
            Events.RemoveListener(Events.F_BASE_DESTROYED, OnConquered);
        }

        private void OnRobotCreating(object[] args) {
            if (identity.baseId == (int) args[0]) {
                nextRobotID++;
                if (Summary.team.teamColor == identity.flagColor) {
                    var template = Constants.ROBOT_TEMPLATES[(string) args[1]];
                    Summary.team.robots.Add(
                        nextRobotID, new Model.Equipment.Robot(nextRobotID, (string) args[2], template)
                    );
                    creatingIds.Add(nextRobotID);
                }
            }
        }

        private void OnConquered(object[] args) {
            if (identity.baseId == (int) args[0] && Summary.team.teamColor == (int) args[1]) {
                // 占领时取消生成所有正在生产的小车
                foreach (var fetus in creatingIds) {
                    Summary.team.robots[fetus].manufacturing = false;
                }
                creatingIds.Clear();
            }
        } 
    }
}