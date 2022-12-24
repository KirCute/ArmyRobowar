using Photon.Pun;

namespace Equipment.Sensor {
    public class MEComponentIdentifier : MonoBehaviourPun, IPunObservable {
        public int robotId { get; private set; }
        public int index { get; private set; }
        public int team { get; private set; }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(robotId);
                stream.SendNext(index);
                stream.SendNext(team);
            } else {
                robotId = (int) stream.ReceiveNext();
                index = (int) stream.ReceiveNext();
                team = (int) stream.ReceiveNext();
            }
        }
        
    }
}