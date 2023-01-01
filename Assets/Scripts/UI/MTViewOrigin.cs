using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MTViewOrigin : MonoBehaviourPunCallbacks
    {
        private const int VIEW_ORIGIN_PAGE_ID = 1;
        private const int VIEW_ORIGIN_PAGE_ID_ = 2;
        private static MTViewOrigin _instance;
        private const string VIEW_ORIGIN_PAGE_TITLE = "";
        private string roomNameCreate = "";
        private string roomNameJoin = "";
        private GUIStyle text_style = new GUIStyle();
        private TypedLobby typedLobby;
        private Dictionary<string, RoomInfo> myRoomList = new Dictionary<string, RoomInfo>();
        private string roomExistCreate = "";
        private string roomExistJoin = "";
        private string joinFail = "";
        private string createFail = "";
        public static MTViewOrigin getInstance() {
            return _instance;
        }

        private void Awake() {
            if (_instance == null) {
                _instance = this;
            }
        }

        private void OnGUI() {
            text_style = GUI.skin.textField;
            text_style.normal.textColor = Color.white;
            text_style.fontSize = 75;
             GUILayout.Window(VIEW_ORIGIN_PAGE_ID, new Rect(0,
                    0 ,
                    Screen.width/2 , Screen.height), _ =>
                {
                    GUILayout.BeginVertical("Box");
                    GUILayout.Label(roomExistCreate,GUILayout.ExpandHeight(true));
                    roomNameCreate=GUILayout.TextField(roomNameCreate,9,text_style,GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true));
                    GUILayout.Label("",GUILayout.ExpandHeight(true));
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(createFail,GUILayout.ExpandWidth(false));
                    if (GUILayout.Button("创建房间",GUILayout.ExpandHeight(false),GUILayout.Height(Screen.height/6))) {
                        if (roomNameCreate.Length>2) {
                            if (myRoomList.ContainsKey(roomNameCreate)) {
                                roomExistCreate = "房间已存在";
                            }
                            else {
                                roomExistCreate = "";
                                RoomOptions roomOptions = new RoomOptions { MaxPlayers = 10 };
                                PhotonNetwork.CreateRoom(roomNameCreate, roomOptions);
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                },
                 VIEW_ORIGIN_PAGE_TITLE);
             GUILayout.Window(VIEW_ORIGIN_PAGE_ID_, new Rect(Screen.width/2,
                     0 ,
                     Screen.width/2 , Screen.height), _ =>
                 {
                     GUILayout.BeginVertical("Box");
                     GUILayout.Label(roomExistJoin,GUILayout.ExpandHeight(true));
                     roomNameJoin=GUILayout.TextField(roomNameJoin,9,text_style,GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true));
                     GUILayout.Label("",GUILayout.ExpandHeight(true));
                     GUILayout.BeginHorizontal("Box");
                     GUILayout.Label(joinFail,GUILayout.ExpandWidth(false));
                     if (GUILayout.Button("加入房间",GUILayout.ExpandHeight(false),GUILayout.Height(Screen.height/6))) {
                         if (roomNameJoin.Length>2) {
                             roomExistJoin = "";
                             PhotonNetwork.JoinRoom(roomNameJoin);
                             if (myRoomList.ContainsKey(roomNameJoin)) {
								 // FIXME
                             }
                             else {
                                 roomExistJoin = "不存在此房间";
                             }
                         }
                     }
                     GUILayout.EndHorizontal();
                     GUILayout.EndVertical();
                 },
                 VIEW_ORIGIN_PAGE_TITLE);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList) {
            base.OnRoomListUpdate(roomList);

            //遍历房间信息改变了的房间，注意这个roomList不包括所有房间，而是有属性改变了的房间，比如新增的
            foreach (var r in roomList)
            {
                //清除关闭或不可显示或已经移除了的房间
                if (!r.IsOpen || !r.IsVisible || r.RemovedFromList)
                {
                    if (myRoomList.ContainsKey(r.Name))//如果该房间之前有，现在没有了就去掉它
                    {
                        myRoomList.Remove(r.Name);
                    }
                    continue;
                }
                //更新房间信息
                if (myRoomList.ContainsKey(r.Name))
                {
                    myRoomList[r.Name] = r;
                }
                //添加新房间
                else
                {
                    myRoomList.Add(r.Name, r);//如果该房间之前没有，现在有了就加进myRoomList
                }
            }
        }

        public override void OnJoinedRoom() {
            base.OnJoinedRoom();
            _instance.enabled = false;
            MTViewStart.GetInstance().enabled = true;
            Events.Invoke(Events.M_PLAYER_ATTEND, new object[] { PhotonNetwork.LocalPlayer });
        }

        public override void OnJoinRoomFailed(short returnCode, string message) {
            base.OnJoinRoomFailed(returnCode, message);
            joinFail = "此房间不存在";
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            base.OnCreateRoomFailed(returnCode, message);
            createFail = "此房间已存在";
        }
    }
}