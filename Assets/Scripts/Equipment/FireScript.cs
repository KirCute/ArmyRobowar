using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Equipment;

namespace Equipment
{

    public class FireScript : MonoBehaviour
    {
        private int layerMask;
        // Start is called before the first frame update
        void Start()
        {
            layerMask = 1 << 6;     //body
            layerMask += 1 << 7 ;   //增加允许检测第7层Layer（component）
        }

        private void OnEnable()
        {
            Events.AddListener(Events.M_ROBOT_FIRE, OnFire);
        }

        // Update is called once per frame
        void Update()
        {
            if (Physics.Raycast(transform.position, transform.forward, out var hit, 20f, layerMask))
            {
                if (hit.collider.gameObject.layer == 6)
                {
                    /*参数应该是ID与血量*/
                    Events.Invoke(Events.F_BODY_HEALTH_CHANGED, new object[] { hit.collider.gameObject.name });
                }
                if (hit.collider.gameObject.layer == 7)
                {
                    /*参数应该是ID与血量*/
                    Events.Invoke(Events.F_COMPONENT_HEALTH_CHANGED, new object[] { hit.collider.gameObject.name });
                    /*参数应该是ID与血量*/
                    Events.Invoke(Events.F_COMPONENT_DAMAGE, new object[] { hit.collider.gameObject.name });
                }
            }
            
        }

        private void OnDisable()
        {
            Events.RemoveListener(Events.M_ROBOT_FIRE, OnFire);
        }

        public void OnFire(object[] args)
        {
            if (args[0] == gameObject.name)
            {
                Events.Invoke(Events.F_ROBOT_FIRED, new object[] { gameObject.name });
            }
            
        }
    }
}


