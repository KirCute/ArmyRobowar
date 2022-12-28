using System;
using System.Linq;
using Model.Equipment;
using UnityEngine;

namespace UI {
    public class MTViewAutoNavigation : MonoBehaviour{
        
        private const int VIEW_ROBOTS_PAGE_ID = 0;//TODO
        private const float VIEW_ROBOTS_PAGE_WIDTH = 0.8F;
        private const float VIEW_ROBOTS_PAGE_HEIGHT = 0.8F;
        private const string VIEW_ROBOTS_PAGE_TITLE = "自主导航路径设置";
        
        private Vector2 scroll = Vector2.zero;
        private void OnGUI() {
            if(!Summary.isGameStarted) return;
            var dim = new Rect(
                Screen.width * (1 - VIEW_ROBOTS_PAGE_WIDTH) / 2, Screen.height * (1 - VIEW_ROBOTS_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_ROBOTS_PAGE_WIDTH, Screen.height * VIEW_ROBOTS_PAGE_HEIGHT
            );
            GUILayout.Window(VIEW_ROBOTS_PAGE_ID, dim, ViewAutoNavigation,VIEW_ROBOTS_PAGE_TITLE);

        }

        private void ViewAutoNavigation(int id) {
            scroll = GUILayout.BeginScrollView(scroll, false, false,
                GUILayout.Height(Screen.height * VIEW_ROBOTS_PAGE_HEIGHT));
            GUILayout.BeginHorizontal("Box");
            GUILayout.BeginVertical("Box");
            //TODO,第一块区域是小地图
            if (GUILayout.Button("开始导航")) {
                //TODO
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");//可供选择的机器人列表
            foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                GUILayout.BeginHorizontal("Box"); // 单独的机器人条目
                
                GUILayout.BeginVertical(); // 左侧名称和血条
                GUILayout.Label(robot.name);
                GUILayout.Label(new string('❤', robot.health / 3));
                GUILayout.EndVertical();

                if (GUILayout.Button("选择"))
                {
                    //TODO
                }
                
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();//机器人列表结束
            GUILayout.EndHorizontal();
            
            GUILayout.EndScrollView();
        }
    }
}