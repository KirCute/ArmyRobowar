using System;
using Photon.Pun;
using Photon.Realtime;

namespace Equipment.Robot {
    public class MERobotPlayerController : MonoBehaviourPun, IPunObservable {
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_CONTROL, OnControlled);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_CONTROL, OnControlled);
        }

        private void OnControlled(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                Summary.team.robots[identity.id].controller = (Player) args[1];
            }
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (Summary.team.teamColor == identity.team) {
                if (stream.IsWriting) {
                    stream.SendNext(Summary.team.robots[identity.id].controller);
                } else {
                    Summary.team.robots[identity.id].controller = (Player) stream.ReceiveNext();
                }
            }
        }
    }
}