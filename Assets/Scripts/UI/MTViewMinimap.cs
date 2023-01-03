using System;
using Equipment.Sensor.Lidar;
using Model.Equipment.Template;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace UI {
    public class MTViewMinimap : MonoBehaviour {
        [SerializeField] private Camera minimapCamera;
        private GameObject minimap;
        private int robotId = -1;
        private int lidarIndex = -1;
        private int lidarLevel;

        private void Awake() {
            minimap = transform.Find("Image").gameObject;
        }

        private void Update() {
            if (robotId == -1) return;
            var lidar = Summary.team.robots[robotId].equippedComponents[lidarIndex];
            if (lidar == null || lidar.template.type != SensorTemplate.SENSOR_TYPE_LIDAR) {
                robotId = -1;
                minimap.SetActive(false);
                return;
            }

            var pos = GameObject.Find($"Robot_{robotId}").transform.position;
            minimapCamera.transform.position = new Vector3(pos.x, lidarLevel * 10f, pos.z);
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MONITOR, OnMonitored);
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MONITOR, OnMonitored);
        }

        private void OnMonitored(object[] args) {
            if (Equals((Player) args[1], PhotonNetwork.LocalPlayer)) {
                lidarIndex = -1;
                for (var i = 0; i < Summary.team.robots[(int) args[0]].equippedComponents.Length; i++) {
                    var sensor = Summary.team.robots[(int) args[0]].equippedComponents[i];
                    if (sensor != null && sensor.template.type == SensorTemplate.SENSOR_TYPE_LIDAR) {
                        lidarIndex = i;
                        break;
                    }
                }
                if ((bool) args[2] && lidarIndex != -1) {
                    robotId = (int) args[0];
                    var lidar = GameObject.Find($"Component_{robotId}_{lidarIndex}").GetComponent<MTMapBuilder>();
                    lidarLevel = lidar.scanLayer;
                    minimap.SetActive(true);
                } else {
                    robotId = -1;
                    minimap.SetActive(false);
                }
            }
        }
        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }
        
    }
}