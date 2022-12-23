using System;
using UnityEngine;

namespace Equipment
{
    public class MTTowerConnection : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            if (gameObject.gameObject.CompareTag("outside"))
            {
                other.gameObject.connection += 10;
                Events.Invoke(Events.F_ROBOT_CONNECTION, new object[] { other.gameObject.name,other.gameObject.connection });
            }
            
            
        }

        private void OnTriggerStay(Collider other)
        {
            Events.Invoke(Events.F_ROBOT_ACQUIRED_CONNECTION, new object[] { other.gameObject.name });
        }

        private void OnTriggerExit(Collider other)
        {
            if (gameObject.gameObject.CompareTag("outside"))
            {
                Events.Invoke(Events.F_ROBOT_LOST_CONNECTION, new object[] { other.gameObject.name });
            }
            
            if (gameObject.gameObject.CompareTag("inside"))
            {
                Events.Invoke(Events.F_ROBOT_WEAK_CONNECTION, new object[] { other.gameObject.name });
            }
        }
    }
}