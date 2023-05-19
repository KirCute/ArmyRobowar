using Photon.Pun;
using UnityEngine;

namespace UI {
    public class MTViewOriginMore : MonoBehaviourPunCallbacks {
        private const int VIEW_ORIGIN_MORE_PAGE_ID = 0;
        private const string VIEW_ORIGIN_MORE_PAGE_TITLE = "";
        private string nickname = "";

        private void Start() {
            PhotonNetwork.ConnectUsingSettings(); //初始化设置，连接服务器
        }

        private void OnGUI() {
            var labelStyle = GUI.skin.label;
            labelStyle.fontSize = 15;
            GUILayout.Window(VIEW_ORIGIN_MORE_PAGE_ID, new Rect(0,
                0,
                Screen.width, Screen.height), _ => {
                var titleLabelStyle = new GUIStyle(GUI.skin.label) {
                    normal = {
                        textColor = Color.white
                    },
                    fontSize = 72,
                    alignment = TextAnchor.MiddleCenter
                };
                var textStyle = new GUIStyle(GUI.skin.textField) {
                    normal = {
                        textColor = Color.white
                    },
                    fontSize = 30
                };
                var anotherLabelStyle = new GUIStyle(GUI.skin.label) {
                    normal = {
                        textColor = Color.white
                    },
                    fontSize = 30
                };
                GUILayout.BeginVertical("Box");
                GUILayout.Label("Army: Robowar", titleLabelStyle, GUILayout.ExpandHeight(true));
                GUILayout.BeginHorizontal();
                GUILayout.Label("请输入你的名字:", anotherLabelStyle);
                nickname = GUILayout.TextField(nickname, textStyle, GUILayout.ExpandHeight(false),
                    GUILayout.ExpandWidth(true), GUILayout.Width(Screen.width / 3f),
                    GUILayout.Height(Screen.height / 8f));
                GUILayout.Label("(不少于两个字)                             ");
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("");
                if (GUILayout.Button("确定", GUILayout.ExpandWidth(true), GUILayout.Height(Screen.height / 8f))) {
                    if (nickname.Length <= 2) {
                        return;
                    }

                    PhotonNetwork.NickName = nickname;
                    enabled = false;
                    MTViewOrigin.GetInstance().enabled = true;
                }

                GUILayout.Label("");
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }, VIEW_ORIGIN_MORE_PAGE_TITLE);
        }

        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }

        public override void OnDisable() {
            base.OnDisable();
            Events.RemoveListener(Events.F_GAME_OVER, OnGameOver);
        }
    }
}