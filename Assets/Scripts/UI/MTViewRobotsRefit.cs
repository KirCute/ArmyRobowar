using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.Equipment;
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
        
        private void Start() {
            
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
                GUILayout.Button("ds");
                GUILayout.BeginVertical();
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT));
                foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                    GUILayout.BeginHorizontal("Box");
                    
                    GUILayout.BeginVertical();
                    GUILayout.Label(robot.name,GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("查看机器人", GUILayout.ExpandWidth(false)))
                    {
                        //TODO
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();
                
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT));
                foreach (var component in Temp_Component_Self_Use) {
                    GUILayout.BeginHorizontal("Box");
                    
                    GUILayout.BeginVertical();
                    GUILayout.Label(component.template.name,GUILayout.ExpandWidth(true));//配件名字（或者是贴图）未解决
                    if (GUILayout.Button("装配", GUILayout.ExpandWidth(false))) {
                        //TODO
                        // 参数：队伍号(int), 机器人id(int), 安装位置(int), 要安装的传感器在仓库中的索引(int)
                        Events.Invoke(Events.M_ROBOT_INSTALL_COMPONENT, new object[] { Summary.team.teamColor,
                            Summary.team.robots[0].id});
                    }
                    GUILayout.EndVertical();
                }
                
                GUILayout.EndHorizontal();
            },VIEW_REFIT_PAGE_TITLE);
        }
    }
}