using System;
using System.Collections.Generic;
using System.Linq;
using Model.Equipment;
using Model.Equipment.Template;
using UnityEngine;

namespace UI {
    public class MTViewRobotsRefit : MonoBehaviour {
        private const int VIEW_REFIT_PAGE_ID = 0;
        private const float VIEW_REFIT_PAGE_WIDTH = 0.8F;
        private const float VIEW_REFIT_PAGE_HEIGHT = 0.8F;
        private const string VIEW_REFIT_PAGE_TITLE = "";
        private Vector2 scroll = Vector2.zero;
        private Robot myRobot;
        private int componentPos;
        private string componentStr = "";
        private readonly List<int> componentType = new();

        private static string ComponentToString(IEnumerable<Sensor> component) {
            var i = 0;

            return component.Aggregate("  ",
                (current, sensor) => $"{current}{i++}:{(sensor == null ? "空" : sensor.template.name)}  ");
        }

        private void OnGUI() {
            if (!Summary.isGameStarted) return; //游戏未开始
            var dim = new Rect(Screen.width * (1 - VIEW_REFIT_PAGE_WIDTH) / 2,
                Screen.height * (1 - VIEW_REFIT_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_REFIT_PAGE_WIDTH,
                Screen.height * VIEW_REFIT_PAGE_HEIGHT);
            GUILayout.Window(VIEW_REFIT_PAGE_ID, dim, _ => {
                GUILayout.BeginHorizontal("Box");
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT));
                GUILayout.BeginVertical("Box");
                foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(robot.name, GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("查看机器人", GUILayout.ExpandWidth(false))) {
                        componentType.Clear();
                        myRobot = robot;
                        componentStr = ComponentToString(robot.equippedComponents);
                        foreach (var component in robot.equippedComponents) {
                            if (component != null) componentType.Add(component.template.type);
                        }
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(componentStr);
                    if (GUILayout.Button("收起", GUILayout.ExpandWidth(false))) {
                        componentStr = "";
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();
                GUILayout.EndScrollView();

                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT));
                GUILayout.BeginVertical("Box");

                for (var i = 0; i < Summary.team.components.Count; i++) {
                    var component = Summary.team.components[i];
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(component.template.name, GUILayout.ExpandWidth(true)); //配件名字（或者是贴图）未解决
                    if (myRobot != null && GUILayout.Button("装配", GUILayout.ExpandWidth(false))) {
                        if (componentType.Count >= myRobot.template.capacity) {
                            // TODO
                        } else if (component.template.type is SensorTemplate.SENSOR_TYPE_CAMERA or
                                       SensorTemplate.SENSOR_TYPE_LIDAR or
                                       SensorTemplate.SENSOR_TYPE_GUN or
                                       SensorTemplate.SENSOR_TYPE_ENGINEER &&
                                   componentType.Contains(component.template.type)) {
                            // TODO
                        } else {
                            Events.Invoke(Events.M_ROBOT_INSTALL_COMPONENT, new object[] {
                                Summary.team.teamColor, myRobot.id, componentType.Count, i
                            });
                            componentType.Add(component.template.type);
                            componentStr = ComponentToString(myRobot.equippedComponents);
                        }
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
            }, VIEW_REFIT_PAGE_TITLE);
            if (Input.GetKeyDown(KeyCode.Escape)) {
                enabled = false;
                GetComponent<MEMainCameraController>().active = true;
            }
        }
    }
}