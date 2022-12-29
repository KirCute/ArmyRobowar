using Photon.Pun;

namespace System.Pickable {
    public class MEPickableDestroyer : MonoBehaviourPun {
        private MEPickableIdentifier identity;
        public double deadTime;

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