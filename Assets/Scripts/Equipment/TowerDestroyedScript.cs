﻿using System;
using UnityEngine;

namespace Equipment
{
    public class TowerDestroyedScript : MonoBehaviour
    {
        private void OnEnable()
        {
            Events.AddListener(Events.F_TOWER_HEALTH_CHANGED, ComponentDestroyed);
        }

        private void OnDisable()
        {
            Events.RemoveListener(Events.F_TOWER_HEALTH_CHANGED, ComponentDestroyed);
        }

        public void ComponentDestroyed(object[] args)
        {
            if (args[0] == gameObject.name && (byte)args[1] == 0)
            {
                Events.Invoke(Events.F_TOWER_DESTROYED, new object[] { gameObject.name });
            }
        }
    }
}