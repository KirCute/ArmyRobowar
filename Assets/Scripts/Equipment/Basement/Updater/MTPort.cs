using System;
using Equipment.Robot;
using Equipment.Robot.Body;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Updater {
    public class MTPort : MonoBehaviourPun {
        private MEBaseFlag identity;
        
        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<MTRobotHurt>() != null && Summary.team.teamColor == identity.flagColor) {
                var robot = other.GetComponentInParent<MERobotIdentifier>();
                if (robot.team == identity.flagColor) {
                    Summary.team.robots[robot.id].gameObject = other.gameObject;
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.GetComponent<MTRobotHurt>() != null && Summary.team.teamColor == identity.flagColor) {
                var robot = other.GetComponentInParent<MERobotIdentifier>();
                if (robot.team == identity.flagColor) {
                    Summary.team.robots[robot.id].gameObject = null;
                }
            }
        }
    }
}