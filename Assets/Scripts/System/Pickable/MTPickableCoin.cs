using Photon.Pun;
using UnityEngine;

namespace System.Pickable {
    public class MTPickableCoin : AbstractMTPickable, IPunObservable {
        [SerializeField] private int value;
        private bool picked;
        private MEPickableIdentifier identity;
        public override string pickableName => $"金币 * {value}";

        private void Awake() {
            identity = GetComponentInParent<MEPickableIdentifier>();
        }

        public override void Pickup(int team, int robotId) {
            if (picked) return;
            picked = true;
            Events.Invoke(Events.F_ROBOT_ACQUIRE_COINS, new object[] {team, robotId, value});
            Events.Invoke(Events.F_PICKABLE_PICKED, new object[] {identity.id});
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(picked);
            } else {
                picked = (bool) stream.ReceiveNext();
            }
        }
    }
}