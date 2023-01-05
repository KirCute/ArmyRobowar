using System;
using System.Collections.Generic;
using System.Linq;
using Equipment.Sensor.Lidar;
using Map.Navigation;
using Model.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MTViewGlobalMap : MonoBehaviour {
        [SerializeField] private GameObject blackMask;
        [SerializeField] private GameObject map;
        [SerializeField] private GameObject base0Prefab;
        [SerializeField] private GameObject base1Prefab;
        [SerializeField] private GameObject base2Prefab;
        [SerializeField] private GameObject base3Prefab;
        [SerializeField] private GameObject base4Prefab;
        [SerializeField] private GameObject base5Prefab;
        [SerializeField] private GameObject navMarkPrefab;
        [SerializeField] private GameObject navButtonPrefab;

        private const int VIEW_MAP_PAGE_ID = 0;
        private const float VIEW_MAP_PAGE_WIDTH = 0.8F;
        private const float VIEW_MAP_PAGE_HEIGHT = 0.8F;
        private const string VIEW_MAP_PAGE_TITLE = "自主导航路径设置";
        private static readonly Vector2[] BASE_POSITION = new []
        {
            new Vector2(-60.0F,-40.0F),new Vector2(-60.0F,5.0F),new Vector2(-40.0F,45.0F),
            new Vector2(60.0F,-25.0F),new Vector2(18.0F,-45.0F),new Vector2(43.0F,35.0F),
        };

        private Vector2 robotScroll = Vector2.zero;
        private int selectedRobot = -1;

        private readonly GameObject[] blackMasks =
            new GameObject[MTMapBuilder.MAP_CELL_COLUMN_CNT * MTMapBuilder.MAP_CELL_ROW_CNT];

        private GameObject mapGameObject;
        private readonly List<Vector2> positionInWorld = new();
        private GameObject base0;
        private GameObject base1;
        private GameObject base2;
        private GameObject base3;
        private GameObject base4;
        private GameObject base5;
        private readonly List<GameObject> navMarkList = new();
        private readonly GameObject[] navButtons =
            new GameObject[MTMapBuilder.MAP_CELL_COLUMN_CNT * MTMapBuilder.MAP_CELL_ROW_CNT];

        private void Awake() {
            mapGameObject = Instantiate(map,
                transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());

            mapGameObject.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            mapGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(MTMapBuilder.MAP_CELL_COLUMN_CNT * 10,
                MTMapBuilder.MAP_CELL_ROW_CNT * 10);
            mapGameObject.SetActive(false);
            
            CreateBase();
            CreateMask();
            CreateNavButton();
        }

        private void OnGUI() {
            if (!Summary.isGameStarted) return;

            var dim = new Rect(
                Screen.width * (1 - VIEW_MAP_PAGE_WIDTH) / 2, Screen.height * (1 - VIEW_MAP_PAGE_HEIGHT) / 2,
                Screen.width * VIEW_MAP_PAGE_WIDTH, Screen.height * VIEW_MAP_PAGE_HEIGHT
            );
            GUILayout.Window(VIEW_MAP_PAGE_ID, dim, ViewGlobalMap, VIEW_MAP_PAGE_TITLE);

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) {
                enabled = false;
                mapGameObject.SetActive(false);
                selectedRobot = -1;
                for (var i = 0; i < MTMapBuilder.MAP_CELL_ROW_CNT; i++) {
                    for (var j = 0; j < MTMapBuilder.MAP_CELL_COLUMN_CNT; j++) {
                        var temp = i * MTMapBuilder.MAP_CELL_COLUMN_CNT + j;
                        blackMasks[temp].SetActive(false);
                    }
                }

                for (var i = 0; i < MTMapBuilder.MAP_CELL_ROW_CNT; i++) {
                    for (var j = 0; j < MTMapBuilder.MAP_CELL_COLUMN_CNT; j++) {
                        var temp = i * MTMapBuilder.MAP_CELL_COLUMN_CNT + j;
                        navButtons[temp].SetActive(false);
                    }
                }
                
                base0.SetActive(false);
                base1.SetActive(false);
                base2.SetActive(false);
                base3.SetActive(false);
                base4.SetActive(false);
                base5.SetActive(false);

                if (GetComponent<MTGlobalMapPoint>().friendPointList != null) {
                    foreach (var friend in GetComponent<MTGlobalMapPoint>().friendPointList) {
                        friend.Value.SetActive(false);
                    }
                }

                if (GetComponent<MTGlobalMapPoint>().enemyPointList != null) {
                    foreach (var enemy in GetComponent<MTGlobalMapPoint>().enemyPointList) {
                        enemy.Value.SetActive(false);
                    }
                }

                if (GetComponent<MTGlobalMapPoint>().towerPointList != null) {
                    foreach (var tower in GetComponent<MTGlobalMapPoint>().towerPointList) {
                        tower.Value.SetActive(false);
                    }
                }

                foreach (var nav in navMarkList) {
                    Destroy(nav);
                }

                navMarkList.Clear();
                positionInWorld.Clear();
                GetComponent<MEMainCameraController>().active = true;
            }
        }

        private void ViewGlobalMap(int id) {
            GUILayout.BeginHorizontal();
            mapGameObject.SetActive(true);
            base0.SetActive(true);
            base1.SetActive(true);
            base2.SetActive(true);
            base3.SetActive(true);
            base4.SetActive(true);
            base5.SetActive(true);

            for (var i = 0; i < MTMapBuilder.MAP_CELL_ROW_CNT; i++) {
                for (var j = 0; j < MTMapBuilder.MAP_CELL_COLUMN_CNT; j++) {
                    var temp = (MTMapBuilder.MAP_CELL_ROW_CNT - i - 1) * MTMapBuilder.MAP_CELL_COLUMN_CNT + j; //坐标系不同
                    Debug.Log(Summary.team.teamMap[temp]);
                    blackMasks[temp].SetActive(!Summary.team.teamMap[temp]);
                }
            }
            
            for (var i = 0; i < MTMapBuilder.MAP_CELL_ROW_CNT; i++) {
                for (var j = 0; j < MTMapBuilder.MAP_CELL_COLUMN_CNT; j++) {
                    var temp = (MTMapBuilder.MAP_CELL_ROW_CNT - i - 1) * MTMapBuilder.MAP_CELL_COLUMN_CNT + j; //坐标系不同

                    navButtons[temp].SetActive(true);
                }
            }

            if (GetComponent<MTGlobalMapPoint>().friendPointList != null) {
                foreach (var friend in GetComponent<MTGlobalMapPoint>().friendPointList) {
                    friend.Value.SetActive(true);
                }
            }

            if (GetComponent<MTGlobalMapPoint>().enemyPointList != null) {
                foreach (var enemy in GetComponent<MTGlobalMapPoint>().enemyPointList) {
                    enemy.Value.SetActive(true);
                }
            }

            if (GetComponent<MTGlobalMapPoint>().towerPointList != null) {
                foreach (var tower in GetComponent<MTGlobalMapPoint>().towerPointList) {
                    tower.Value.SetActive(true);
                }
            }

            robotScroll = GUILayout.BeginScrollView(robotScroll, false, false);
            GUILayout.BeginArea(new Rect(608, 0, 148, Screen.height));
            GUILayout.BeginVertical("Box"); //未失联机器人列表
            foreach (var robot in Summary.team.robots.Values.Where(r => r.status == Robot.STATUS_ACTIVE)) {
                GUILayout.BeginVertical("Box"); //单个机器人
                GUILayout.Label(robot.name, GUILayout.ExpandWidth(true));
                if (selectedRobot != robot.id && GUILayout.Button("选择机器人", GUILayout.ExpandWidth(false))) {
                    selectedRobot = robot.id;
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
            if (selectedRobot > -1 && positionInWorld.Count > 0 && GUILayout.Button("开始导航")) {
                var temp = GameObject.Find($"Robot_{selectedRobot}");
                positionInWorld.Insert(0, new Vector2(temp.transform.position.x, temp.transform.position.z));
                var path = MDNavigationCenter.GetInstance().GetFinalPath(positionInWorld);
                var input = new object[path.Count + 2];
                input[0] = selectedRobot;
                input[1] = path.Count;
                for (var i = 2; i < path.Count + 2; i++) input[i] = path[i - 2];
                Events.Invoke(Events.M_ROBOT_NAVIGATION, input);
                foreach (var nav in navMarkList) {
                    Destroy(nav);
                }

                navMarkList.Clear();
            }

            GUILayout.EndArea();
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        private void CreateMask() {
            for (var m = 0; m < MTMapBuilder.MAP_CELL_ROW_CNT; m++) {
                for (var n = 0; n < MTMapBuilder.MAP_CELL_COLUMN_CNT; n++) {
                    var temp = Instantiate(blackMask,
                        transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
                    blackMasks[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n] = temp;
                    blackMasks[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n].name =
                        "blackMask_" + (m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n);
                    blackMasks[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n].GetComponent<RectTransform>().localPosition =
                        new Vector2(-230 + 5 + n * 10, -170 + 5 + m * 10);
                    blackMasks[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n].GetComponent<RectTransform>().sizeDelta =
                        new Vector2(10, 10);
                    blackMasks[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n].SetActive(false);
                    /*AddListener(
                        transform.Find("Canvas").Find("Naviga")
                            .Find("blackMask_" + (m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n))
                            .GetComponent<Button>(), m, n);*/
                }
            }
        }

        private void CreateBase() {
            base0 = Instantiate(base0Prefab,
                transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
            var mapPose = MTGlobalMapPoint.World2Map(BASE_POSITION[0].x, BASE_POSITION[0].y);
            base0.GetComponent<RectTransform>().localPosition =
                new Vector2(mapPose[0], mapPose[1]);
            base0.SetActive(false);

            base1 = Instantiate(base1Prefab,
                transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
            mapPose = MTGlobalMapPoint.World2Map(BASE_POSITION[1].x, BASE_POSITION[1].y);
            base1.GetComponent<RectTransform>().localPosition =
                new Vector2(mapPose[0], mapPose[1]);
            base1.SetActive(false);

            base2 = Instantiate(base2Prefab,
                transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
            mapPose = MTGlobalMapPoint.World2Map(BASE_POSITION[2].x, BASE_POSITION[2].y);
            base2.GetComponent<RectTransform>().localPosition =
                new Vector2(mapPose[0], mapPose[1]);
            base2.SetActive(false);

            base3 = Instantiate(base3Prefab,
                transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
            mapPose = MTGlobalMapPoint.World2Map(BASE_POSITION[3].x, BASE_POSITION[3].y);
            base3.GetComponent<RectTransform>().localPosition =
                new Vector2(mapPose[0], mapPose[1]);
            base3.SetActive(false);

            base4 = Instantiate(base4Prefab,
                transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
            mapPose = MTGlobalMapPoint.World2Map(BASE_POSITION[4].x, BASE_POSITION[4].y);
            base4.GetComponent<RectTransform>().localPosition =
                new Vector2(mapPose[0], mapPose[1]);
            base4.SetActive(false);

            base5 = Instantiate(base5Prefab,
                transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
            mapPose = MTGlobalMapPoint.World2Map(BASE_POSITION[5].x, BASE_POSITION[5].y);
            base5.GetComponent<RectTransform>().localPosition =
                new Vector2(mapPose[0], mapPose[1]);
            base5.SetActive(false);
        }

        private void CreateNavButton()
        {
            for (var m = 0; m < MTMapBuilder.MAP_CELL_ROW_CNT; m++) {
                for (var n = 0; n < MTMapBuilder.MAP_CELL_COLUMN_CNT; n++) {
                    var temp = Instantiate(navButtonPrefab,
                        transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
                    navButtons[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n] = temp;
                    navButtons[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n].name =
                        "navButtonPrefab_" + (m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n);
                    navButtons[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n].GetComponent<RectTransform>().localPosition =
                        new Vector2(-230 + 5 + n * 10, -170 + 5 + m * 10);
                    navButtons[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n].GetComponent<RectTransform>().sizeDelta =
                        new Vector2(10, 10);
                    navButtons[m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n].SetActive(false);
                    AddListener(
                        transform.Find("Canvas").Find("Naviga")
                            .Find("navButtonPrefab_" + (m * MTMapBuilder.MAP_CELL_COLUMN_CNT + n))
                            .GetComponent<Button>(), m, n);
                }
            }
        }

        private void TransformPosition(int m, int n) {
            var first = (float) (-62 + 62.0 / (17.0 * 2) + (33 - m) * 62.0 / 17.0);
            var second = (float) (-83 + 83.0 / (23.0 * 2) + n * 83.0 / 23.0);
            //屏幕坐标转化为世界坐标
            var temp = new Vector2(first, second);
            positionInWorld.Add(temp);
            if (selectedRobot != -1) {
                var navMark = Instantiate(navMarkPrefab,
                    transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
                navMarkList.Add(navMark);
                navMark.GetComponent<RectTransform>().localPosition = new Vector2(10 * n + 5 - 230, (10 * m + 5 - 170));
            }
        }

        private void AddListener(Button button, int m, int n) {
            button.onClick.AddListener(delegate { TransformPosition(m, n); });
        }
    }
}