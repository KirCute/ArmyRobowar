using System;
using System.Collections.Generic;
using System.Linq;
using Map.Navigation;
using Model.Equipment;
using UnityEngine;

namespace UI {
    public class MTViewAutoNavigation : MonoBehaviour{
        
        private const int PICK_ROBOTS_PAGE_ID = 0;//TODO
        private const float PICK_ROBOTS_PAGE_WIDTH = 0.8F;
        private const float PICK_ROBOTS_PAGE_HEIGHT = 0.8F;
        private const string PICK_ROBOTS_PAGE_TITLE = "自主导航路径设置";
        private const int SKETCH_MAP_WIDTH = 920;
        private const int SKETCH_MAP_HEIGHT = 680;
        
        private List<Vector2> positionInSketchMap = new List<Vector2>();
        private List<Vector2> positionInWorld = new List<Vector2>();
        private int selectedRobotId;
        
        
        private Vector2 scroll = Vector2.zero;
        private void OnGUI() {
            if(!Summary.isGameStarted) return;
            var dim = new Rect(
                Screen.width * (1 - PICK_ROBOTS_PAGE_WIDTH) / 2, Screen.height * (1 - PICK_ROBOTS_PAGE_HEIGHT) / 2,
                Screen.width * PICK_ROBOTS_PAGE_WIDTH, Screen.height * PICK_ROBOTS_PAGE_HEIGHT
            );
            GUILayout.Window(PICK_ROBOTS_PAGE_ID, dim, ViewAutoNavigation,PICK_ROBOTS_PAGE_TITLE);

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
                var mapRect = new Rect((Screen.width - SKETCH_MAP_WIDTH) / 2, (Screen.height - SKETCH_MAP_HEIGHT) / 2, 
                    SKETCH_MAP_WIDTH, SKETCH_MAP_HEIGHT);
                Texture map = null;
                //TODO,换成小地图
                GUI.Label(mapRect,map);
                
                if (Input.GetMouseButtonDown(0)) {
                    Vector2 mousePosition = new Vector2((Input.mousePosition.x - (Screen.width - SKETCH_MAP_WIDTH) / 2) / 20,
                        (SKETCH_MAP_HEIGHT - (Input.mousePosition.y - (Screen.height - SKETCH_MAP_HEIGHT) / 2)) / 20);
                    positionInSketchMap.Add(mousePosition);
                }

                if (GUI.Button(new Rect(Screen.width - 10, Screen.height - 10, 10, 10), "开始路径规划")) {
                    //TODO,画线
                    
                    GameObject robot = GameObject.Find($"Robot_{selectedRobotId}");
                    Vector2 robotPosition = new Vector2(robot.transform.position.x, robot.transform.position.z);
                    positionInWorld.Add(robotPosition);

                    //从屏幕坐标转化为世界坐标
                    for (int i = 0; i < positionInSketchMap.Count; i++) {
                        Vector2 position = new Vector2((positionInSketchMap[i].x - 23) / 23 * 61,
                            (positionInSketchMap[i].y - 17) / 17 * 82);
                        positionInWorld.Add(position);
                    }
                    
                    List<Vector2> path = MDNavigationCenter.GetInstance().GetFinalPath(positionInWorld);
                    var input = new object[path.Count + 2];
                    input[0] = selectedRobotId;
                    input[1] = path.Count;
                    for (var i = 2; i < path.Count + 2; i++) input[i] = path[i - 2];
                    Events.Invoke(Events.M_ROBOT_NAVIGATION, input);
                }
            }
            
            GUILayout.EndScrollView();
        }
    }
}