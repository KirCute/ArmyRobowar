using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MTViewOrigin : MonoBehaviourPunCallbacks
    {
        private const int VIEW_START_PAGE_ID = 1;
        private static MTViewOrigin _instance;
        private const string VIEW_START_PAGE_TITLE = "";

        public static MTViewOrigin getInstance() {
            return _instance;
        }

        private void Awake() {
            if (_instance == null) {
                _instance = this;
            }
        }

        public override void OnConnectedToMaster() {
            base.OnConnectedToMaster();
            TypedLobby typedLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);//设置当前大厅类型为sqllobby
            PhotonNetwork.JoinLobby(typedLobby);//加入大厅
            Debug.Log("加入大厅成功");
        }

        private void Start() {
            PhotonNetwork.ConnectUsingSettings();
        }

        private void OnGUI() {
             GUILayout.Window(VIEW_START_PAGE_ID, new Rect(0,
                    0 ,
                    Screen.width , Screen.height), _ =>
                {
                    GUILayout.BeginVertical("Box");
                    
                    GUILayout.Label("",GUILayout.ExpandHeight(true));
                    GUILayout.BeginHorizontal("Box");
                    if (GUILayout.Button("创建房间",GUILayout.ExpandHeight(false),GUILayout.Height(Screen.height/6))) {
                        //TODO
                    }
                    if (GUILayout.Button("加入房间",GUILayout.ExpandHeight(false),GUILayout.Height(Screen.height/6))); {
                        //TODO
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                },
                VIEW_START_PAGE_TITLE);
        }
        
    }
}