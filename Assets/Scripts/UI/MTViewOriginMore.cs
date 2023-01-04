using Photon.Pun;
using UnityEngine;

namespace UI
{
    public class MTViewOriginMore : MonoBehaviourPunCallbacks
    {
        private const int VIEW_ORIGIN_MORE_PAGE_ID = 0;
        private const string VIEW_ORIGIN_MORE_PAGE_TITLE = "";
        private string nickname = "";

        private void Start() {
            PhotonNetwork.ConnectUsingSettings();//初始化设置，连接服务器
        }

        private void OnGUI() {
            GUIStyle labelStyle = GUI.skin.label;
            labelStyle.fontSize = 15;
            GUILayout.Window(VIEW_ORIGIN_MORE_PAGE_ID, new Rect(0,
                0,
                Screen.width, Screen.height), _ =>
            {
                GUIStyle text_style = new GUIStyle(GUI.skin.textField);
                GUIStyle label_style = new GUIStyle(GUI.skin.label);
                
                text_style.normal.textColor = Color.white;
                text_style.fontSize = 30;
                
                label_style.normal.textColor = Color.white;
                label_style.fontSize = 30;
                GUILayout.BeginVertical("Box");
                GUILayout.Label("",GUILayout.ExpandHeight(true));
                //GUILayout.Label("请输入你的名字:",GUILayout.ExpandHeight(false));
                GUILayout.BeginHorizontal();
                GUILayout.Label("请输入你的名字:",label_style);
                nickname =  GUILayout.TextField(nickname,text_style,GUILayout.ExpandHeight(false),GUILayout.ExpandWidth(true),GUILayout.Width(Screen.width/3),GUILayout.Height(Screen.height/8));
                GUILayout.Label("(不少于两个字)                             ");
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("");
                if (GUILayout.Button("确定",GUILayout.ExpandWidth(true),GUILayout.Height(Screen.height/8))) {
                    if (nickname.Length<=2) {
                        return;
                    }
                    PhotonNetwork.NickName = nickname;
                    enabled = false;
                    MTViewOrigin.getInstance().enabled = true;
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