using System;
using Equipment.Robot.Body;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Spawner {
    public class MERobotCreator : MonoBehaviourPun {
        private static int nextRobotID = -1;
        private MEBaseFlag identity;
        private int crowd = 0;
        private int creatingId = -1;

        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }

        private void Update() {
            if (creatingId == -1 || Summary.team.teamColor != identity.flagColor) return;
            var robot = Summary.team.robots[creatingId];
            if (PhotonNetwork.Time - robot.createTime < robot.template.makingTime || crowd > 0) return;
            robot.manufacturing = false;
            if (Summary.isTeamLeader) {
                PhotonNetwork.Instantiate(robot.template.prefabName, transform.position, transform.rotation, 0,
                    new object[] {creatingId, Summary.team.teamColor}
                );
                Events.Invoke(Events.F_ROBOT_CREATED, new object[] {Summary.team.teamColor, robot.id});
            }

            creatingId = -1;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<MTRobotHurt>() != null) crowd++;
        }

        private void OnTriggerExit(Collider other) {
            if (other.GetComponent<MTRobotHurt>() != null) crowd--;
        }

        private void OnEnable() {
            Events.AddListener(Events.M_CREATE_ROBOT, OnRobotCreating);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CREATE_ROBOT, OnRobotCreating);
        }

        private void OnRobotCreating(object[] args) {
            nextRobotID++;
            if (identity.baseId == (int) args[0] && Summary.team.teamColor == identity.flagColor) {
                var template = Constants.ROBOT_TEMPLATES[(string) args[1]];
                Summary.team.robots.Add(
                    nextRobotID, new Model.Equipment.Robot(nextRobotID, (string) args[2], template)
                );
                creatingId = nextRobotID;
            }
        }
    }
}