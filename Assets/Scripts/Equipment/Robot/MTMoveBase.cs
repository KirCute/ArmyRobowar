using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot {
    public class MTMoveBase : MonoBehaviourPun {
        private const float ACCEPT_NAVIGATION_ERROR = 0.5F;
        private MERobotIdentifier identity;
        private readonly List<Vector2> navigationPoints = new();

        private void Awake() {
            identity = GetComponent<MERobotIdentifier>();
        }

        private void Update() {
            if (photonView.IsMine && navigationPoints.Count > 0) {
                var pos = new Vector2(transform.position.x, transform.position.z);
                if (Vector2.Distance(navigationPoints[0], pos) < ACCEPT_NAVIGATION_ERROR) {
                    navigationPoints.RemoveAt(0);
                    if (navigationPoints.Count == 0) {
                        SendCancel();
                    } else {
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
                navigationPoints.Clear();
                navigationPoints.Add(new Vector2(transform.position.x, transform.position.z));
                for (var i = 2; i < (int) args[1] + 2; i++) {
                    navigationPoints.Add((Vector2) args[i]);
                }
            }
        }

        private void OnControlled(object[] args) {
            if (identity.id == (int) args[0] && navigationPoints.Count > 0 && args[1] != null) {
                navigationPoints.Clear();
                SendCancel();
            }
        }

        private void SendCancel() {
            Events.Invoke(Events.M_ROBOT_MOTIVATION_CHANGE, new object[] {identity.id, 0, Vector2.zero});
        }
    }
}