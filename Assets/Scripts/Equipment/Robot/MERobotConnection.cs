using System;
using Photon.Pun;

namespace Equipment.Robot {
    /// <summary>
    /// 用于记录机器人的信号连接信号，发送弱信号警告和失联警告
    /// </summary>
    public class MERobotConnection : MonoBehaviourPun, IPunObservable {
        private const int STRONG_CONNECTION_STANDARD = 10000;
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponent<MERobotIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_CHANGE_CONNECTION, OnConnectionChanging);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_CHANGE_CONNECTION, OnConnectionChanging);
        }

        private void OnConnectionChanging(object[] args) {
            if (identity.id == (int) args[0] && identity.team == Summary.team.teamColor && photonView.IsMine) {
                var delta = (int) args[1];
                var conn = Summary.team.robots[identity.id].connection;
                Summary.team.robots[identity.id].connection += delta;
                switch (conn + delta) {  // 发送信号提示
                    case <= 0 when conn != 0:
                        Events.Invoke(Events.F_ROBOT_LOST_CONNECTION, new object[] {identity.id});
                        break;
                    case > 0 and < STRONG_CONNECTION_STANDARD when conn is 0 or > STRONG_CONNECTION_STANDARD:
                        Events.Invoke(Events.F_ROBOT_WEAK_CONNECTION, new object[] {identity.id});
                        break;
                    case >= STRONG_CONNECTION_STANDARD when conn < STRONG_CONNECTION_STANDARD:
                        Events.Invoke(Events.F_ROBOT_STRONG_CONNECTION, new object[] {identity.id});
                        break;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (identity.team == Summary.team.teamColor) {
                if (stream.IsWriting) {
                    stream.SendNext(Summary.team.robots[identity.id].connection);
                } else {
                    Summary.team.robots[identity.id].connection = (int) stream.ReceiveNext();
                }
            }
        }
    }
}