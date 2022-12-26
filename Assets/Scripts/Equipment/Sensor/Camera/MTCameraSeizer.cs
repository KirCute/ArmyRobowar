using System;
using Equipment.Robot;
using Equipment.Robot.Body;
using Equipment.Sensor;
using Photon.Pun;
using UnityEngine;


namespace Equiment {
    /// <summary>
    /// 发布锁敌以及失去锁敌的事件
    /// </summary>
    public class MTCameraSeizer : MonoBehaviourPun {
        private MEComponentIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MEComponentIdentifier>();
        }

        public void OnTriggerEnter(Collider other) {
            //返回该碰撞体对应车辆的id
            if (photonView.IsMine && other.GetComponent<MTRobotHurt>() != null) {
                var id = other.GetComponentInParent<MERobotIdentifier>();
                if (id.team != identity.team) {
                    Events.Invoke(Events.F_ROBOT_SEIZE_ENEMY, new object[] {identity.team, id.id});
                }
            }
        }

        public void OnTriggerExit(Collider other) {
            if (photonView.IsMine && other.GetComponent<MTRobotHurt>() != null) {
                var id = other.GetComponentInParent<MERobotIdentifier>();
                if (id.team != identity.team) {
                    Events.Invoke(Events.F_ROBOT_LOST_SEIZE_ENEMY, new object[] {identity.team, id.id});
                }
            }
        }
    }
}