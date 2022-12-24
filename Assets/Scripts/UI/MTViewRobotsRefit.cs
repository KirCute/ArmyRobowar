using System;
using System.Collections;
using System.Collections.Generic;
using Model.Equipment;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class MTViewRobotsRefit : MonoBehaviour
    {
        private List<BaseSensor> Temp_Component_Self_Use = new List<BaseSensor>(); 

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
                
                GUILayout.BeginVertical();
                //TODO
                GUILayout.EndVertical();
                
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_REFIT_PAGE_HEIGHT));
                foreach (var component in Temp_Component_Self_Use) {
                    GUILayout.BeginHorizontal("Box");
                    
                    GUILayout.BeginVertical();
                    GUILayout.Label(component.template.name);
                    GUILayout.EndVertical();
                }
                
                GUILayout.EndHorizontal();
            },VIEW_REFIT_PAGE_TITLE);
        }
    }
}