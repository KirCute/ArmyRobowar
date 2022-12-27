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

                foreach (var goods in Summary.team.availableRobotTemplates) {
                    // 先展示未失联的机器人
                    GUILayout.BeginHorizontal("Box"); // 单独的机器人条目
                    GUILayout.BeginVertical(); // 左侧名称和血条
                    GUILayout.Label(goods,GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("购买",GUILayout.ExpandWidth(false)))
                    {
                        Events.Invoke(Events.M_TEAM_BUY_COMPONENT,
                            new object[] { Summary.team.teamColor, (string)photonView.InstantiationData[0] });
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }

                foreach (var goods in Summary.team.availableSensorTemplates) {
                    // 再展示已失联的机器人
                    GUILayout.BeginHorizontal("Box"); // 单独的机器人条目
                    GUILayout.BeginVertical(); // 左侧名称
                    GUILayout.Label(goods,GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("购买",GUILayout.ExpandWidth(false)))
                    {
                        Events.Invoke(Events.M_TEAM_BUY_COMPONENT,
                            new object[] { Summary.team.teamColor, (string)photonView.InstantiationData[0] });
                    }
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndScrollView();
            }, VIEW_SHOP_TITLE);
        }
    }
}