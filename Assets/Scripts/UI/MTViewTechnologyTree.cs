using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;

namespace UI {
    public class MTViewTechnologyTree : MonoBehaviour {
        private const int VIEW_TECH_PAGE_ID = 0;
        private const float VIEW_TECH_PAGE_WIDTH = 0.8F;
        private const float VIEW_TECH_PAGE_HEIGHT = 0.7F;
        private const string VIEW_TECH_PAGE_TITLE = "";
        private Vector2 scroll = Vector2.zero;


        private void OnGUI() {
            var dim = new Rect(
                Screen.width * (1 - VIEW_TECH_PAGE_WIDTH) / 2, Screen.height * (1 - VIEW_TECH_PAGE_WIDTH) / 2,
                Screen.width * VIEW_TECH_PAGE_WIDTH, Screen.height * VIEW_TECH_PAGE_WIDTH
            );

            GUILayout.Window(VIEW_TECH_PAGE_ID, dim, _ => {
                GUILayout.Label($"当前科技点：{Summary.team.researchPoint:0.00}");
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_TECH_PAGE_HEIGHT));
                GUILayout.BeginVertical("Box");

                foreach (var tech in Constants.TECHNOLOGY.Keys.Where(tech =>
                             !Summary.team.achievedTechnics.Contains(tech))) {
                    GUILayout.BeginHorizontal("Box");

                    GUILayout.Label($"{Constants.TECHNOLOGY[tech].name} ({Constants.TECHNOLOGY[tech].cost:0.00})");
                    if (Constants.TECHNIC_TOPOLOGY[tech].Any(t => !Summary.team.achievedTechnics.Contains(t))) {
                        GUILayout.Label("需先学习前置科技", GUILayout.ExpandWidth(false));
                    } else if (Summary.team.researchPoint < Constants.TECHNOLOGY[tech].cost) {
                        GUILayout.Label("科技点不足", GUILayout.ExpandWidth(false));
                    } else if (GUILayout.Button("学习", GUILayout.ExpandWidth(false))) {
                        Events.Invoke(Events.M_TECHNOLOGY_RESEARCH,
                            new object[] {Summary.team.teamColor, tech}
                        );
                    }

                    GUILayout.EndHorizontal();
                }

                foreach (var tech in Summary.team.achievedTechnics) {
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(Constants.TECHNOLOGY[tech].name);
                    GUILayout.Label("已学习", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            }, VIEW_TECH_PAGE_TITLE);
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) {
                enabled = false;
                GetComponent<MEMainCameraController>().active = true;
            }
        }
    }
}