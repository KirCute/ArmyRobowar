using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEditor;

namespace UI {
    public class MTViewShop : MonoBehaviourPun {
        private const int VIEW_SHOP_ID = 0;
        private const float VIEW_SHOP_WIDTH = 0.8F;
        private const float VIEW_SHOP_HEIGHT = 0.7F;
        private const string VIEW_SHOP_TITLE = "";
        private Vector2 scroll = Vector2.zero;
        private int baseId = 6;
        private readonly Dictionary<string, string> robotNames = new();
        private readonly Dictionary<string, string> robotNameErrors = new();
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
            var robotNameErrorsKey = robotNameErrors.Keys.ToList();
            foreach (var key in robotNameErrorsKey) robotNameErrors[key] = "";
            Events.AddListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
            
            robotImg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ShopImage/robot.png");
            cameraImg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ShopImage/camera.png");
            gunImg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ShopImage/gun.png");
            lidarImg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ShopImage/lidar.png");
            inventoryImg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ShopImage/inventory.png");
            armorImg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ShopImage/armor.png");
            engineerImg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ShopImage/engineer.png");
            towerImg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ShopImage/tower.png");

        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
            Events.RemoveListener(Events.F_GAME_OVER, OnGameOver);
           // a = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Scripts/UI/image/Lider.png");
        }

        private void OnBaseDestroyed(object[] args) {
            if (baseId == (int) args[0]) SwitchNextBase();
        }

        private void OnGUI() {
            foreach (var template in Summary.team.availableRobotTemplates) {
                if (!robotNames.ContainsKey(template)) robotNames.Add(template, "");
                if (!robotNameErrors.ContainsKey(template)) robotNameErrors.Add(template, "");
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
                    GUILayout.Label(getImage(goods),GUILayout.ExpandWidth(false));
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();
                    var template = Constants.ROBOT_TEMPLATES[goods];
                    GUILayout.Label($"{template.name} (${template.cost})", GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("购买", GUILayout.ExpandWidth(false))) {
                        if (robotNames[goods] == "") {
                            robotNameErrors[goods] = "请输入机器人名称";
                        } else if (Summary.team.robots.Values.Any(robot => robot.name == robotNames[goods])) {
                            robotNameErrors[goods] = "该名称已被使用";
                        } else {
                            Events.Invoke(Events.M_CREATE_ROBOT, new object[] {baseId, goods, robotNames[goods]});
                            robotNameErrors[goods] = "";
                            robotNames[goods] = "";
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("命名:", GUILayout.ExpandWidth(false));
                    GUIStyle styleTemp = new GUIStyle(GUI.skin.textField);
                    styleTemp.fontSize = 30;
                    robotNames[goods] = GUILayout.TextField(robotNames[goods],9,styleTemp, GUILayout.ExpandWidth(true));
                    GUILayout.Label($"{robotNameErrors[goods], -16}", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("Box");
                    GUIStyle styleTempForLabel = new GUIStyle(GUI.skin.label);
                    styleTempForLabel.normal.textColor = Color.yellow;
                    styleTempForLabel.fontSize = 15;
                    GUILayout.Label(Constants.TECHNOLOGY[goods].description,styleTempForLabel);
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }

                foreach (var goods in Summary.team.availableSensorTemplates) {
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(getImage(goods),GUILayout.ExpandWidth(false));
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                    var template = Constants.SENSOR_TEMPLATES[goods];
                    GUILayout.Label($"{template.name} (${template.cost})", GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("购买", GUILayout.ExpandWidth(false))) {
                        Events.Invoke(Events.M_TEAM_BUY_COMPONENT, new object[] {Summary.team.teamColor, goods});
                    }

                    GUILayout.EndHorizontal();
                    GUIStyle styleTempForLabel = new GUIStyle(GUI.skin.label);
                    styleTempForLabel.normal.textColor = Color.yellow;
                    styleTempForLabel.fontSize = 15;
                    GUILayout.Label(Constants.TECHNOLOGY[goods].description,styleTempForLabel);
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
        
        private Texture2D getImage(string tech) {
            if (Constants.ROBOT_TEMPLATES.ContainsKey(tech)) {
                return robotImg;
            } else if (Constants.SENSOR_TEMPLATES.ContainsKey(tech)) {
                switch (Constants.SENSOR_TEMPLATES[tech].type) {
                    case 0:
                        return cameraImg;
                    case 1:
                        return gunImg;
                    case 2:
                        return lidarImg;
                    case 3:
                        return inventoryImg;
                    case 4:
                        return armorImg;
                    case 5:
                        return engineerImg;
                    default:
                        return null;
                        

                }
            }
            else {
                return towerImg;
            }
            
        }
    }
}