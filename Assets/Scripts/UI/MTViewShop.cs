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
        private const string VIEW_SHOP_TITLE = "";
        private Vector2 scroll = Vector2.zero;
        private int baseId = 6;
        private readonly Dictionary<string, string> robotNames = new();
        private readonly Dictionary<string, string> robotNameErrors = new();

        private void OnEnable() {
            if (baseId == 6 || !Summary.team.bases.Keys.Contains(baseId)) SwitchNextBase();
            var robotNamesKey = robotNames.Keys.ToList();
            foreach (var key in robotNamesKey) robotNames[key] = "";
            var robotNameErrorsKey = robotNameErrors.Keys.ToList();
            foreach (var key in robotNameErrorsKey) robotNameErrors[key] = "";
            Events.AddListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
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
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("命名:", GUILayout.ExpandWidth(false)); 
                    robotNames[goods] = GUILayout.TextField(robotNames[goods], GUILayout.ExpandWidth(true));
                    GUILayout.Label($"{robotNameErrors[goods], -16}", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }

                foreach (var goods in Summary.team.availableSensorTemplates) {
                    GUILayout.BeginHorizontal("Box");
                    var template = Constants.SENSOR_TEMPLATES[goods];
                    GUILayout.Label($"{template.name} (${template.cost})", GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("购买", GUILayout.ExpandWidth(false))) {
                        Events.Invoke(Events.M_TEAM_BUY_COMPONENT, new object[] {Summary.team.teamColor, goods});
                    }

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
    }
}