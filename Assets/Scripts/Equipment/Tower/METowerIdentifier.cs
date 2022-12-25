using Photon.Pun;

namespace Equipment.Tower {
    public class METowerIdentifier : MonoBehaviourPun, IPunObservable {
        public int id { get; private set; }
        public int team { get; private set; }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(id);
                stream.SendNext(team);
            } else {
                id = (int) stream.ReceiveNext();
                team = (int) stream.ReceiveNext();
            }
        }
    }
}