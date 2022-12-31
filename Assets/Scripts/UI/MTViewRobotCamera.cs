using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace UI {
    public class MTViewRobotCamera : MonoBehaviour {
        private int viewingRobot { get; set; } = -1;
        
        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MONITOR, OnMonitored);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MONITOR, OnMonitored);
        }

        private void Update() {
            if (viewingRobot == -1) return;
            
            // TODO 显示控制按钮
            
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Events.Invoke(Events.M_ROBOT_MONITOR, new object[] {viewingRobot, PhotonNetwork.LocalPlayer, false});
            }
        }

        private void OnMonitored(object[] args) {
            if (PhotonNetwork.LocalPlayer.Equals((Player) args[1])) {
                if ((bool) args[2]) {
                    viewingRobot = (int) args[0];
                } else {
                    viewingRobot = -1;
                    GetComponent<MTViewRobotsPage>().enabled = true;
                }
            }
        }
    }
}