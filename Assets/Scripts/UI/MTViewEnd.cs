﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class MTViewEnd : MonoBehaviourPunCallbacks {
        private int winner = -1;
        private const int VIEW_END_PAGE_ID = 0;
        private const string VIEW_END_PAGE_TITLE = "";

        private void OnGUI() {
            if (winner == -1) return;
            Cursor.lockState = CursorLockMode.None;
            GUILayout.Window(VIEW_END_PAGE_ID, new Rect(0, 0, Screen.width, Screen.height), _ => {
                GUILayout.BeginVertical();
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = 50;
                style.normal.textColor = Color.white;
                GUILayout.Label("游戏结束", style);
                GUIStyle styleTemp = new GUIStyle();
                styleTemp.alignment = TextAnchor.MiddleCenter;
                styleTemp.fontSize = 50;
                styleTemp.normal.textColor = winner == 0 ? Color.blue : Color.red;
                GUILayout.Label(winner == 0 ? "蓝队获胜" : "红队获胜", styleTemp);

                if (GUILayout.Button("重新开始")) {
                    this.enabled = false;
                    PhotonNetwork.LeaveRoom();
                    SceneManager.LoadScene(0, LoadSceneMode.Single);
                    Events.Invoke(Events.F_RESTART, new object[] {});
                }

                GUILayout.EndVertical();
            }, VIEW_END_PAGE_TITLE);
        }


        private void OnGameOver(object[] args) {
            winner = (int) args[0];
        }

        public override void OnEnable() {
            base.OnEnable();
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }
    }
}