using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Model.Equipment;
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

        private string[] technologies_strings = new[] { "Robot","Camera","Gun","Lidar","Transporter","Shield","Tower","Hider" };

        private Dictionary<int, string> technologies = new Dictionary<int, string>
        {
            { 1, "Robot" }, { 0, "Camera" }, { 0, "Gun" }, { 0, "Lidar" }, { 0, "Transporter" }, { 0, "Shield" },
            { 0, "Tower" }, { 0, "Hider" }
        };
        private void OnGUI() {
            var dim = new Rect(
                Screen.width * (1 - VIEW_TECH_PAGE_WIDTH) / 2, Screen.height * (1 - VIEW_TECH_PAGE_WIDTH) / 2,
                Screen.width * VIEW_TECH_PAGE_WIDTH, Screen.height * VIEW_TECH_PAGE_WIDTH
            );
            GUILayout.Window(VIEW_TECH_PAGE_ID, dim, _ => {
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_TECH_PAGE_HEIGHT));

                foreach (var tech in technologies) {
                    
                    GUILayout.BeginHorizontal("Box"); 
                    
                    GUILayout.BeginVertical("Box");
                    
                    GUILayout.Label(tech.Value);
                    
                    GUILayout.Label("当前等级是:"+tech.Key);
                    if (tech.Value.Equals("Camera")&&tech.Key>=1) {
                        if (GUILayout.Button("查看详情",GUILayout.ExpandWidth(false))) {
                            //TODO
                        }
                    }else if (tech.Value.Equals("Gun")&&tech.Key>=1) {
                        if (GUILayout.Button("查看详情",GUILayout.ExpandWidth(false))) {
                            //TODO
                        }
                    }else if (tech.Value.Equals("Lidar")&&tech.Key>=1) {
                        if (GUILayout.Button("查看详情",GUILayout.ExpandWidth(false))) {
                            //TODO
                        }
                    }else if (tech.Value.Equals("Tower")&&tech.Key>=1) {
                        if (GUILayout.Button("查看详情",GUILayout.ExpandWidth(false))) {
                            //TODO
                        }
                    }else {
                        if (GUILayout.Button("升级",GUILayout.ExpandWidth(false))){
                            //TODO
                        }
                    }

                    GUILayout.EndVertical();
                    
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }, VIEW_TECH_PAGE_TITLE);
        
        }
    }
}