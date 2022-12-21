using System;
using Photon.Pun;
using UnityEngine;


namespace Equiment {
    /// <summary>
    /// 发布锁敌以及失去锁敌的事件
    /// </summary>
    public class MTRobotSeizeAndLostEnemy : MonoBehaviourPun {
        public void OnTriggerEnter(Collider other) {
            //返回该碰撞体对应车辆的id
            Events.Invoke(Events.F_ROBOT_SEIZE_ENEMY, new object[] {other.gameObject.GetComponent<MTIdOfBody>().id});
            }

        public void OnTriggerExit(Collider other) {
            Events.Invoke(Events.F_ROBOT_LOST_SEIZE_ENEMY, new object[] {other.gameObject.GetComponent<MTIdOfBody>().id});
        }
    }
}