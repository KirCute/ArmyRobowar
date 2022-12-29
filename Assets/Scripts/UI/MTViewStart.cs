using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MTViewStart : MonoBehaviour
    {
        private const int VIEW_START_PAGE_ID = 0;

        private const string VIEW_START_PAGE_TITLE = "";
        
        private GUIStyle style = new GUIStyle(); //定义控件

        private List<String> TEMP_TEST_RED = new List<string>();
        private List<String> TEMP_TEST_BLUE = new List<string>();
        private void Start() {
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 24;
            TEMP_TEST_RED.Add("red1");
            TEMP_TEST_RED.Add("red2");
            TEMP_TEST_RED.Add("red3");
            TEMP_TEST_RED.Add("red4");
            TEMP_TEST_RED.Add("red5");
        }
        
        private void OnGUI() {
            if (Summary.isGameStarted) return;
            GUILayout.Window(VIEW_START_PAGE_ID, new Rect(0,
                    0 ,
                    Screen.width , Screen.height), _ =>
                {
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                    if (true) //按下准备后切换按钮，if里面放的是个人是否准备
                    {
                        if (GUILayout.Button("准备游戏", style))
                        {
                            Events.Invoke(Events.M_PLAYER_READY, new object[] { });
                        }
                    }
                    else {
                        if (GUILayout.Button("取消准备", style)) {
                            Events.Invoke(Events.M_CANCEL_READY, new object[] { });
                        }
                    }

                    if (GUILayout.Button("交换队伍",style)) {
                        Events.Invoke(Events.M_CHANGE_TEAM, new object[] { });
                    }

                    if (GUILayout.Button("离开匹配",style)) {
                        Events.Invoke(Events.M_LEAVE_MATCHING, new object[] { });
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    
                    GUILayout.BeginVertical("Box");
                    GUILayout.Box("red1",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("red2",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("red3",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("red4",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("red5",style,GUILayout.ExpandHeight(true));
                    GUILayout.EndVertical();
                    
                    GUILayout.BeginVertical("Box");
                    GUILayout.Box("blue1",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("blue2",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("blue3",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("blue4",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("blue5",style,GUILayout.ExpandHeight(true));
                    GUILayout.EndVertical();
                    
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                },
                VIEW_START_PAGE_TITLE);
        }
    }
}