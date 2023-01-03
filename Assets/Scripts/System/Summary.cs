using System.Collections.Generic;
using Equipment.Basement;
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
            team = new Team();
        }

        private void OnEnable() {
            Events.AddListener(Events.F_GAME_START, OnGameStart);
            Events.AddListener(Events.F_RESTART, OnRestart);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_GAME_START, OnGameStart);
            Events.RemoveListener(Events.F_RESTART, OnRestart);
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
            team.teamColor = teamId;
            team.startTime = startTime;
            if (teamId == 0) {
                foreach (var player in team0Players) team.members.Add(player);
                isTeamLeader = team0Players[0].Equals(PhotonNetwork.LocalPlayer);
            } else {
                foreach (var player in team1Players) team.members.Add(player);
                isTeamLeader = team1Players[0].Equals(PhotonNetwork.LocalPlayer);
            }
            
            GameObject.Find($"Base_{base0}").GetComponentInChildren<MEBaseFlag>().CaptureBy(0);
            GameObject.Find($"Base_{base1}").GetComponentInChildren<MEBaseFlag>().CaptureBy(1);
        }

        private static void OnRestart(object[] _) {
            isGameStarted = false;
        }
    }
}