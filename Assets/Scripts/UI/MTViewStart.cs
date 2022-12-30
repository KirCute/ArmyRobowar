using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Random = System.Random;

namespace UI
{
    public class MTViewStart : MonoBehaviourPunCallbacks
    {
        private static MTViewStart _instance;
        private const int VIEW_START_PAGE_ID = 0;

        private const string VIEW_START_PAGE_TITLE = "";
        
        private readonly GUIStyle style = new(); //定义控件

        private readonly List<Player> tempTestRed = new();//1
        private readonly List<Player> tempTestBlue = new();//0
        private readonly Dictionary<Player, bool> ready = new();
        private static Random _random = new Random();
        private int blueHome;
        private int redHome;
        private int myTeam;

        public static MTViewStart getInstance() {
            return _instance;
        }
        private void Awake() {
            _instance = this;
            do {
                blueHome = _random.Next(0, 5);
                redHome = _random.Next(0, 5);
            } while (blueHome == redHome);
        }

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
                    if (PhotonNetwork.IsMasterClient) {
                        if (GUILayout.Button("开始游戏", style)) {
                            if (IsAllReady()) {
                                var input = new object[5 + tempTestBlue.Count + tempTestRed.Count];
                                input[0] = PhotonNetwork.Time;
                                input[1] = blueHome;
                                input[2] = redHome;
                                input[3] = tempTestBlue.Count;
                                input[4] = tempTestBlue[0];
                                int cntBlue = 1;
                                for (int i = 5; i < 5+tempTestBlue.Count-1; i++) {
                                    input[i] = tempTestBlue[cntBlue];
                                    cntBlue++;
                                }
                                input[3 + tempTestBlue.Count] = tempTestRed.Count;
                                input[3 + tempTestBlue.Count + 1] = tempTestBlue[0];
                                int cntRed = 1;
                                for (int i = 3 + tempTestBlue.Count + 1 + 1; i < 5 + tempTestBlue.Count + tempTestRed.Count; i++) {
                                    input[i] = tempTestRed[cntRed];
                                    cntRed++;
                                }
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
                    if (GUILayout.Button("交换队伍",style)) {
                        if (myTeam == 0) {
                            if (tempTestRed.Count == 5) {
                                
                            }
                        }
                        Events.Invoke(Events.M_CHANGE_TEAM, new object[] {PhotonNetwork.LocalPlayer, 1 - myTeam});
                    }

                    if (GUILayout.Button("离开匹配",style)) {
                        Events.Invoke(Events.M_LEAVE_MATCHING, new object[] {PhotonNetwork.LocalPlayer});
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    
                    GUILayout.BeginVertical("Box");
                    foreach (var player in tempTestBlue) {
                        if (ready[player]) {
                            GUIStyle styleTemp = new GUIStyle();
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
                        GUIStyle styleTemp = new GUIStyle();
                        styleTemp.alignment = TextAnchor.MiddleCenter;
                        styleTemp.fontSize = 24;
                        styleTemp.normal.textColor = Color.magenta;
                        GUILayout.Box("NULL",styleTemp,GUILayout.ExpandHeight(true));
                    }
                    GUILayout.EndVertical();
                    
                    GUILayout.BeginVertical("Box");
                    foreach (var player in tempTestRed) {
                        if (ready[player]) {
                            GUIStyle styleTemp = new GUIStyle();
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
                        GUIStyle styleTemp = new GUIStyle();
                        styleTemp.alignment = TextAnchor.MiddleCenter;
                        styleTemp.fontSize = 24;
                        styleTemp.normal.textColor = Color.magenta;
                        GUILayout.Box("NULL",styleTemp,GUILayout.ExpandHeight(true));
                    }
                    GUILayout.EndVertical();
                    
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                },
                VIEW_START_PAGE_TITLE);
        }

        private void OnPlayerAttend(object[] args) {
            if (PhotonNetwork.IsMasterClient) Events.Invoke(Events.M_PLAYER_READY, new object[] {PhotonNetwork.LocalPlayer, true});
            var player = (Player) args[0];
            ready.Add(player, false);
        }

        public override void OnJoinedRoom() {
            base.OnJoinedRoom();
            var player = PhotonNetwork.LocalPlayer;
            if(!PhotonNetwork.IsMasterClient){
                Events.Invoke(Events.M_PLAYER_ATTEND, new object[] { player });
            }
            if (tempTestBlue.Count == 0 && tempTestRed.Count == 0) {
                var team = 0;  
                Events.Invoke(Events.M_CHANGE_TEAM, new object[] {player, team});
            }else if (tempTestBlue.Count==5&&tempTestRed.Count<5) {
                var team = 1;  
                Events.Invoke(Events.M_CHANGE_TEAM, new object[] {player, team});
            }else if (tempTestRed.Count==5&&tempTestBlue.Count<5) {
                var team = 0;  
                Events.Invoke(Events.M_CHANGE_TEAM, new object[] {player, team});
            }else if (tempTestBlue.Count==tempTestRed.Count) {
                var team = 0;  
                Events.Invoke(Events.M_CHANGE_TEAM, new object[] {player, team});
            }else if (tempTestBlue.Count > tempTestRed.Count) {
                var team = 1;  
                Events.Invoke(Events.M_CHANGE_TEAM, new object[] {player, team});
            }else {
                var team = 0;  
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