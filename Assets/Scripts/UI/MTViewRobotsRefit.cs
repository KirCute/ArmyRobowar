﻿using System;
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
        private const string VIEW_REFIT_PAGE_TITLE = "改装";
        private Vector2 robotScroll = Vector2.zero;
        private Vector2 detailScroll = Vector2.zero;
        private Vector2 componentScroll = Vector2.zero;
        private Robot myRobot;
        private int componentPos;
        private Texture2D armorImg;
        private Texture2D cameraImg;
        private Texture2D gunImg;
        private Texture2D lidarImg;
        private Texture2D inventoryImg;
        private Texture2D robotImg;
        private Texture2D engineerImg;

        private void OnGUI() {
            if (!Summary.isGameStarted) return; //游戏未开始
            var dim = new Rect(Screen.width * (1 - VIEW_REFIT_PAGE_WIDTH) / 2,
                Screen.height * (1 - VIEW_REFIT_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_REFIT_PAGE_WIDTH,
                Screen.height * VIEW_REFIT_PAGE_HEIGHT);
            GUILayout.Window(VIEW_REFIT_PAGE_ID, dim, _ => {
                GUILayout.BeginHorizontal("Box");
                GUILayout.BeginVertical();
                
                robotScroll = GUILayout.BeginScrollView(robotScroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT / 2F),GUILayout.Width(Screen.width * VIEW_REFIT_PAGE_WIDTH/2));
                GUILayout.BeginVertical("Box");
                foreach (var robot in Summary.team.robots.Values.Where(r => r.atHome)) {
                    
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(robotImg,GUILayout.ExpandWidth(false));
                    GUILayout.Label(robot.name, GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("查看机器人", GUILayout.ExpandWidth(false))) {
                        myRobot = robot;
                    }
                    GUILayout.EndHorizontal();
                    
                    
                }
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                
                if (myRobot != null) {
                    GUILayout.BeginVertical("Box");
                    detailScroll = GUILayout.BeginScrollView(detailScroll, false, false,
                        GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT / 2F));
                    GUILayout.BeginVertical("Box");
                    GUILayout.Label("已安装配件：", GUILayout.ExpandWidth(true));
                    for (var i = 0; i < myRobot.equippedComponents.Length; i++) {
                        var component = myRobot.equippedComponents[i];
                        GUILayout.BeginHorizontal("Box");
                        //GUILayout.Label(getImage(component.template.type),GUILayout.ExpandWidth(false));
                        var styleTemp = new GUIStyle(GUI.skin.label) {
                            fontSize = 20
                        };
                        GUILayout.Label($"{i} - {(component == null ? "空" : component.template.name)}",styleTemp,
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
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT),GUILayout.Width(Screen.width * VIEW_REFIT_PAGE_WIDTH/2));
                GUILayout.BeginVertical("Box");

                for (var i = 0; i < Summary.team.components.Count; i++) {
                    var component = Summary.team.components[i];
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(GetImage(component.template.type),GUILayout.ExpandWidth(false));
                    GUILayout.Label(component.template.name, GUILayout.ExpandWidth(true)); 
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
        private Texture2D GetImage(int tech) {
            return tech switch {
                0 => cameraImg,
                1 => gunImg,
                2 => lidarImg,
                3 => inventoryImg,
                4 => armorImg,
                5 => engineerImg,
                _ => null
            };
        }
        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
            armorImg = Resources.Load("ShopImage/armor") as Texture2D;
            cameraImg = Resources.Load("ShopImage/camera") as Texture2D;
            gunImg = Resources.Load("ShopImage/gun") as Texture2D;
            inventoryImg = Resources.Load("ShopImage/inventory") as Texture2D;
            lidarImg = Resources.Load("ShopImage/lidar") as Texture2D;
            robotImg = Resources.Load("ShopImage/robot") as Texture2D;
            engineerImg = Resources.Load("ShopImage/engineer") as Texture2D;
        }

        public override void OnDisable() {
            base.OnDisable();
            Events.RemoveListener(Events.F_GAME_OVER, OnGameOver);
        }
    }
}