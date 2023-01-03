using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MEPickableName : MonoBehaviour {
        private Text text;
        private int robotId;

        private void Awake() {
            text = transform.Find("Text").GetComponent<Text>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MONITOR, OnMonitor);
            Events.AddListener(Events.F_ROBOT_FOUND_PICKABLE, OnFound);
            Events.AddListener(Events.F_ROBOT_LOST_FOUND_PICKABLE, OnLost);
            //Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MONITOR, OnMonitor);
            Events.RemoveListener(Events.F_ROBOT_FOUND_PICKABLE, OnFound);
            Events.RemoveListener(Events.F_ROBOT_LOST_FOUND_PICKABLE, OnLost);
        }

        private void OnFound(object[] args) {
            if (robotId == (int) args[0]) {
                text.text = (string) args[1];
            }
        }

        private void OnLost(object[] args) {
            if (robotId == (int) args[0]) {
                text.text = "";
            }
        }

        private void OnMonitor(object[] args) {
            if (Equals((Player) args[1], PhotonNetwork.LocalPlayer)) {
                if ((bool) args[2]) {
                    robotId = (int) args[0];
                } else {
                    robotId = -1;
                    text.text = "";
                }
            }
        }
        
        /*private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }*/
    }
}