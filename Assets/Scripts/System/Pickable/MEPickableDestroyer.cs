using Photon.Pun;

namespace System.Pickable {
    public class MEPickableDestroyer : MonoBehaviourPun {
        private MEPickableIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MEPickableIdentifier>();
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