using System;
using System.Linq;
using Model.Equipment;
using UnityEngine;

namespace UI {
    public class MTViewRobotsPage : MonoBehaviour {
        private const int VIEW_ROBOTS_PAGE_ID = 0;
        private const float VIEW_ROBOTS_PAGE_WIDTH = 0.8F;
        private const float VIEW_ROBOTS_PAGE_HEIGHT = 0.8F;
        private const string VIEW_ROBOTS_PAGE_TITLE = "";
        
        private Vector2 scroll = Vector2.zero;

        private void OnGUI() {
            if (!Summary.isGameStarted) return; // 游戏未开始
            var dim = new Rect(
                Screen.width * (1 - VIEW_ROBOTS_PAGE_WIDTH) / 2, Screen.height * (1 - VIEW_ROBOTS_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_ROBOTS_PAGE_WIDTH, Screen.height * VIEW_ROBOTS_PAGE_HEIGHT
            );
            GUILayout.Window(VIEW_ROBOTS_PAGE_ID, dim, _ => {
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_ROBOTS_PAGE_HEIGHT));

                foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                    // 先展示未失联的机器人
                    GUILayout.BeginHorizontal("Box"); // 单独的机器人条目

                    GUILayout.BeginVertical(); // 左侧名称和血条
                    GUILayout.Label(robot.name);
                    GUILayout.Label(new string('❤', (int) Mathf.Ceil(robot.health)));
                    GUILayout.EndVertical();

                    if (GUILayout.Button("查看画面")) {
                        // TODO
                    }

                    GUILayout.EndHorizontal();
                }

                foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_MISSING)) {
                    // 再展示已失联的机器人
                    GUILayout.BeginHorizontal("Box"); // 单独的机器人条目

                    GUILayout.BeginVertical(); // 左侧名称
                    GUILayout.Label(robot.name);
                    GUILayout.Label("");
                    GUILayout.EndVertical();

                    GUILayout.Label("已失联");

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndScrollView();
            }, VIEW_ROBOTS_PAGE_TITLE);
        }
    }
}