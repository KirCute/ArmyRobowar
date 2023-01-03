using System;
using System.Linq;
using Model.Equipment;
using Model.Equipment.Template;
using Photon.Pun;
using UnityEngine;

namespace UI {
    public class MTViewRobotsPage : MonoBehaviourPunCallbacks {
        private const int VIEW_ROBOTS_PAGE_ID = 0;
        private const float VIEW_ROBOTS_PAGE_WIDTH = 0.8F;
        private const float VIEW_ROBOTS_PAGE_HEIGHT = 0.8F;
        private const string VIEW_ROBOTS_PAGE_TITLE = "";

        private Vector2 scroll = Vector2.zero;

        private void OnGUI() {
            if (!Summary.isGameStarted) return; // 游戏未开始
            var dim = new Rect(
                Screen.width * (1 - VIEW_ROBOTS_PAGE_WIDTH) / 2, Screen.height * (1 - VIEW_ROBOTS_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_ROBOTS_PAGE_WIDTH, Screen.height * VIEW_ROBOTS_PAGE_HEIGHT
            );
            GUILayout.Window(VIEW_ROBOTS_PAGE_ID, dim, _ => {
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_ROBOTS_PAGE_HEIGHT));

                foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                    // 先展示未失联的机器人
                    GUILayout.BeginHorizontal("Box"); // 单独的机器人条目

                    GUILayout.BeginVertical(); // 左侧名称和血条
                    GUILayout.Label($"{robot.name}{(robot.controller == null ? "" : $" ({robot.controller.NickName} 正在控制)")}{(robot.atHome && robot.controller == null ? " (可改装)" : "")}");
                    GUILayout.Label(new string('❤', robot.health / 3));
                    GUILayout.EndVertical();

                    if (robot.equippedComponents.Any(c => c != null && c.template.type == SensorTemplate.SENSOR_TYPE_CAMERA)) {
                        if (GUILayout.Button("查看画面")) {
                            Events.Invoke(Events.M_ROBOT_MONITOR, new object[] {robot.id, PhotonNetwork.LocalPlayer, true});
                            enabled = false;
                        }
                    } else {
                        GUILayout.Label("无摄像机");
                    }

                    GUILayout.EndHorizontal();
                }

                foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_MANUFACTURING)) {
                    // 再展示制造中的机器人
                    GUILayout.BeginVertical("Box"); // 单独的机器人条目
                    GUILayout.Label(robot.name);
                    var restTime = robot.template.makingTime + robot.createTime - PhotonNetwork.Time;
                    GUILayout.Label(restTime > 0.0 ? $"制造中，剩余时间：{restTime:0.00}s" : "发车点被挤占，正在等待清空发车点。");
                    GUILayout.EndVertical();
                }

                foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_MISSING)) {
                    // 最后展示已失联的机器人
                    GUILayout.BeginVertical("Box"); // 单独的机器人条目
                    GUILayout.Label(robot.name);
                    GUILayout.Label("已失联");
                    GUILayout.EndVertical();
                }

                GUILayout.EndScrollView();
            }, VIEW_ROBOTS_PAGE_TITLE);

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) {
                enabled = false;
                GetComponent<MEMainCameraController>().active = true;
            }
        }
        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }

        public override void OnDisable() {
            base.OnDisable();
            Events.RemoveListener(Events.F_GAME_OVER, OnGameOver);
        }
    }
}