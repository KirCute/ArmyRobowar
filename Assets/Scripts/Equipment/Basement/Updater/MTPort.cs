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

        private void OnTriggerStay(Collider other) {
            var robot = other.GetComponent<MERobotIdentifier>();
            if (robot != null && Summary.team.teamColor == robot.team) {
                Summary.team.robots[robot.id].atBase = identity.flagColor == -1 ? identity.baseId : -1;
                Summary.team.robots[robot.id].atHome = robot.team == identity.flagColor;
            }
        }
    }
}