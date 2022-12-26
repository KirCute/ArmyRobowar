using Photon.Pun;

namespace System.Judgement {
    public class MTScoreboard : MonoBehaviourPun {
        private void OnEnable() {
            Events.AddListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
        }

        private void OnBaseDestroyed(object[] args) {
            // TODO
        }
    }
}