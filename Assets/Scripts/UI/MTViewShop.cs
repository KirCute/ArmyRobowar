using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

namespace UI {
    public class MTViewShop : MonoBehaviourPun {
        private const int VIEW_SHOP_ID = 0;
        private const float VIEW_SHOP_WIDTH = 0.8F;
        private const float VIEW_SHOP_HEIGHT = 0.7F;
        private const string VIEW_SHOP_TITLE = "商店";
        private Vector2 scroll = Vector2.zero;
        private int baseId = 6;
        private readonly Dictionary<string, string> robotNames = new();
        private readonly Dictionary<string, string> robotErrors = new();
        private readonly Dictionary<string, string> componentErrors = new();
        
        private Texture2D robotImg;
        private Texture2D cameraImg;
        private Texture2D gunImg;
        private Texture2D lidarImg;
        private Texture2D inventoryImg;
        private Texture2D armorImg;
        private Texture2D engineerImg;
        private Texture2D towerImg;
        
        private void OnEnable() {
            if (baseId == 6 || !Summary.team.bases.Keys.Contains(baseId)) SwitchNextBase();
            var robotNamesKey = robotNames.Keys.ToList();
            foreach (var key in robotNamesKey) robotNames[key] = "";
            var robotNameErrorsKey = robotErrors.Keys.ToList();
            foreach (var key in robotNameErrorsKey) robotErrors[key] = "";
            var componentErrorsKey = componentErrors.Keys.ToList();
            foreach (var key in componentErrorsKey) componentErrors[key] = "";
            Events.AddListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);

            armorImg = Resources.Load("ShopImage/armor") as Texture2D;
            cameraImg = Resources.Load("ShopImage/camera") as Texture2D;
            gunImg = Resources.Load("ShopImage/gun") as Texture2D;
            inventoryImg = Resources.Load("ShopImage/inventory") as Texture2D;
            lidarImg = Resources.Load("ShopImage/lidar") as Texture2D;
            robotImg = Resources.Load("ShopImage/robot") as Texture2D;
            towerImg = Resources.Load("ShopImage/tower") as Texture2D;
            engineerImg = Resources.Load("ShopImage/engineer") as Texture2D;
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
            Events.RemoveListener(Events.F_GAME_OVER, OnGameOver);
        }

        private void OnBaseDestroyed(object[] args) {
            if (baseId == (int) args[0]) SwitchNextBase();
        }

        private void OnGUI() {
            foreach (var template in Summary.team.availableRobotTemplates) {
                if (!robotNames.ContainsKey(template)) robotNames.Add(template, "");
                if (!robotErrors.ContainsKey(template)) robotErrors.Add(template, "");
            }
            foreach (var template in Summary.team.availableSensorTemplates) {
                if (!componentErrors.ContainsKey(template)) componentErrors.Add(template, "");
            }

            var dim = new Rect(
                Screen.width * (1 - VIEW_SHOP_WIDTH) / 2, Screen.height * (1 - VIEW_SHOP_HEIGHT) / 2,
                Screen.width * VIEW_SHOP_WIDTH, Screen.height * VIEW_SHOP_HEIGHT
            );
            GUILayout.Window(VIEW_SHOP_ID, dim, _ => {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"当前机器人出生基地：{baseId}", GUILayout.ExpandWidth(false));
                if (GUILayout.Button(">", GUILayout.ExpandWidth(false))) SwitchNextBase();
                GUILayout.Label("", GUILayout.ExpandWidth(true));
                GUILayout.Label($"剩余货币：{Summary.team.coins,-8}", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_SHOP_HEIGHT));
                GUILayout.BeginVertical("Box");

                foreach (var goods in Summary.team.availableRobotTemplates) {
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(GetImage(goods), GUILayout.ExpandWidth(false));
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();
                    var template = Constants.ROBOT_TEMPLATES[goods];
                    GUILayout.Label($"{template.name} (${template.cost})", GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("购买", GUILayout.ExpandWidth(false))) {
                        if (robotNames[goods] == "") {
                            robotErrors[goods] = "请输入机器人名称";
                        } else if (Summary.team.robots.Values.Any(robot => robot.name == robotNames[goods])) {
                            robotErrors[goods] = "该名称已被使用";
                        } else if (Summary.team.coins < template.cost) {
                            robotErrors[goods] = "货币不足";
                        } else {
                            Events.Invoke(Events.M_CREATE_ROBOT, new object[] {baseId, goods, robotNames[goods]});
                            robotErrors[goods] = "";
                            robotNames[goods] = "";
                        }
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("命名:", GUILayout.ExpandWidth(false));
                    var styleTemp = new GUIStyle(GUI.skin.textField) {
                        fontSize = 30
                    };
                    robotNames[goods] =
                        GUILayout.TextField(robotNames[goods], 9, styleTemp, GUILayout.ExpandWidth(true));
                    GUILayout.Label($"{robotErrors[goods],-10}", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("Box");
                    var styleTempForLabel = new GUIStyle(GUI.skin.label) {
                        normal = {
                            textColor = Color.yellow
                        },
                        fontSize = 15
                    };
                    GUILayout.Label(Constants.TECHNOLOGY[goods].description, styleTempForLabel);
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }

                foreach (var goods in Summary.team.availableSensorTemplates) {
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(GetImage(goods), GUILayout.ExpandWidth(false));
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                    var template = Constants.SENSOR_TEMPLATES[goods];
                    GUILayout.Label($"{template.name} (${template.cost})", GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("购买", GUILayout.ExpandWidth(false))) {
                        if (Summary.team.coins < template.cost) {
                            componentErrors[goods] = "货币不足";
                        } else {
                            componentErrors[goods] = "";
                            Events.Invoke(Events.M_TEAM_BUY_COMPONENT, new object[] {Summary.team.teamColor, goods});
                        }
                    }

                    GUILayout.EndHorizontal();
                    var styleTempForLabel = new GUIStyle(GUI.skin.label) {
                        normal = {
                            textColor = Color.yellow
                        },
                        fontSize = 15
                    };
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(Constants.TECHNOLOGY[goods].description, styleTempForLabel, GUILayout.ExpandWidth(true));
                    GUILayout.Label($"{componentErrors[goods],-10}", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            }, VIEW_SHOP_TITLE);
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) {
                enabled = false;
                GetComponent<MEMainCameraController>().active = true;
            }
        }

        private void SwitchNextBase() {
            var min = Constants.BASE_COUNT;
            var minBigger = Constants.BASE_COUNT;
            foreach (var id in Summary.team.bases.Keys) {
                if (min > id) min = id;
                if (minBigger > id && id > baseId) minBigger = id;
            }

            baseId = minBigger == Constants.BASE_COUNT ? min : minBigger;
        }

        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }

        private Texture2D GetImage(string tech) {
            if (Constants.ROBOT_TEMPLATES.ContainsKey(tech)) {
                return robotImg;
            }

            if (Constants.SENSOR_TEMPLATES.ContainsKey(tech)) {
                return Constants.SENSOR_TEMPLATES[tech].type switch {
                    0 => cameraImg,
                    1 => gunImg,
                    2 => lidarImg,
                    3 => inventoryImg,
                    4 => armorImg,
                    5 => engineerImg,
                    _ => null
                };
            }

            return towerImg;
        }
    }
}