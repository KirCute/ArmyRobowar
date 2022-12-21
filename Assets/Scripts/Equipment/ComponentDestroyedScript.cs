using System;
using UnityEngine;

namespace Equipment
{
    public class ComponentDestroyedScript : MonoBehaviour
    {
        private void OnEnable()
        {
            Events.AddListener(Events.F_COMPONENT_HEALTH_CHANGED, ComponentDestroyed);
        }

        private void OnDisable()
        {
            Events.RemoveListener(Events.F_COMPONENT_HEALTH_CHANGED, ComponentDestroyed);
        }

        public void ComponentDestroyed(object[] args)
        {
            if (args[0] == gameObject.name && (byte)args[1] == 0)
            {
                Events.Invoke(Events.F_COMPONENT_DESTROYED, new object[] { gameObject.name });
            }
        }
    }
}