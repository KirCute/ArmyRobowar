using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    public class BaseDestroyedScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void OnEnable()
        {
            Events.AddListener(Events.F_BASE_HEALTH_CHANGED, BaseDestroyed);
        }

        private void OnDisable()
        {
            Events.RemoveListener(Events.F_BASE_HEALTH_CHANGED, BaseDestroyed);
        }

        public void BaseDestroyed(object[] args)
        {
            if (args[0] == gameObject.name && (int)args[1] == 0)
            {
                Events.Invoke(Events.F_BASE_DESTROYED, new object[] { gameObject.name });
            }
        }
    }
}