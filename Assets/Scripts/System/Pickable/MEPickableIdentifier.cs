using Photon.Pun;

namespace System.Pickable {
    /// <summary>
    /// 用于储存掉落物的ID，从而可以从掉落物的GameObject反推其ID
    /// </summary>
    public class MEPickableIdentifier : MonoBehaviourPun, IPunObservable {
        public int id { get; private set; }
		
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