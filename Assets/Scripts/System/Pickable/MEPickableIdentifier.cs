using Photon.Pun;

namespace System.Pickable {
    public class MEPickableIdentifier : MonoBehaviourPun, IPunObservable {
        public int id { get; set; }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(id);
            } else {
                id = (int) stream.ReceiveNext();
            }
        }
    }
}