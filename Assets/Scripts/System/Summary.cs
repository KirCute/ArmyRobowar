using System.Collections.Generic;
using Equipment.Basement;
using Model;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace System {
    /// <summary>
    /// 获取游戏全部动态数据的脚本
    /// </summary>
    public class Summary : MonoBehaviour {
        public static bool isGameStarted;  // 游戏是否开始
        public static bool isTeamLeader;  // 是否为队长
        public static Team team { get; private set; }  // 整句游戏唯一Team对象

        private void Start() {
            team = new Team();  // 防止有脚本先于Summary触发F_GAME_START回调函数，提前构造Team，通过修改Team的teamColor属性实现队伍身份识别
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
            var team0Players = new List<Player>();  // 0队玩家列表
            var team1Players = new List<Player>();  // 1队玩家列表

            var startTime = (double) args[index++];  // 游戏开始时间
            var base0 = (int) args[index++];  // 0队基地
            var base1 = (int) args[index++];  // 1队基地
            var team0PlayerCnt = (int) args[index++];   // 0队玩家数
            for (var i = 0; i < team0PlayerCnt; i++) {
                team0Players.Add((Player) args[index++]);
            }

            var team1PlayerCnt = (int) args[index++];  // 1队玩家数
            for (var i = 0; i < team1PlayerCnt; i++) {
                team1Players.Add((Player) args[index++]);
            }
            
            // 得到本客户端的队号
            var teamId = team0Players.Contains(PhotonNetwork.LocalPlayer) ? 0 : 1;
            // 更新team对象
            team.teamColor = teamId;
            team.startTime = startTime;
            if (teamId == 0) {
                foreach (var player in team0Players) team.members.Add(player);
                isTeamLeader = team0Players[0].Equals(PhotonNetwork.LocalPlayer);
            } else {
                foreach (var player in team1Players) team.members.Add(player);
                isTeamLeader = team1Players[0].Equals(PhotonNetwork.LocalPlayer);
            }
            
            // 不能用占领基地事件，这是因为从机网络环境可能较差，此处可能还未初始化队伍号
            GameObject.Find($"Base_{base0}").GetComponentInChildren<MEBaseFlag>().CaptureBy(0);
            GameObject.Find($"Base_{base1}").GetComponentInChildren<MEBaseFlag>().CaptureBy(1);
        }

        private static void OnRestart(object[] _) {
            isGameStarted = false;
        }
    }
}