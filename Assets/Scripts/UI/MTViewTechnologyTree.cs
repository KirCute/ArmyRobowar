using System;
using UnityEngine;

namespace UI {
    public class MTViewTechnologyTree : MonoBehaviour {
        private const int VIEW_TECH_PAGE_ID = 0;
        private const float VIEW_TECH_PAGE_WIDTH = 0.8F;
        private const float VIEW_TECH_PAGE_HEIGHT = 0.8F;
        private const string VIEW_TECH_PAGE_TITLE = "";
        private Vector2 scroll = Vector2.zero;

        private void OnGUI() {
            var dim = new Rect(
                Screen.width * (1 - VIEW_TECH_PAGE_WIDTH) / 2, Screen.height * (1 - VIEW_TECH_PAGE_WIDTH) / 2,
                Screen.width * VIEW_TECH_PAGE_WIDTH, Screen.height * VIEW_TECH_PAGE_WIDTH
            );
            GUILayout.Window(VIEW_TECH_PAGE_ID, dim, _ => {
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_TECH_PAGE_HEIGHT));

                foreach (var tech in Constants.TECHNOLOGY) {
                    GUILayout.BeginHorizontal("Box");

                    GUILayout.BeginHorizontal("Box");

                    GUILayout.Label(tech.Key);
                    var cnt = 0;
                    foreach (var technic in Summary.team.achievedTechnics) {
                        if (Constants.TECHNIC_TOPOLOGY[tech.Key][0].Equals("")) {
                            if (Constants.TECHNIC_TOPOLOGY[tech.Key][0].Equals(technic)) {
                                break;
                            }

                            if (GUILayout.Button("升级", GUILayout.ExpandWidth(false))) {
                                Events.Invoke(Events.M_TECHNOLOGY_RESEARCH,
                                    new object[] {Summary.team.teamColor, tech.Key}
                                );
                            }

                            break;
                        } else {
                            if (Constants.TECHNIC_TOPOLOGY[tech.Key][0].Equals(technic)) {
                                GUILayout.Label("已升级", GUILayout.ExpandWidth(false));
                                break;
                            }
                        }

                        cnt++;
                        if (cnt == Summary.team.achievedTechnics.Count) {
                            cnt = 0;
                            if (GUILayout.Button("升级", GUILayout.ExpandWidth(false))) {
                                //TODO
                            }
                        }
                    }

                    GUILayout.BeginHorizontal();

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndScrollView();
            }, VIEW_TECH_PAGE_TITLE);
        }
    }
}