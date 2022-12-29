using UnityEngine;

namespace System.Judgement {
    public class MEMapMerger : MonoBehaviour {
        private void OnEnable() {
            Events.AddListener(Events.F_ROBOT_LIDAR_SYNC, OnLidarSync);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_ROBOT_LIDAR_SYNC, OnLidarSync);
        }

        private static void OnLidarSync(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                var cnt = (int) args[1];
                for (var i = 2; i < cnt + 2; i++) {
                    Summary.team.teamMap[(int) args[i]] = true;
                }
            }
        }
    }
}