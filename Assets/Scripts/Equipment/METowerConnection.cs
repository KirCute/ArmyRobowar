using System;
using UnityEngine;

namespace Equipment
{
    public class METowerConnection : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            if (gameObject.gameObject.CompareTag("outside"))
            {
                //由于还没有ID和connection的文件会报错所以先注释掉
                //other.gameObject.connection += 10;
                //Events.Invoke(Events.F_ROBOT_CONNECTION, new object[] { other.gameObject.id,other.gameObject.connection });
            }
            if (gameObject.gameObject.CompareTag("inside"))
            {
                //other.gameObject.connection += 10000;
                //Events.Invoke(Events.F_ROBOT_CONNECTION, new object[] { other.gameObject.id,other.gameObject.connection });
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (gameObject.gameObject.CompareTag("outside"))
            {
                //other.gameObject.connection -= 10;
                //Events.Invoke(Events.F_ROBOT_CONNECTION, new object[] { other.gameObject.id,other.gameObject.connection }
            }
            
            if (gameObject.gameObject.CompareTag("inside"))
            {
                //other.gameObject.connection -= 10000;
                //Events.Invoke(Events.F_ROBOT_CONNECTION, new object[] { other.gameObject.id,other.gameObject.connection });
            }
        }
    }
}