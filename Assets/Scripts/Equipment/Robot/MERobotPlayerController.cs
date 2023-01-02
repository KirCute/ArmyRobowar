using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Equipment.Robot {
    public class MERobotPlayerController : MonoBehaviourPun {
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_CONTROL, OnControlled);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_CONTROL, OnControlled);
        }

        private void OnControlled(object[] args) {
            if (identity.id == (int) args[0] && Summary.team.teamColor == identity.team) {
                Summary.team.robots[identity.id].controller = (Player) args[1];
            }
        }
    }
}