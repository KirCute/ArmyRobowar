using Model.Inventory;
using Photon.Pun;

namespace System.Judgement {
    public class MERobotIncome : MonoBehaviourPun {
        private void OnEnable() {
            Events.AddListener(Events.F_ROBOT_ACQUIRE_COINS, OnRobotAcquireCoins);
            Events.AddListener(Events.F_ROBOT_ACQUIRE_COMPONENT, OnRobotAcquireComponent);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_ROBOT_ACQUIRE_COINS, OnRobotAcquireCoins);
            Events.RemoveListener(Events.F_ROBOT_ACQUIRE_COMPONENT, OnRobotAcquireComponent);
        }

        private static void OnRobotAcquireCoins(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                Summary.team.robots[(int) args[1]].inventory.Add(new Coin((int) args[2]));
            }
        }

        private static void OnRobotAcquireComponent(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                Summary.team.robots[(int) args[1]].inventory.Add(new SensorItemAdapter((string) args[2], (int) args[3]));
            }
        }
    }
}