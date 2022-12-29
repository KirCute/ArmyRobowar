using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Model.Equipment;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace UI
{
    public class MTViewTechnologyTree : MonoBehaviour
    {
        private const int VIEW_TECH_PAGE_ID = 0;
        private const float VIEW_TECH_PAGE_WIDTH = 0.8F;
        private const float VIEW_TECH_PAGE_HEIGHT = 0.8F;
        private const string VIEW_TECH_PAGE_TITLE = "";
        private Vector2 scroll = Vector2.zero;

        private Dictionary<string, List<string>> technics = new Dictionary<string, List<string>>
        {
            {"iRobot",new List<string>{""}},{"iiRobot",new List<string>{"iRobot"}},{"iiiRobot",new List<string>{"iiRobot"}},{"BaseCamera",new List<string>{""}},{"BaseGun",new List<string>{""}}
        };
        private void OnGUI() {
            var dim = new Rect(
                Screen.width * (1 - VIEW_TECH_PAGE_WIDTH) / 2, Screen.height * (1 - VIEW_TECH_PAGE_WIDTH) / 2,
                Screen.width * VIEW_TECH_PAGE_WIDTH, Screen.height * VIEW_TECH_PAGE_WIDTH
            );
            GUILayout.Window(VIEW_TECH_PAGE_ID, dim, _ => {
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_TECH_PAGE_HEIGHT));

                foreach (var tech in System.Constants.TECHNOLOGY ) {
                    
                    GUILayout.BeginHorizontal("Box"); 
                    
                    GUILayout.BeginHorizontal("Box");
                    
                    GUILayout.Label(tech.Key);

                    foreach (var technic in Summary.team.achievedTechnics) { 
                        if (technics[tech.Key][0].Equals("")) {
                            if (technics[tech.Key][0].Equals(technic)) {
                                break;   
                            }
                            if (GUILayout.Button("升级",GUILayout.ExpandWidth(false))) {
                                //TODO
                            }
                            break;
                        }else {
                            if (technics[tech.Key][0].Equals(technic)) {
                                if (GUILayout.Button("升级",GUILayout.ExpandWidth(false))) {
                                    //TODO
                                }
                                break;  
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