using Photon.Pun;

namespace Equipment.Robot {
    /// <summary>
    /// 仅用于存储车体的id
    /// </summary>
    public class MERobotIdentifier : MonoBehaviourPun, IPunObservable {
        public int id { get; set; }
        public int team { get; set; }

        public void Awake() {
            id = (int) photonView.InstantiationData[0];
            team = (int) photonView.InstantiationData[1];
            gameObject.name = $"Robot_{id}";
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
    }
}