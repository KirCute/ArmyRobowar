using Photon.Pun;

namespace System.Pickable {
    /// <summary>
    /// 用于处理拾取掉落物时掉落物的销毁，以及掉落物的自然销毁
    /// </summary>
    public class MEPickableDestroyer : MonoBehaviourPun {
        private MEPickableIdentifier identity;
        public double deadTime;  // 自然销毁发生事件，为0.0时永远不自然销毁，实际情况只有掉落的配件会自然销毁

        private void Awake() {
            identity = GetComponentInParent<MEPickableIdentifier>();
        }

        private void Update() {
            if (photonView.IsMine && deadTime > 0.0 && PhotonNetwork.Time >= deadTime) {
                PhotonNetwork.Destroy(photonView);
            }
        }

        private void OnEnable() {
            Events.AddListener(Events.F_PICKABLE_PICKED, OnDestroying);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_PICKABLE_PICKED, OnDestroying);
        }

        private void OnDestroying(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                PhotonNetwork.Destroy(photonView);
            }
        }
    }
}