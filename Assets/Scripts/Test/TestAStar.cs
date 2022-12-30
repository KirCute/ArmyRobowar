using System.Collections.Generic;
using UnityEngine;

namespace Test {
    public class TestAStar : MonoBehaviour {
        // Update is called once per frame
        private void Update() {
            var list = new List<Vector3>();
            //list = MDNavigationCenter.GetInstance().GetMatch(transform.localPosition, new Vector3(30.5f, 0, -30.97f));

            foreach (var point in list) {
                print(point);
                if (Input.GetMouseButtonDown(0)) {
                    var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localPosition = point;
                }
            }
        }
    }
}