using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Map.Navigation {
    public class MDNavigationCenter : MonoBehaviour {
        private static MDNavigationCenter INSTANCE;
        public MDNavigationPoint[] points;

        public static MDNavigationCenter getInstance() {
            return INSTANCE;
        }

        public void Awake() {
            INSTANCE = this;
            points = GetComponentsInChildren<MDNavigationPoint>();
        }

        public List<Vector3> GetPath(Vector3 startPos, Vector3 endPos) {
            
            
            return null;
        }
    }
}
