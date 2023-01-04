using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = System.Random;

namespace UI
{
    public class MTViewStart : MonoBehaviourPunCallbacks
    {
        private static MTViewStart instance;
        private const int VIEW_START_PAGE_ID = 0;

        private const string VIEW_START_PAGE_TITLE = "";
        
        

        private readonly List<Player> tempTestRed = new();  // 1, client-server
        private readonly List<Player> tempTestBlue = new();  // 0, client-server
        private readonly Dictionary<Player, bool> ready = new();  // client-server
        private static readonly Random RANDOM = new Random();
        private int blueHome;
        private int redHome;
        private int myTeam;  // client-server

        public static MTViewStart GetInstance() {
            return instance;
        }
        private void Awake() {
            instance = this;
            do {
                blueHome = RANDOM.Next(0, 5);
                redHome = RANDOM.Next(0, 5);
            } while (blueHome == redHome);
        }

        public override void OnEnable() {
            base.OnEnable();
            if (PhotonNetwork.IsMasterClient) {
                Events.AddListener(Events.M_PLAYER_ATTEND, OnPlayerAttend);
                Events.AddListener(Events.M_PLAYER_READY, OnPlayerReady);
                Events.AddListener(Events.M_CHANGE_TEAM, OnTeamChanging);
                Events.AddListener(Events.M_LEAVE_MATCHING, OnLeavingMatch);
            } else {
                Events.AddListener(Events.F_PLAYER_LIST_UPDATED, OnPlayerListSync);
            }
            Events.AddListener(Events.F_GAME_START, OnGameStart);
        }

        public override void OnDisable() {
            base.OnDisable();
            if (PhotonNetwork.IsMasterClient) {
                Events.RemoveListener(Events.M_PLAYER_ATTEND, OnPlayerAttend);
                Events.RemoveListener(Events.M_PLAYER_READY, OnPlayerReady);
                Events.RemoveListener(Events.M_CHANGE_TEAM, OnTeamChanging);
                Events.RemoveListener(Events.M_LEAVE_MATCHING, OnLeavingMatch);
            } else {
                Events.RemoveListener(Events.F_PLAYER_LIST_UPDATED, OnPlayerListSync);
            }
            Events.RemoveListener(Events.F_GAME_START, OnGameStart);
            
        }
        
        private void Start() {
       
        }
        
        private void OnGUI() {
            if (Summary.isGameStarted) return;
            GUIStyle style = new GUIStyle(GUI.skin.button); //定义控件
            style.alignment = TextAnchor.MiddleCenter; 
            style.fontSize = 24;
            GUILayout.Window(VIEW_START_PAGE_ID, new Rect(0,
                    0 ,
                    Screen.width , Screen.height), _ =>
                {
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                    if (PhotonNetwork.IsMasterClient) {
                        if (GUILayout.Button("开始游戏", style)) {
                            if (IsAllReady()) {
                                var input = new object[5 + tempTestBlue.Count + tempTestRed.Count];
                                var index = 0;
                                input[index++] = PhotonNetwork.Time;
                                input[index++] = blueHome;
                                input[index++] = redHome;
                                input[index++] = tempTestBlue.Count;
                                foreach (var player in tempTestBlue) {
                                    input[index++] = player;
                                }
                                input[index++] = tempTestRed.Count;
                                foreach (var player in tempTestRed) {
                                    input[index++] = player;
                                }
                                PhotonNetwork.CurrentRoom.IsOpen = false;
                                Events.Invoke(Events.F_GAME_START, input);
                            }
                        }
                    }
                    else{
                        if (!ready.ContainsKey(PhotonNetwork.LocalPlayer) ||
                            !ready[PhotonNetwork.LocalPlayer]) //按下准备后切换按钮，if里面放的是个人是否准备
                        {
                            if (GUILayout.Button("准备游戏", style)) {
                                Events.Invoke(Events.M_PLAYER_READY, new object[] { PhotonNetwork.LocalPlayer, true });
                            }
                        }
                        else {
                            if (GUILayout.Button("取消准备", style)) {
                                Events.Invoke(Events.M_PLAYER_READY, new object[] { PhotonNetwork.LocalPlayer, false });
                            }
                        }
                    }
                    if (myTeam == 0) {
                        if (tempTestRed.Count == 5) {
                            if (GUILayout.Button("队伍已满",style)) {}
                        }
                        else {
                            if (GUILayout.Button("交换队伍",style)) {
                                if (myTeam == 0) {
                                    if (tempTestRed.Count == 5) {
                                
                                    }
                                }
                                Events.Invoke(Events.M_CHANGE_TEAM, new object[] {PhotonNetwork.LocalPlayer, 1 - myTeam});
                            }
                        }
                    }
                    else {
                        if (tempTestBlue.Count == 5) {
                            if (GUILayout.Button("队伍已满",style)) {}
                        }
                        else {
                            if (GUILayout.Button("交换队伍",style)) {
                                if (myTeam == 0) {
                                    if (tempTestRed.Count == 5) {
                                
                                    }
                                }
                                Events.Invoke(Events.M_CHANGE_TEAM, new object[] {PhotonNetwork.LocalPlayer, 1 - myTeam});
                            }
                        }
                    }
                    

                    if (GUILayout.Button("离开匹配",style)) {
                        this.enabled = false;
                        MTViewOrigin.getInstance().enabled = true;
                        Events.Invoke(Events.M_LEAVE_MATCHING, new object[] {PhotonNetwork.LocalPlayer});
                        //  PhotonNetwork.ConnectUsingSettings();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    
                    GUILayout.BeginVertical("Box");
                    foreach (var player in tempTestBlue) {
                        if (ready[player]) {
                            GUIStyle styleTemp = new GUIStyle(GUI.skin.box);
                            styleTemp.alignment = TextAnchor.MiddleCenter;
                            styleTemp.fontSize = 24;
                            styleTemp.normal.textColor = Color.green;
                            GUILayout.Box(player.NickName,styleTemp,GUILayout.ExpandHeight(true));
                        }
                        else {
                            GUILayout.Box(player.NickName,style,GUILayout.ExpandHeight(true));
                        }
                    }

                    for (int i = 0; i < 5-tempTestBlue.Count; i++) {
                        GUIStyle styleTemp = new GUIStyle(GUI.skin.box);
                        styleTemp.alignment = TextAnchor.MiddleCenter;
                        styleTemp.fontSize = 24;
                        styleTemp.normal.textColor = Color.magenta;
                        GUILayout.Box("",styleTemp,GUILayout.ExpandHeight(true));
                    }
                    GUILayout.EndVertical();
                    
                    GUILayout.BeginVertical("Box");
                    foreach (var player in tempTestRed) {
                        if (ready[player]) {
                            GUIStyle styleTemp = new GUIStyle(GUI.skin.box);
                            styleTemp.alignment = TextAnchor.MiddleCenter;
                            styleTemp.fontSize = 24;
                            styleTemp.normal.textColor = Color.green;
                            GUILayout.Box(player.NickName,styleTemp,GUILayout.ExpandHeight(true));
                        }
                        else {
                            GUILayout.Box(player.NickName,style,GUILayout.ExpandHeight(true));
                        }
                    }

                    for (int i = 0; i < 5-tempTestRed.Count; i++) {
                        GUIStyle styleTemp = new GUIStyle(GUI.skin.box);
                        styleTemp.alignment = TextAnchor.MiddleCenter;
                        styleTemp.fontSize = 24;
                        styleTemp.normal.textColor = Color.magenta;
                        GUILayout.Box("",styleTemp,GUILayout.ExpandHeight(true));
                    }
                    GUILayout.EndVertical();
                    
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                },
                VIEW_START_PAGE_TITLE);
        }

        private void OnPlayerAttend(object[] args) {
            var player = (Player) args[0];
            if (player.IsMasterClient) {
                ready.Add(player, true);
                tempTestBlue.Add(player);
            } else {
                ready.Add(player, false);
                (tempTestBlue.Count > tempTestRed.Count ? tempTestRed : tempTestBlue).Add(player);
            }
            SyncPlayerList();
        }

        private void OnPlayerReady(object[] args) {
            ready[(Player) args[0]] = (bool) args[1];
            SyncPlayerList();
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
            SyncPlayerList();
        }

        private void OnLeavingMatch(object[] args) {
            var player = (Player) args[0];
            ready.Remove(player);
            if (tempTestRed.Contains(player)) tempTestRed.Remove(player);
            if (tempTestBlue.Contains(player)) tempTestBlue.Remove(player);
            SyncPlayerList();
        }

        private void OnPlayerListSync(object[] args) {
            tempTestBlue.Clear();
            tempTestRed.Clear();
            ready.Clear();
            var index = 0;
            var teamBlueCount = (int) args[index++];
            for (var i = 0; i < teamBlueCount; i++) {
                var player = (Player) args[index++];
                var playerReady = (bool) args[index++];
                tempTestBlue.Add(player);
                ready.Add(player, playerReady);
                if (Equals(player, PhotonNetwork.LocalPlayer)) myTeam = 0;
            }
            var teamRedCount = (int) args[index++];
            for (var i = 0; i < teamRedCount; i++) {
                var player = (Player) args[index++];
                var playerReady = (bool) args[index++];
                tempTestRed.Add(player);
                ready.Add(player, playerReady);
                if (Equals(player, PhotonNetwork.LocalPlayer)) myTeam = 1;
            }
        }

        private void SyncPlayerList() {
            var output = new object[2 + 2 * (tempTestBlue.Count + tempTestRed.Count)];
            var index = 0;
            output[index++] = tempTestBlue.Count;
            foreach (var t in tempTestBlue) {
                output[index++] = t;
                output[index++] = ready[t];
            }
            output[index++] = tempTestRed.Count;
            foreach (var t in tempTestRed) {
                output[index++] = t;
                output[index++] = ready[t];
            }

            Events.Invoke(Events.F_PLAYER_LIST_UPDATED, output);
        }

        private bool IsAllReady() {
            return ready.Values.All(v => v);
        }

        private void OnGameStart(object[] args) {
            enabled = false;
        }
        
    }
}