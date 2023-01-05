using System;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Diamond {
    /// <summary>
    /// 用于处理基地水晶的扣血，并同步血量数据（含发布基地摧毁事件）
    /// </summary>
    public class MEBaseHealthChanger : MonoBehaviourPun, IPunObservable {
        private MEBaseFlag identity;

        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_BASE_DAMAGE, OnHealthChanging);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_BASE_DAMAGE, OnHealthChanging);
        }

        private void OnHealthChanging(object[] args) {
            if (identity.baseId == (int) args[0] && identity.flagColor == Summary.team.teamColor &&
                Summary.isTeamLeader) {
                var health = Summary.team.bases[identity.baseId].health;
                health = Mathf.Max(health + (int) args[1], 0);
                health = Mathf.Min(health, Model.Equipment.Basement.BASE_MAX_HEALTH);
                Summary.team.bases[identity.baseId].health = health;
                Events.Invoke(Events.F_BASE_HEALTH_CHANGED, new object[] {identity.baseId, health});
                if (health <= 0) {
                    Events.Invoke(Events.F_BASE_DESTROYED, new object[] {identity.baseId, identity.flagColor});
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (Summary.isGameStarted && identity.flagColor == Summary.team.teamColor) {
                if (stream.IsWriting) {
                    stream.SendNext(Summary.team.bases[identity.baseId].health);
                } else {
                    Summary.team.bases[identity.baseId].health = (int) stream.ReceiveNext();
                }
            }
        }
    }
}