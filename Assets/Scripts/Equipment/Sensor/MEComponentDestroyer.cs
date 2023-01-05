using System;
using Photon.Pun;

namespace Equipment.Sensor {
    /// <summary>
    /// 用于处理配件的销毁
    /// </summary>
    public class MEComponentDestroyer : MonoBehaviourPun {
        private MEComponentIdentifier identity;

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.F_COMPONENT_DESTROYED, OnDestroying);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_COMPONENT_DESTROYED, OnDestroying);
        }

        private void OnDestroying(object[] args) {
            if (identity.robotId == (int) args[0] && identity.index == (int) args[1] &&
                identity.team == Summary.team.teamColor && photonView.IsMine) {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}