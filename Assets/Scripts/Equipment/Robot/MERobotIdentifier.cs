using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot {
    /// <summary>
    /// 仅用于存储车体的id
    /// </summary>
    public class MERobotIdentifier : MonoBehaviourPun, IPunObservable {
        [SerializeField] private List<Color> lightColors;
        public int id { get; set; }
        public int team { get; set; }

        public void Awake() {
            id = (int) photonView.InstantiationData[0];
            team = (int) photonView.InstantiationData[1];
            gameObject.name = $"Robot_{id}";
            foreach (var lightRenderer in GetComponentsInChildren<Light>()) {
                lightRenderer.color = lightColors[team];
            }
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