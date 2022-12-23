using System;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot {
    /// <summary>
    /// 仅用于存储车体的id
    /// </summary>
    public class MERobotIdentifier : MonoBehaviourPun, IPunObservable {
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