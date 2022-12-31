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
                    GUILayout.Label(goods,GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("购买",GUILayout.ExpandWidth(false)))
                    {
                        // TODO 选择机器人出生在哪个基地
                        Events.Invoke(Events.M_CREATE_ROBOT,
                            new object[] { Summary.team.bases.Values.First().id, goods, $"TestCar_{Summary.team.robots.Count}" });
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