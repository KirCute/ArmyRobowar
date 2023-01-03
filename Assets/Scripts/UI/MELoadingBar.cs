using System;
using Equipment.Sensor.Gun;
using Model.Equipment.Template;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MELoadingBar : MonoBehaviourPunCallbacks {
        [SerializeField] private Color filledColor;
        [SerializeField] private Color loadingColor;
        private GameObject ringObject;
        private Image ring;
        private int robotId = -1;
        private int gunIndex = -1;

        private void Awake() {
            ringObject = transform.Find("Image").gameObject;
            ring = ringObject.GetComponent<Image>();
        }

        private void Update() {
            if (robotId == -1) return;
            var gun = Summary.team.robots[robotId].equippedComponents[gunIndex];
            if (gun == null || gun.template.type != SensorTemplate.SENSOR_TYPE_GUN) {
                robotId = -1;
                ringObject.SetActive(false);
                return;
            }

            var fire = GameObject.Find($"Component_{robotId}_{gunIndex}").GetComponent<MEFire>();
            ring.fillAmount = fire.loadProcess;
            ring.color = fire.loadProcess >= 1.0f ? filledColor : loadingColor;
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MONITOR, OnMonitor);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MONITOR, OnMonitor);
        }

        private void OnMonitor(object[] args) {
            if (Equals((Player) args[1], PhotonNetwork.LocalPlayer)) {
                gunIndex = -1;
                for (var i = 0; i < Summary.team.robots[(int) args[0]].equippedComponents.Length; i++) {
                    var sensor = Summary.team.robots[(int) args[0]].equippedComponents[i];
                    if (sensor != null && sensor.template.type == SensorTemplate.SENSOR_TYPE_GUN) {
                        gunIndex = i;
                        break;
                    }
                }
                if ((bool) args[2] && gunIndex != -1) {
                    robotId = (int) args[0];
                    ringObject.SetActive(true);
                } else {
                    robotId = -1;
                    ringObject.SetActive(false);
                }
            }
        }
    }
}