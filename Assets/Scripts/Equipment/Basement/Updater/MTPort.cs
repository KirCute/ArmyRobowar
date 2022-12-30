using System;
using Equipment.Robot;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Updater {
    public class MTPort : MonoBehaviourPun {
        private MEBaseFlag identity;

        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }

        private void OnTriggerEnter(Collider other) {
            var robot = other.GetComponent<MERobotIdentifier>();
            if (robot != null && Summary.team.teamColor == identity.flagColor && robot.team == identity.flagColor) {
                Summary.team.robots[robot.id].gameObject = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other) {
            var robot = other.GetComponent<MERobotIdentifier>();
            if (robot != null && Summary.team.teamColor == identity.flagColor && robot.team == identity.flagColor) {
                Summary.team.robots[robot.id].gameObject = null;
            }
        }
    }
}