using Photon.Pun;

namespace System.Pickable {
    public class MEPickableIdentifier : MonoBehaviourPun, IPunObservable {
        public int id { get; set; }
		
		public void Awake() {
            id = (int) photonView.InstantiationData[0];
            gameObject.name = $"Pickable_{id}";
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(id);
            } else {
                id = (int) stream.ReceiveNext();
            }
        }
    }
}