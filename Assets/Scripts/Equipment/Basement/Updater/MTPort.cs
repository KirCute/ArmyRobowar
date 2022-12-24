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
            if (Summary.team.teamColor == identity.flagColor && other.GetComponent<MTRobotHurt>() != null) {
                var robot = other.GetComponentInParent<MERobotIdentifier>();
                if (robot.team == identity.flagColor) {
                    Summary.team.robots[identity.baseId].gameObject = other.gameObject;
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (Summary.team.teamColor == identity.flagColor && other.GetComponent<MTRobotHurt>() != null) {
                var robot = other.GetComponentInParent<MERobotIdentifier>();
                if (robot.team == identity.flagColor) {
                    Summary.team.robots[identity.baseId].gameObject = null;
                }
            }
        }
    }
}