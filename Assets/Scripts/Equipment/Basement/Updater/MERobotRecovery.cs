using System;
using Equipment.Robot;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Updater {
    public class MERobotRecovery : MonoBehaviourPun {
        private const double RECOVER_DELAY = 10.0;
        private const int RECOVER_AMOUNT = 3;
        private MEBaseFlag identity;

        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }

        private void OnTriggerStay(Collider other) {
            var robot = other.GetComponent<MERobotIdentifier>();
            if (robot != null && Summary.team.teamColor == identity.flagColor && robot.team == identity.flagColor && 
                Summary.team.robots[robot.id].lastRecoveryTime < PhotonNetwork.Time - RECOVER_DELAY) {
                Summary.team.robots[robot.id].lastRecoveryTime = PhotonNetwork.Time;
                if (Summary.isTeamLeader) {
                    Events.Invoke(Events.M_BODY_DAMAGE, new object[] {robot.id, RECOVER_AMOUNT});
                }
            }
        }
    }
}