using System;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Tower {
    public class METowerHealthChanger : MonoBehaviourPun, IPunObservable {
        private METowerIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<METowerIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_TOWER_DAMAGE, OnHealthChanging);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_TOWER_DAMAGE, OnHealthChanging);
        }

        private void OnHealthChanging(object[] args) {
            if (identity.id == (int) args[0] && identity.team == Summary.team.teamColor && photonView.IsMine) {
                var health = Summary.team.towers[identity.id].health;
                health = Mathf.Max(health + (int) args[1], 0);
                health = Mathf.Min(health, Summary.team.towers[identity.id].template.maxHealth);
                Summary.team.towers[identity.id].health = health;
                Events.Invoke(Events.F_TOWER_HEALTH_CHANGED, new object[] {identity.id, health});
                if (health <= 0) {
                    Events.Invoke(Events.F_TOWER_DESTROYED, new object[] {identity.id});
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (identity.team == Summary.team.teamColor) {
                if (stream.IsWriting) {
                    stream.SendNext(Summary.team.towers[identity.id].health);
                } else {
                    Summary.team.towers[identity.id].health = (int) stream.ReceiveNext();
                }
            }
        }
        
    }
}