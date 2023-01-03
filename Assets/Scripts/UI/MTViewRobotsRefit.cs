using System;
using System.Collections.Generic;
using System.Linq;
using Model.Equipment;
using Model.Equipment.Template;
using Photon.Pun;
using UnityEngine;

namespace UI {
    public class MTViewRobotsRefit : MonoBehaviourPunCallbacks {
        private const int VIEW_REFIT_PAGE_ID = 0;
        private const float VIEW_REFIT_PAGE_WIDTH = 0.8F;
        private const float VIEW_REFIT_PAGE_HEIGHT = 0.8F;
        private const string VIEW_REFIT_PAGE_TITLE = "";
        private Vector2 robotScroll = Vector2.zero;
        private Vector2 detailScroll = Vector2.zero;
        private Vector2 componentScroll = Vector2.zero;
        private Robot myRobot;
        private int componentPos;

        private void OnGUI() {
            if (!Summary.isGameStarted) return; //游戏未开始
            var dim = new Rect(Screen.width * (1 - VIEW_REFIT_PAGE_WIDTH) / 2,
                Screen.height * (1 - VIEW_REFIT_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_REFIT_PAGE_WIDTH,
                Screen.height * VIEW_REFIT_PAGE_HEIGHT);
            GUILayout.Window(VIEW_REFIT_PAGE_ID, dim, _ => {
                GUILayout.BeginHorizontal("Box");
                GUILayout.BeginVertical();
                GUILayout.BeginVertical("Box");
                robotScroll = GUILayout.BeginScrollView(robotScroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT / 2F));
                foreach (var robot in Summary.team.robots.Values.Where(r => r.atHome)) {
                    GUILayout.BeginVertical("Box");
                    GUILayout.Label(robot.name, GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("查看机器人", GUILayout.ExpandWidth(false))) {
                        myRobot = robot;
                    }

                    GUILayout.EndVertical();
                }

                GUILayout.EndScrollView();
                GUILayout.EndVertical();

                if (myRobot != null) {
                    GUILayout.BeginVertical("Box");
                    detailScroll = GUILayout.BeginScrollView(detailScroll, false, false,
                        GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT / 2F));
                    GUILayout.Label(myRobot.name);
                    GUILayout.BeginVertical("Box");
                    GUILayout.Label("已安装配件：", GUILayout.ExpandWidth(true));
                    for (var i = 0; i < myRobot.equippedComponents.Length; i++) {
                        var component = myRobot.equippedComponents[i];
                        GUILayout.BeginHorizontal("Box");
                        GUILayout.Label($"{i} - {(component == null ? "空" : component.template.name)}",
                            GUILayout.ExpandWidth(true));
                        if (component != null && GUILayout.Button("拆卸", GUILayout.ExpandWidth(false))) {
                            Events.Invoke(Events.M_ROBOT_UNINSTALL_COMPONENT, new object[] {
                                Summary.team.teamColor, myRobot.id, i
                            });
                        }

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                    if (myRobot.inventoryCapacity > 0) {
                        GUILayout.BeginVertical("Box");
                        GUILayout.BeginHorizontal();
                        GUILayout.Label($"物品栏 (容量: {myRobot.inventoryCapacity})：", GUILayout.ExpandWidth(true));
                        if (GUILayout.Button("卸货", GUILayout.ExpandWidth(false))) {
                            Events.Invoke(Events.M_ROBOT_RELEASE_INVENTORY, new object[] {
                                Summary.team.teamColor, myRobot.id
                            });
                        }

                        GUILayout.EndHorizontal();
                        for (var i = 0; i < myRobot.inventory.Count; i++) {
                            GUILayout.BeginHorizontal("Box");
                            GUILayout.Label($"{i} - {myRobot.inventory[i].name}", GUILayout.ExpandWidth(true));
                            GUILayout.EndHorizontal();
                        }

                        GUILayout.EndVertical();
                    }

                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();

                componentScroll = GUILayout.BeginScrollView(componentScroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT));
                GUILayout.BeginVertical("Box");

                for (var i = 0; i < Summary.team.components.Count; i++) {
                    var component = Summary.team.components[i];
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(component.template.name, GUILayout.ExpandWidth(true)); //配件名字（或者是贴图）未解决
                    if (myRobot != null && GUILayout.Button("装配", GUILayout.ExpandWidth(false))) {
                        var componentType = new List<int>();
                        var firstNull = -1;
                        for (var j = 0; j < myRobot.equippedComponents.Length; j++) {
                            if (myRobot.equippedComponents[j] == null) {
                                if (firstNull == -1) firstNull = j;
                            } else {
                                componentType.Add(myRobot.equippedComponents[j].template.type);
                            }
                        }

                        if (firstNull == -1) {
                            // TODO
                        } else if (component.template.type is SensorTemplate.SENSOR_TYPE_CAMERA or
                                       SensorTemplate.SENSOR_TYPE_LIDAR or
                                       SensorTemplate.SENSOR_TYPE_GUN or
                                       SensorTemplate.SENSOR_TYPE_ENGINEER &&
                                   componentType.Contains(component.template.type)) {
                            // TODO
                        } else {
                            Events.Invoke(Events.M_ROBOT_INSTALL_COMPONENT, new object[] {
                                Summary.team.teamColor, myRobot.id, firstNull, i
                            });
                        }
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
            }, VIEW_REFIT_PAGE_TITLE);
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) {
                enabled = false;
                myRobot = null;
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