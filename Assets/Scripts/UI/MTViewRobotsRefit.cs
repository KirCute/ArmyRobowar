using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.Equipment;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MTViewRobotsRefit : MonoBehaviour
    {
        private List<Sensor> Temp_Component_Self_Use = new List<Sensor>(); 

        private const int VIEW_REFIT_PAGE_ID = 0;
        private const float VIEW_REFIT_PAGE_WIDTH = 0.8F;
        private const float VIEW_REFIT_PAGE_HEIGHT = 0.8F;
        private const string VIEW_REFIT_PAGE_TITLE = "";
        private Vector2 scroll = Vector2.zero;
        private Robot myRobot;
        private int componentPos;
        private string componentStr = "";
        private Dictionary<int, int> componentType = new Dictionary<int, int>();
        
        private void Start() {
            
        }

        private string componentToString(Sensor[] component) {
            string temp = "  ";
            int i = 0;
            foreach (var sensor in component) {
                temp = temp + i +":"+sensor.template.name+"  ";
            }
            
            return temp;
        }
        private void OnGUI()
        {
            if (!Summary.isGameStarted) return; //游戏未开始
            var dim = new Rect(Screen.width * (1 - VIEW_REFIT_PAGE_WIDTH) / 2,
                Screen.height * (1 - VIEW_REFIT_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_REFIT_PAGE_WIDTH,
                Screen.height * VIEW_REFIT_PAGE_HEIGHT);
            GUILayout.Window(VIEW_REFIT_PAGE_ID, dim, _ =>
            {
                GUILayout.BeginHorizontal("Box");
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT));
                GUILayout.BeginVertical("Box");
                foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(robot.name,GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("查看机器人", GUILayout.ExpandWidth(false))) {
                        componentType.Clear();
                        myRobot = robot;
                        componentStr = componentToString(robot.equippedComponents);
                        var cnt = 0;
                        foreach (var component in robot.equippedComponents) {
                            componentType.Add(cnt++,component.template.type);
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(componentStr);
                    if (GUILayout.Button("收起",GUILayout.ExpandWidth(false))) {
                        componentStr = "";
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();
                
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT));
                GUILayout.BeginVertical("Box");

                for (int i = 0; i < Summary.team.components.Count; i++) {
                    var component = Summary.team.components[i];
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(component.template.name,GUILayout.ExpandWidth(true));//配件名字（或者是贴图）未解决
                    if (GUILayout.Button("装配", GUILayout.ExpandWidth(false))) {
                        if (componentType.Count>=myRobot.template.capacity) {
                            
                        }else if (componentType.ContainsValue(component.template.type)) {
                            
                        }else {
                            Events.Invoke(Events.M_ROBOT_INSTALL_COMPONENT, new object[]
                            {
                                Summary.team.teamColor, myRobot.id, componentType.Count, i
                            });
                            componentType.Add(componentType.Count,component.template.type);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.BeginHorizontal();
            },VIEW_REFIT_PAGE_TITLE);
            if (Input.GetKeyDown(KeyCode.Escape)) {
                enabled = false;
                GetComponent<MEMainCameraController>().active = true;
            }
        }
    }
}