﻿using System;
using System.Collections.Generic;
using System.Linq;
using Map.Navigation;
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
        private int selectedRobot;
        private static GameObject[] blackMasks = new GameObject[46 * 34];
        private GameObject mapGameObject;
        private List<Vector2> positionInWorld = new List<Vector2>();
//        private int m = 0, n = 0;

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
                positionInWorld.Clear();
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
            //bool last = true;
            for (int i = 0; i < 34; i++) {
                for (int j = 0; j < 46; j++) {
                    int temp = i * 46 + j;
                    //last = !last;
                    //Summary.team.teamMap[temp] = last;
                    if (Summary.team.teamMap[temp]) {
                        blackMasks[temp].SetActive(false);
                    }
                    else {
                        blackMasks[temp].SetActive(true);
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
                    selectedRobot = robot.id;
                }
                GUILayout.EndVertical();
            }

            if (GUILayout.Button("开始导航")) {
                var temp = GameObject.Find($"Robot_{selectedRobot}");
                positionInWorld.Insert(0,new Vector2(temp.transform.position.x,temp.transform.position.z));
                List<Vector2> path = MDNavigationCenter.GetInstance().GetFinalPath(positionInWorld);
                var input = new object[path.Count + 2];
                input[0] = selectedRobot;
                input[1] = path.Count;
                for (var i = 2; i < path.Count + 2; i++) input[i] = path[1 - 2];
                Events.Invoke(Events.M_ROBOT_NAVIGATION, input);
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        
        void CreateMask() {
            for (int m = 0; m < 34; m++) {
                for (int n = 0; n < 46; n++) {
                    GameObject temp = Instantiate(blackMask,transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
                    blackMasks[m * 46 + n] = temp;
                    blackMasks[m * 46 + n].name = "blackMask_" + (m * 46 + n);
                    blackMasks[m * 46 + n].GetComponent<RectTransform>().localPosition = new Vector2(-230+5+n*10, -170+5+m*10);
                    blackMasks[m * 46 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
                    blackMasks[m * 46 + n].SetActive(false);
                    AddListener(transform.Find("Canvas").Find("Naviga").Find("blackMask_" + (m * 46 + n)).GetComponent<Button>(),m,n);
                }
            }
        }

        void TransformPosition(int m,int n) {
            //屏幕坐标转化为世界坐标
            Vector2 temp = new Vector2(-61 + 61 / 17 * 2 + m * 61 / 17, -82 + 82 / 23 * 2 + n * 82 / 23);
            positionInWorld.Add(temp);
            Debug.Log("clickPosition " + m + "," + n);
        }
        
        void AddListener(Button button, int m,int n)
        {
            button.onClick.AddListener(delegate { TransformPosition(m,n); });
        }
    }
}