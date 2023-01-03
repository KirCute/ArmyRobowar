using System;
using System.Collections.Generic;
using System.Linq;
using Model.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MTViewGlobalMap : MonoBehaviour {
        public GameObject blackMask;
        public GameObject map;
        
        private const int VIEW_MAP_PAGE_ID = 0;
        private const float VIEW_MAP_PAGE_WIDTH = 0.8F;
        private const float VIEW_MAP_PAGE_HEIGHT = 0.8F;
        private const string VIEW_MAP_PAGE_TITLE = "自主导航路径设置";
        
        private Vector2 robotScroll = Vector2.zero;
        private Robot selectedRobot;
        private static GameObject[] blackMasks = new GameObject[46 * 34];
        private GameObject mapGameObject;

        private void Awake() {
            mapGameObject = Instantiate(map,transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
            mapGameObject.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            mapGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(460, 340);
            mapGameObject.SetActive(false);
            CreateMask();
        }

        private void OnEnable()
        {
            //throw new NotImplementedException();
        }

        void OnGUI() {
            if(!Summary.isGameStarted) return;
            /*var dim = new Rect(Screen.width * (1 - VIEW_MAP_PAGE_WIDTH) / 2,
                Screen.height * (1 - VIEW_MAP_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_MAP_PAGE_WIDTH,
                Screen.height * VIEW_MAP_PAGE_HEIGHT);*/
            var dim = new Rect(0, 0, Screen.width, Screen.height);
            GUILayout.Window(VIEW_MAP_PAGE_ID, dim, ViewGlobalMap,VIEW_MAP_PAGE_TITLE);

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) {
                enabled = false;
                mapGameObject.SetActive(false);
                for (int i = 0; i < 34; i++) {
                    for (int j = 0; j < 46; j++) {
                        int temp = i * 46 + j;
                        blackMasks[temp].SetActive(false);
                    }
                }
                GetComponent<MEMainCameraController>().active = true;
            }
        }

        void ViewGlobalMap(int id) {
            GUILayout.BeginHorizontal();
            /*GUI.DrawTexture(new Rect(Screen.width * (1 - VIEW_MAP_PAGE_WIDTH) / 2,
                Screen.height * (1 - VIEW_MAP_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_MAP_PAGE_WIDTH,
                Screen.height * VIEW_MAP_PAGE_HEIGHT ), Resources.Load<Texture>("map"));*/
            /*GameObject mapGameObject = Instantiate(map,GetComponent<RectTransform>());
            mapGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            mapGameObject.SetActive(true); 
            ChangeMask();*/
            mapGameObject.SetActive(true);
            for (int i = 0; i < 34; i++) {
                for (int j = 0; j < 46; j++) {
                    int temp = i * 46 + j;
                    if (Summary.team.teamMap[temp]) {
                        blackMasks[temp].SetActive(true);
                    }
                    else {
                        blackMasks[temp].SetActive(false);
                    }
                }
            }
            robotScroll = GUILayout.BeginScrollView(robotScroll, false, false);
            GUILayout.BeginVertical("Box");//未失联机器人列表
            foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                Debug.Log("in");
                GUILayout.BeginVertical("Box");//单个机器人
                GUILayout.Label(robot.name, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("选择机器人", GUILayout.ExpandWidth(false))) {
                    selectedRobot = robot;
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        void CreateMask() {
            for (int i = 0; i < 34; i++) {
                for (int j = 0; j < 46; j++) {
                    GameObject temp = Instantiate(blackMask,transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
                    blackMasks[i * 46 + j] = temp;
                    blackMasks[i * 46 + j].name = "blackMask_" + (i * 46 + j);
                    blackMasks[i * 46 + j].GetComponent<RectTransform>().localPosition = new Vector2(-230+5+j*10, -170+5+i*10);
                    blackMasks[i * 46 + j].GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
                    blackMasks[i * 46 + j].SetActive(false);
                }
            }
        }
    }
}