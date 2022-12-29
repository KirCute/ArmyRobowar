using System.Collections.Generic;
using Map.Navigation;
using UnityEngine;

namespace Test {
    public class TestAStar : MonoBehaviour
    {
   
        
            
        // Start is called before the first frame update
        void Start()
        {
        
            
        
        }

        // Update is called once per frame
        void Update()
        {
            List<Vector3> list = new List<Vector3>();
            //list = MDNavigationCenter.GetInstance().GetMatch(transform.localPosition, new Vector3(30.5f, 0, -30.97f));
            
            foreach (var VARIABLE in list) {
                print(VARIABLE);
                if (Input.GetMouseButtonDown(0)) {
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localPosition = VARIABLE;
                }
            }
        }
    }
}
