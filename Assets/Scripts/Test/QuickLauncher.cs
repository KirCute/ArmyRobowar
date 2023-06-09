﻿using Photon.Pun;
using Photon.Realtime;

namespace Test {
    public class QuickLauncher : MonoBehaviourPunCallbacks {
        private void Start() {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster() {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinOrCreateRoom("DevelopmentTest", new RoomOptions {MaxPlayers = 2}, default);
        }

        public override void OnJoinedRoom() {
            if (!PhotonNetwork.IsMasterClient) {
                Events.Invoke(Events.F_GAME_START, new object[] {
                    PhotonNetwork.Time,
                    0, 5,
                    1, PhotonNetwork.MasterClient,
                    1, PhotonNetwork.LocalPlayer
                });
            }
        }
    }
}