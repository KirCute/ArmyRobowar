using System;
using UnityEngine;

namespace Equipment
{
    public class BodyDestroyedScript : MonoBehaviour
    {
        private void OnEnable()
        {
            Events.AddListener(Events.F_BODY_HEALTH_CHANGED, BodyDestroyed);
        }

        private void OnDisable()
        {
            Events.RemoveListener(Events.F_BODY_HEALTH_CHANGED, BodyDestroyed);
        }

        public void BodyDestroyed(object[] args)
        {
            if (args[0] == gameObject.name && (byte)args[1] == 0)
            {
                Events.Invoke(Events.F_BODY_DESTROYED, new object[] { gameObject.name });
            }
        }
    }
}