using System.Collections.Generic;
using Model;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace System {
    public class Summary : MonoBehaviour {
        public static bool isGameStarted;
        public static bool isTeamLeader;
        public static Team team { get; private set; }

        private void Start() {
            Events.AddListener(Events.F_GAME_START, OnGameStart);
        }

        private static void OnGameStart(object[] args) {
            isGameStarted = true;
            var index = 0;
            var team0Players = new List<Player>();
            var team1Players = new List<Player>();

            var startTime = (double) args[index++];
            var base0 = (int) args[index++];
            var base1 = (int) args[index++];
            var team0PlayerCnt = (int) args[index++];
            for (var i = 0; i < team0PlayerCnt; i++) {
                team0Players.Add((Player) args[index++]);
            }

            var team1PlayerCnt = (int) args[index++];
            for (var i = 0; i < team1PlayerCnt; i++) {
                team1Players.Add((Player) args[index++]);
            }
            
            var teamId = team0Players.Contains(PhotonNetwork.LocalPlayer) ? 0 : 1;
			Events.Invoke(Events.LOG, new object[] {$"{PhotonNetwork.LocalPlayer} - {teamId}"});
            if (teamId == 0) {
                team = new Team(teamId, team0Players, startTime);
                if (team0Players[0].Equals(PhotonNetwork.LocalPlayer)) {
                    isTeamLeader = true;
                    PhotonNetwork.Instantiate("TeamHelper", Vector3.zero, Quaternion.identity);
                    Events.Invoke(Events.M_CAPTURE_BASE, new object[] {base0, 0});
                }
            } else {
                team = new Team(teamId, team1Players, startTime);
                if (team1Players[0].Equals(PhotonNetwork.LocalPlayer)) {
                    isTeamLeader = true;
                    PhotonNetwork.Instantiate("TeamHelper", Vector3.zero, Quaternion.identity);
                    Events.Invoke(Events.M_CAPTURE_BASE, new object[] {base1, 1});
                }
            }
        }
    }
}