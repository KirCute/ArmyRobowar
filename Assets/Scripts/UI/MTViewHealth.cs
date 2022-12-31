using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MTViewHealth : MonoBehaviour {
        public Slider robotSlider;
        private int robotId;

        private void OnEnable() {
            Events.AddListener(Events.F_BODY_HEALTH_CHANGED, ChangeBodyHealth);
            Events.AddListener(Events.M_ROBOT_CONTROL, IsControl);
            Events.AddListener(Events.M_ROBOT_MONITOR, IsMonitor);
        }
        
        private void OnDisable() {
            Events.RemoveListener(Events.F_BODY_HEALTH_CHANGED, ChangeBodyHealth);
            Events.RemoveListener(Events.M_ROBOT_CONTROL, IsControl);
            Events.RemoveListener(Events.M_ROBOT_MONITOR, IsMonitor);
        }

        private void IsControl(object[] args) {
            if (Equals((Player)args[1],PhotonNetwork.LocalPlayer)) {
                robotId = (int)args[0];
            }
        }

        private void IsMonitor(object[] args) {
            if (Equals((Player)args[1], PhotonNetwork.LocalPlayer) && (bool)args[2]) {
                robotId = (int)args[0];
            }
        }

        private void ChangeBodyHealth(object[] args) {
            if ((int)args[0] == robotId) {
                robotSlider.value = (float)args[1]/Summary.team.robots[robotId].maxHealth;
            }
        }
    }
}