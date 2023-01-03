using System;
using System.Collections.Generic;
using System.Linq;
using Map.Navigation;
using Model.Equipment;
using UnityEngine;
using UnityEngine.UI;
using UI;

namespace UI {
    public class MTViewGlobalMap : MonoBehaviour {
        public GameObject blackMask;
        public GameObject map;
        public GameObject Base0;
        public GameObject Base1;
        public GameObject Base2;
        public GameObject Base3;
        public GameObject Base4;
        public GameObject Base5;
        
        private const int VIEW_MAP_PAGE_ID = 0;
        private const float VIEW_MAP_PAGE_WIDTH = 0.8F;
        private const float VIEW_MAP_PAGE_HEIGHT = 0.8F;
        private const string VIEW_MAP_PAGE_TITLE = "自主导航路径设置";

        private Vector2 robotScroll = Vector2.zero;
        private int selectedRobot;
        private static GameObject[] blackMasks = new GameObject[46 * 34];
        private GameObject mapGameObject;
        private List<Vector2> positionInWorld = new List<Vector2>();

        private void Awake()
        {
            mapGameObject = Instantiate(map,
                transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
            
            mapGameObject.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            mapGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(460, 340);
            mapGameObject.SetActive(false);

            
            CreateBase();
            
            CreateMask();
        }

        void OnGUI() {
            if(!Summary.isGameStarted) return;
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
            mapGameObject.SetActive(true);
//            bool last = true;
            //TODO,机器人，基地，信号塔位置
            for (int i = 0; i < 34; i++) {
                for (int j = 0; j < 46; j++) {
                    int temp = (33-i)*46+j;//坐标系不同
//                    last = !last;
//                    Summary.team.teamMap[temp] = last;
                    if (Summary.team.teamMap[temp]) {
                        blackMasks[temp].SetActive(false);
                    }
                    else {
                        blackMasks[temp].SetActive(true);
                    }
                }
            }
            
            robotScroll = GUILayout.BeginScrollView(robotScroll, false, false);
            GUILayout.BeginArea(new Rect(608,0,148,Screen.height));
            GUILayout.BeginVertical("Box");//未失联机器人列表
            foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                GUILayout.BeginVertical("Box");//单个机器人
                GUILayout.Label(robot.name, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("选择机器人", GUILayout.ExpandWidth(false))) {
                    selectedRobot = robot.id;
                    Debug.Log("selectRobot_"+selectedRobot);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
            if (GUILayout.Button("开始导航")) {
                var temp = GameObject.Find($"Robot_{selectedRobot}");
                positionInWorld.Insert(0,new Vector2(temp.transform.position.x,temp.transform.position.z));
                List<Vector2> path = MDNavigationCenter.GetInstance().GetFinalPath(positionInWorld);
                var input = new object[path.Count + 2];
                input[0] = selectedRobot;
                input[1] = path.Count;
                for (var i = 2; i < path.Count + 2; i++) input[i] = path[1 - 2];
                Events.Invoke(Events.M_ROBOT_NAVIGATION, input);
                Debug.Log("发出导航事件");
            }
            GUILayout.EndArea();
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }
        
        void CreateMask() {
            for (int m = 0; m < 34; m++) {
                for (int n = 0; n < 46; n++)
                {
                    GameObject temp = Instantiate(blackMask,
                        transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
                    blackMasks[m * 46 + n] = temp;
                    blackMasks[m * 46 + n].name = "blackMask_" + (m * 46 + n);
                    blackMasks[m * 46 + n].GetComponent<RectTransform>().localPosition =
                        new Vector2(-230 + 5 + n * 10, -170 + 5 + m * 10);
                    blackMasks[m * 46 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
                    blackMasks[m * 46 + n].SetActive(false);
                    AddListener(
                        transform.Find("Canvas").Find("Naviga").Find("blackMask_" + (m * 46 + n))
                            .GetComponent<Button>(), m, n);
                }
            }
        }

        void CreateBase()
        {
            GameObject Base_0 = Instantiate(Base0, GetComponent<RectTransform>());
            int[] mapPose=MTGlobalMapPoint.World2Map(-60.0, -40.0);
            Base_0.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(mapPose[0], mapPose[1]);
            Base_0.SetActive(true);
            
            GameObject Base_1 = Instantiate(Base1, GetComponent<RectTransform>());
            mapPose=MTGlobalMapPoint.World2Map(-60.0, 5.0);
            Base_1.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(mapPose[0], mapPose[1]);
            Base_1.SetActive(true);
            
            GameObject Base_2 = Instantiate(Base2, GetComponent<RectTransform>());
            mapPose=MTGlobalMapPoint.World2Map(-40.0, 45.0);
            Base_2.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(mapPose[0], mapPose[1]);
            Base_2.SetActive(true);
            
            GameObject Base_3 = Instantiate(Base3, GetComponent<RectTransform>());
            mapPose=MTGlobalMapPoint.World2Map(60.0, -25.0);
            Base_3.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(mapPose[0], mapPose[1]);
            Base_3.SetActive(true);
            
            GameObject Base_4 = Instantiate(Base4, GetComponent<RectTransform>());
            mapPose=MTGlobalMapPoint.World2Map(18.0, -45.0);
            Base_4.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(mapPose[0], mapPose[1]);
            Base_4.SetActive(true);
            
            GameObject Base_5 = Instantiate(Base5, GetComponent<RectTransform>());
            mapPose=MTGlobalMapPoint.World2Map(43.0, 35.0);
            Base_5.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(mapPose[0], mapPose[1]);
            Base_5.SetActive(true);
        }

        void TransformPosition(int m,int n) {
            //屏幕坐标转化为世界坐标
            Vector2 temp = new Vector2(-61 + 61 / (17 * 2) + (33-m) * 61 / 17, -82 + 82 / (23 * 2) + n * 82 / 23);
            positionInWorld.Add(temp);
            Debug.Log("clickPosition " + (33-m) + "," + n);
            Debug.Log("worldPosition " + Convert.ToString(-61 + 61 / (17 * 2) + (33-m) * 61 / 17) + "," + Convert.ToString(-82 + 82 / (23 * 2) + n * 82 / 23));
        }
        
        void AddListener(Button button, int m,int n)
        {
            button.onClick.AddListener(delegate { TransformPosition(m,n); });
        }
        
    }
}