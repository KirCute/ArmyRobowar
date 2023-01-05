using System;
using Photon.Pun;

namespace Equipment.Tower {
    /// <summary>
    /// 用于处理信号塔的销毁
    /// </summary>
    public class METowerDestroyer : MonoBehaviourPun {
        private METowerIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<METowerIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.F_TOWER_DESTROYED, OnDestroying);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_TOWER_DESTROYED, OnDestroying);
        }

        private void OnDestroying(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                PhotonNetwork.Destroy(photonView);
            }
        }

        private void OnDestroy() {
            if (Summary.team.teamColor == identity.team) {
                Summary.team.towers.Remove(identity.id);
            }
        }
    }
}