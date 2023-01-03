using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MTViewHealth : MonoBehaviour {
        private GameObject sliderObject;
        private Slider robotSlider;
        private int robotId = -1;

        private void Awake() {
            sliderObject = transform.Find("Slider").gameObject;
            robotSlider = sliderObject.GetComponent<Slider>();
            sliderObject.SetActive(false);
        }

        private void OnEnable() {
            Events.AddListener(Events.F_BODY_HEALTH_CHANGED, ChangeBodyHealth);
            Events.AddListener(Events.M_ROBOT_MONITOR, OnMonitor);
            //Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_BODY_HEALTH_CHANGED, ChangeBodyHealth);
            Events.RemoveListener(Events.M_ROBOT_MONITOR, OnMonitor);
        }

        private void OnMonitor(object[] args) {
            if (Equals((Player) args[1], PhotonNetwork.LocalPlayer)) {
                if ((bool) args[2]) {
                    robotId = (int) args[0];
                    robotSlider.maxValue = Summary.team.robots[robotId].maxHealth;
                    sliderObject.SetActive(true);
                } else {
                    robotId = -1;
                    sliderObject.SetActive(false);
                }
            }
        }

        private void ChangeBodyHealth(object[] args) {
            if ((int) args[0] == robotId) {
                robotSlider.value = (int) args[1];
                robotSlider.maxValue = Summary.team.robots[robotId].maxHealth;
            }
        }
        /*private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }*/
        
    }
}