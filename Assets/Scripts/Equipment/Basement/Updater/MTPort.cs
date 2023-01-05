using System;
using Equipment.Robot;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Updater {
    /// <summary>
    /// 用于更新机器人的atHome和atBase字段，从而判定机器人能否改装和是否将要占领基地
    /// </summary>
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

        private void OnTriggerExit(Collider other) {
            var robot = other.GetComponent<MERobotIdentifier>();
            if (robot != null && Summary.team.teamColor == robot.team) {
                Summary.team.robots[robot.id].atBase = -1;
                Summary.team.robots[robot.id].atHome = false;
            }
        }
    }
}