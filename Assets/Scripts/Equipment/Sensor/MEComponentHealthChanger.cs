using System;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor {
    public class MEComponentHealthChanger : MonoBehaviourPun, IPunObservable {
        private MEComponentIdentifier identity;

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_COMPONENT_DAMAGE, OnHealthChanging);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_COMPONENT_DAMAGE, OnHealthChanging);
        }

        private void OnHealthChanging(object[] args) {
            if (identity.robotId == (int) args[0] && identity.index == (int) args[1] &&
                identity.team == Summary.team.teamColor && photonView.IsMine) {
                var health = Summary.team.robots[identity.robotId].equippedComponents[identity.index].health;
                health = Mathf.Max(health + (int) args[1], 0);
                Summary.team.robots[identity.robotId].equippedComponents[identity.index].health = health;
                Events.Invoke(Events.F_COMPONENT_HEALTH_CHANGED, new object[] {identity.robotId, identity.index, health});
                if (health <= 0) {
                    Events.Invoke(Events.F_COMPONENT_DESTROYED, new object[] {identity.robotId, identity.index});
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (identity.team == Summary.team.teamColor) {
                if (stream.IsWriting) {
                    stream.SendNext(Summary.team.robots[identity.robotId].equippedComponents[identity.index].health);
                } else {
                    Summary.team.robots[identity.robotId].equippedComponents[identity.index].health = (int) stream.ReceiveNext();
                }
            }
        }
    }
}