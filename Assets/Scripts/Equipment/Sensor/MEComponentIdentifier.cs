using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor {
    /// <summary>
    /// 于记录配件所属机器人的ID和配件安装位置，从而可以从配件的GameObject反推其所属机器人和其安装位置
    /// </summary>
    public class MEComponentIdentifier : MonoBehaviourPun, IPunObservable {
		[SerializeField] private string installPosition;
        public int robotId { get; private set; }
        public int index { get; private set; }
        public int team { get; private set; }

        private void Awake() {
            robotId = (int) photonView.InstantiationData[0];
            index = (int) photonView.InstantiationData[1];
            team = (int) photonView.InstantiationData[2];
            transform.parent = GameObject.Find($"Robot_{robotId}").transform.Find("Body")
                .Find("U arm.step").Find("Gun turret.step").Find("Components").Find(installPosition);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            gameObject.name = $"Component_{robotId}_{index}";
        }

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