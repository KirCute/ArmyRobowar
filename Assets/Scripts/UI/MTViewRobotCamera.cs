using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MTViewRobotCamera : MonoBehaviour {
        private int viewingRobot { get; set; } = -1;
        [SerializeField] private MTViewRobotsPage frontPage;
        private Button button;
        private Text text;

        private void Awake() {
            button = transform.Find("Button").GetComponent<Button>();
            text = button.transform.Find("Text").GetComponent<Text>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MONITOR, OnMonitored);
            Events.AddListener(Events.F_ROBOT_LOST_CONNECTION, OnLostConnection);
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MONITOR, OnMonitored);
            Events.RemoveListener(Events.F_ROBOT_LOST_CONNECTION, OnLostConnection);
            Events.RemoveListener(Events.F_GAME_OVER, OnGameOver);
        }

        private void Update() {
            if (viewingRobot == -1) return;

            if (Summary.team.robots[viewingRobot].controller == null) {
                button.interactable = true;
                text.text = "取得控制权";
            } else if (!Summary.team.robots[viewingRobot].controller.Equals(PhotonNetwork.LocalPlayer)) {
                button.interactable = false;
                text.text = $"控制者：{Summary.team.robots[viewingRobot].controller.NickName}";
            }
            
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Events.Invoke(Events.M_ROBOT_MONITOR, new object[] {viewingRobot, PhotonNetwork.LocalPlayer, false});
            }
        }

        public void OnButtonDown() {
            Events.Invoke(Events.M_ROBOT_CONTROL, new object[] {viewingRobot, PhotonNetwork.LocalPlayer});
            button.gameObject.SetActive(false);
        }

        private void OnMonitored(object[] args) {
            if (PhotonNetwork.LocalPlayer.Equals((Player) args[1])) {
                if ((bool) args[2]) {
                    viewingRobot = (int) args[0];
                    button.gameObject.SetActive(true);
                } else {
                    viewingRobot = -1;
                    button.gameObject.SetActive(false);
                    frontPage.enabled = true;
                }
            }
        }

        private void OnLostConnection(object[] args) {
            if (viewingRobot == (int) args[0]) {
                Events.Invoke(Events.M_ROBOT_MONITOR, new object[] {viewingRobot, PhotonNetwork.LocalPlayer, false});
            }
        }
        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }

        
    }
}