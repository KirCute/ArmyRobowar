using System;
using System.Linq;
using Model.Equipment;
using Model.Technology;
using UnityEngine;
using Photon.Pun;

namespace UI
{
    public class MTViewShop : MonoBehaviourPun
    {
        private const int VIEW_SHOP_ID = 0;
        private const float VIEW_SHOP_WIDTH = 0.8F;
        private const float VIEW_SHOP_HEIGHT = 0.8F;
        private const string VIEW_SHOP_TITLE = "";
        private Vector2 scroll = Vector2.zero;

        private bool HasBase(string s) {
            bool b = false;
            if (int.Parse(s) != 0||int.Parse(s) != 1||int.Parse(s) != 2||int.Parse(s) != 3||int.Parse(s) != 4||int.Parse(s) != 5) {
                return false;
            }
            foreach (var basement in Summary.team.bases) {
                if (basement.Value.id == int.Parse(s)) {
                    b = true;
                    break;
                }
            }

            return b;
        }
        private void OnGUI() {
            var dim = new Rect(
                Screen.width * (1 - VIEW_SHOP_WIDTH) / 2, Screen.height * (1 - VIEW_SHOP_HEIGHT) / 2,
                Screen.width * VIEW_SHOP_WIDTH, Screen.height * VIEW_SHOP_HEIGHT
            );
            GUILayout.Window(VIEW_SHOP_ID, dim, _ => {
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_SHOP_HEIGHT));
                GUILayout.BeginVertical("Box"); 
                foreach (var goods in Summary.team.availableRobotTemplates) {
                    GUILayout.BeginHorizontal("Box");
                    string label = "";
                    GUILayout.Label(goods+label,GUILayout.ExpandWidth(true));
                    GUILayout.Label("取名:");
                    string robotName = "";
                    GUILayout.TextField(robotName);
                    GUILayout.Label("选择基地:");
                    string baseName = "";
                    GUILayout.TextField(baseName);
                    if (GUILayout.Button("购买",GUILayout.ExpandWidth(false)))
                    {
                        if(robotName != ""&& baseName != "" && HasBase(baseName)) {
                            label = "";
                            Events.Invoke(Events.M_CREATE_ROBOT,
                                new object[]
                                {
                                    int.Parse(baseName), goods, robotName
                                });
                        }
                        else {
                            label = "请输入正确的名字和基地号";
                        }                   
                    }
                    GUILayout.EndHorizontal();
                }
                foreach (var goods in Summary.team.availableSensorTemplates) {
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(goods,GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("购买",GUILayout.ExpandWidth(false)))
                    {
                        Events.Invoke(Events.M_TEAM_BUY_COMPONENT,
                            new object[] { Summary.team.teamColor, goods });
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            }, VIEW_SHOP_TITLE);
            if (Input.GetKeyDown(KeyCode.Escape)) {
                enabled = false;
                GetComponent<MEMainCameraController>().active = true;
            }
        }
    }
}