using System;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor {
    public class MEComponentHealthChanger : MonoBehaviourPun, IPunObservable {
        private MEComponentIdentifier identity;
        private bool broken;

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        private void Update() {
            if (identity.team == Summary.team.teamColor &&
                Summary.team.robots[identity.robotId].equippedComponents[identity.index].health <= 0) {
                Summary.team.robots[identity.robotId].equippedComponents[identity.index]
                    .OnUnloaded(identity.robotId, identity.index, photonView.IsMine);
                Summary.team.robots[identity.robotId].equippedComponents[identity.index] = null;
                broken = true;
            }
        }

        private void OnEnable() {
            Events.AddListener(Events.M_COMPONENT_DAMAGE, OnHealthChanging);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_COMPONENT_DAMAGE, OnHealthChanging);
        }

        private void OnHealthChanging(object[] args) {
            if (!broken && identity.robotId == (int) args[0] && identity.index == (int) args[1] &&
                identity.team == Summary.team.teamColor && photonView.IsMine) {
                var component = Summary.team.robots[identity.robotId].equippedComponents[identity.index];
                var health = component.health;
                health = Mathf.Max(health + (int) args[2], 0);
                health = Mathf.Min(health, component.template.maxHealth);
                Summary.team.robots[identity.robotId].equippedComponents[identity.index].health = health;
                Events.Invoke(Events.F_COMPONENT_HEALTH_CHANGED,
                    new object[] {identity.robotId, identity.index, health});
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (broken) return;
            if (identity.team == Summary.team.teamColor) {
                if (stream.IsWriting) {
                    stream.SendNext(Summary.team.robots[identity.robotId].equippedComponents[identity.index].health);
                } else {
                    Summary.team.robots[identity.robotId].equippedComponents[identity.index].health =
                        (int) stream.ReceiveNext();
                }
            }
        }
    }
}