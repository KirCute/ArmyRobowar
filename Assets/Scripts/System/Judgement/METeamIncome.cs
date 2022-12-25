using Model.Equipment;
using UnityEngine;

namespace System.Judgement {
    public class METeamIncome : MonoBehaviour {
        private void OnEnable() {
            Events.AddListener(Events.F_TEAM_ACQUIRE_COINS, OnAcquireCoin);
            Events.AddListener(Events.F_TEAM_ACQUIRE_COMPONENT, OnAcquireComponent);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_TEAM_ACQUIRE_COINS, OnAcquireCoin);
            Events.RemoveListener(Events.F_TEAM_ACQUIRE_COMPONENT, OnAcquireComponent);
        }

        private static void OnAcquireCoin(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                Summary.team.coins += (int) args[1];
            }
        }

        private static void OnAcquireComponent(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                Summary.team.components.Add(
                    new Sensor(Constants.SENSOR_TEMPLATES[(string) args[1]], (int) args[2])
                );
            }
        }
    }
}