using Photon.Pun;

namespace Equipment.Tower {
    public class METowerIdentifier : AbstractMESignalIdentifier, IPunObservable {
        public int id { get; private set; }
        public int team { get; private set; }

        private void Awake() {
            id = (int) photonView.InstantiationData[0];
            team = (int) photonView.InstantiationData[1];
            gameObject.name = $"Tower_{id}";
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(id);
                stream.SendNext(team);
            } else {
                id = (int) stream.ReceiveNext();
                team = (int) stream.ReceiveNext();
            }
        }

        public override int GetTeamId() {
            return team;
        }
    }
}