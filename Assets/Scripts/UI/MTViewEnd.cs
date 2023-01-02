using System;
using System.Diagnostics.Tracing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MTViewEnd : MonoBehaviourPunCallbacks
    {
        private int winner;
        private const int VIEW_END_PAGE_ID = 0;
        private const string VIEW_END_PAGE_TITLE = "";
        private string[] scenePaths;
        private AssetBundle myLoadedAssetBundle;
        private void Start() {
            myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
            scenePaths = myLoadedAssetBundle.GetAllScenePaths();
        }

        private void OnGUI() {
            GUILayout.Window(VIEW_END_PAGE_ID, new Rect(0,
                0,
                Screen.width, Screen.height), _ =>
            {
                GUILayout.BeginVertical();
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = 50;
                style.normal.textColor = Color.white;
                GUILayout.Label("游戏结束",style);
                if (winner == 0) {
                    GUIStyle styleTemp = new GUIStyle();
                    styleTemp.alignment = TextAnchor.MiddleCenter;
                    styleTemp.fontSize = 50;
                    styleTemp.normal.textColor = Color.blue;
                    GUILayout.Label("蓝队获胜",styleTemp);
                }
                else {
                    GUIStyle styleTemp = new GUIStyle();
                    styleTemp.alignment = TextAnchor.MiddleCenter;
                    styleTemp.fontSize = 50;
                    styleTemp.normal.textColor = Color.red;
                    GUILayout.Label("红队获胜",styleTemp);
                }
                if (GUILayout.Button("重新开始")) {
                    this.enabled = false;
                    PhotonNetwork.LeaveRoom();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
                }
                GUILayout.EndVertical();
            }, VIEW_END_PAGE_TITLE);
        }
        

        public void OnGameOver(object[] args) {
            if (args.Length == 1) {
                this.enabled = true;
                winner =(int) args[0];
            }
        }
        public override void OnEnable() {
            base.OnEnable();
            
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
            
        }
    }
}