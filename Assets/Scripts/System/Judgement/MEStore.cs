using Photon.Pun;

namespace System.Judgement {
    public class MEStore : MonoBehaviourPun {
        private void OnEnable() {
            Events.AddListener(Events.M_CREATE_ROBOT, OnCreateRobot);
            Events.AddListener(Events.M_TEAM_BUY_COMPONENT, OnBuyingComponent);
            Events.AddListener(Events.M_CREATE_TOWER, OnCreatingTower);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CREATE_ROBOT, OnCreateRobot);
            Events.RemoveListener(Events.M_TEAM_BUY_COMPONENT, OnBuyingComponent);
            Events.RemoveListener(Events.M_CREATE_TOWER, OnCreatingTower);
        }

        private static void OnCreateRobot(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                Summary.team.coins -= Constants.ROBOT_TEMPLATES[(string) args[1]].cost;
            }
        }

        private void OnBuyingComponent(object[] args) {
            var template = Constants.SENSOR_TEMPLATES[(string) args[1]];
            if (Summary.team.teamColor == (int) args[0]) {
                Summary.team.coins -= template.cost;
            }

            if (photonView.IsMine) Events.Invoke(Events.F_TEAM_ACQUIRE_COMPONENT, new[] {
                args[0], args[1], template.maxHealth
            });
        }

        private static void OnCreatingTower(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                Summary.team.coins -= Constants.TOWER_TEMPLATES[(string) args[1]].cost;
            }
        }
    }
}