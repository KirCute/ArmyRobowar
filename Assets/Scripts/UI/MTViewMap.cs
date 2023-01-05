using System;
using UnityEngine;

namespace UI {
    public class MTViewMap {
        private const int VIEW_MAP_PAGE_ID = 0;
        private const float VIEW_MAP_PAGE_WIDTH = 0.2F;
        private const float VIEW_MAP_PAGE_HEIGHT = 0.3F;
        private const string VIEW_MAP_PAGE_TITLE = "";

        private void OnGUI() {
            if (!Summary.isGameStarted) return; 
            var dim = new Rect(
                0,0, Screen.width * VIEW_MAP_PAGE_WIDTH, Screen.height * VIEW_MAP_PAGE_HEIGHT
            );
            Texture2D texture2D = Resources.Load<Texture2D>("地图俯瞰正交图");
            GUILayout.Window(VIEW_MAP_PAGE_ID, dim,DisplayMap, texture2D);
            
        }

        private void DisplayMap(int id) {
            
        }
    }
}