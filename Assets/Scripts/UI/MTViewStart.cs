using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace UI
{
    public class MTViewStart : MonoBehaviour
    {
        private const int VIEW_START_PAGE_ID = 0;

        private const string VIEW_START_PAGE_TITLE = "";
        
        private readonly GUIStyle style = new(); //定义控件

        private readonly List<Player> tempTestRed = new();
        private readonly List<Player> tempTestBlue = new();
        private readonly Dictionary<Player, bool> ready = new();
        private int myTeam;

        private void OnEnable() {
            Events.AddListener(Events.M_PLAYER_ATTEND, OnPlayerAttend);
            Events.AddListener(Events.M_PLAYER_READY, OnPlayerReady);
            Events.AddListener(Events.M_CHANGE_TEAM, OnTeamChanging);
            Events.AddListener(Events.M_LEAVE_MATCHING, OnLeavingMatch);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_PLAYER_ATTEND, OnPlayerAttend);
            Events.RemoveListener(Events.M_PLAYER_READY, OnPlayerReady);
            Events.RemoveListener(Events.M_CHANGE_TEAM, OnTeamChanging);
            Events.RemoveListener(Events.M_LEAVE_MATCHING, OnLeavingMatch);
        }
        
        private void Start() {
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 24;
        }
        
        private void OnGUI() {
            if (Summary.isGameStarted) return;
            GUILayout.Window(VIEW_START_PAGE_ID, new Rect(0,
                    0 ,
                    Screen.width , Screen.height), _ =>
                {
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                    if (!ready.ContainsKey(PhotonNetwork.LocalPlayer) || !ready[PhotonNetwork.LocalPlayer]) //按下准备后切换按钮，if里面放的是个人是否准备
                    {
                        if (GUILayout.Button("准备游戏", style))
                        {
                            Events.Invoke(Events.M_PLAYER_READY, new object[] {PhotonNetwork.LocalPlayer, true});
                        }
                    }
                    else {
                        if (GUILayout.Button("取消准备", style)) {
                            Events.Invoke(Events.M_PLAYER_READY, new object[] {PhotonNetwork.LocalPlayer, false});
                        }
                    }

                    if (GUILayout.Button("交换队伍",style)) {
                        Events.Invoke(Events.M_CHANGE_TEAM, new object[] {PhotonNetwork.LocalPlayer, 1 - myTeam});
                    }

                    if (GUILayout.Button("离开匹配",style)) {
                        Events.Invoke(Events.M_LEAVE_MATCHING, new object[] {PhotonNetwork.LocalPlayer});
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    
                    GUILayout.BeginVertical("Box");
                    GUILayout.Box("red1",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("red2",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("red3",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("red4",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("red5",style,GUILayout.ExpandHeight(true));
                    GUILayout.EndVertical();
                    
                    GUILayout.BeginVertical("Box");
                    GUILayout.Box("blue1",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("blue2",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("blue3",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("blue4",style,GUILayout.ExpandHeight(true));
                    GUILayout.Box("blue5",style,GUILayout.ExpandHeight(true));
                    GUILayout.EndVertical();
                    
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                },
                VIEW_START_PAGE_TITLE);
        }

        private void OnPlayerAttend(object[] args) {
            var player = (Player) args[0];
            ready.Add(player, false);
            if (PhotonNetwork.IsMasterClient) {
                var team = 0;  // TODO: 决定新玩家进入哪个队
                Events.Invoke(Events.M_CHANGE_TEAM, new object[] {player, team});
            }
        }

        private void OnPlayerReady(object[] args) {
            ready[(Player) args[0]] = true;
        }

        private void OnTeamChanging(object[] args) {
            var player = (Player) args[0];
            var team = (int) args[1];
            if (team == 0) {
                if (tempTestRed.Contains(player)) tempTestRed.Remove(player);
                tempTestBlue.Add(player);
            } else {
                if (tempTestBlue.Contains(player)) tempTestBlue.Remove(player);
                tempTestRed.Add(player);
            }

            if (Equals(player, PhotonNetwork.LocalPlayer)) myTeam = team;
        }

        private void OnLeavingMatch(object[] args) {
            var player = (Player) args[0];
            ready.Remove(player);
            if (tempTestRed.Contains(player)) tempTestRed.Remove(player);
            if (tempTestBlue.Contains(player)) tempTestBlue.Remove(player);
        }

        private bool IsAllReady() {
            return ready.Values.All(v => v);
        }
    }
}