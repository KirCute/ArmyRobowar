using System;
using System.Collections.Generic;
using System.Linq;
using Model.Equipment;
//using Map.Navigation;
//using Model.Equipment;
using UnityEngine;

namespace UI {
    public class MTSelectNavigationRobot : MonoBehaviour{
        
        private const int PICK_ROBOTS_PAGE_ID = 0;//TODO
        private const float PICK_ROBOTS_PAGE_WIDTH = 0.8F;
        private const float PICK_ROBOTS_PAGE_HEIGHT = 0.8F;
        private const string PICK_ROBOTS_PAGE_TITLE = "自主导航路径设置";
        
        public int selectedRobotId;
        
        private Vector2 scroll = Vector2.zero;

        public bool SET_NAVIGATION_PATH = false;
        private void OnGUI() {
            if(!Summary.isGameStarted) return;
            var dim = new Rect(Screen.width * (1 - PICK_ROBOTS_PAGE_WIDTH) / 2,
                Screen.height * (1 - PICK_ROBOTS_PAGE_HEIGHT) / 2,
                Screen.width * PICK_ROBOTS_PAGE_WIDTH,
                Screen.height * PICK_ROBOTS_PAGE_HEIGHT);
            GUILayout.Window(PICK_ROBOTS_PAGE_ID, dim, ViewAutoNavigation,PICK_ROBOTS_PAGE_TITLE);
            if (GUI.Button(new Rect(Screen.width - 10, Screen.height - 10, 10, 10), "下一步，设置导航路径")) {
                
            }

            /*if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) {
                enabled = false;
                GetComponent<MEMainCameraController>().active = true;
            }*/
        }

        private void ViewAutoNavigation(int id) {
            scroll = GUILayout.BeginScrollView(scroll, false, false,
                GUILayout.Height(Screen.height * PICK_ROBOTS_PAGE_HEIGHT));

            GUILayout.BeginHorizontal("Box");//机器人列表水平排布
            foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                GUILayout.BeginVertical("Box"); // 单独的机器人条目
                
                GUILayout.BeginVertical(); // 左侧名称和血条
                GUILayout.Label(robot.name);
                GUILayout.Label(new string('❤', robot.health / 3));
                GUILayout.EndVertical();

                if (GUILayout.Button("选择")) {
                    selectedRobotId = robot.id;
                }
                
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("下一步，设置导航路径")) {
                SET_NAVIGATION_PATH = true;
                this.enabled = false;
            }
            
            GUILayout.EndScrollView();
        }
    }
}
