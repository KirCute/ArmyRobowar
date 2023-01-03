using System.Collections.Generic;
using Map.Navigation;
using Photon.Pun;
using UnityEngine;

namespace UI {
    public class MTViewAutoNavigation : MonoBehaviourPunCallbacks {
        private const int SKETCH_MAP_WIDTH = 920;
        private const int SKETCH_MAP_HEIGHT = 680;
        private readonly List<Vector2> positionInSketchMap = new();
        private readonly List<Vector2> positionInWorld = new();
        private int selectedRobotId;

        private void OnGUI() {
            if (GetComponent<MTSelectNavigationRobot>().setNavigationPath) {
                selectedRobotId = GetComponent<MTSelectNavigationRobot>().selectedRobotId;
                var mapRect = new Rect((Screen.width - SKETCH_MAP_WIDTH) / 2, (Screen.height - SKETCH_MAP_HEIGHT) / 2,
                    SKETCH_MAP_WIDTH, SKETCH_MAP_HEIGHT);
                Texture map = Resources.Load<Texture>("map");
                //TODO,换成小地图
                GUI.Label(mapRect, map);

                this.enabled = true;
                if (Input.GetMouseButtonDown(0)) {
                    Vector2 mousePosition = new Vector2(
                        (Input.mousePosition.x - (Screen.width - SKETCH_MAP_WIDTH) / 2) / 20,
                        (SKETCH_MAP_HEIGHT - (Input.mousePosition.y - (Screen.height - SKETCH_MAP_HEIGHT) / 2)) / 20);
                    positionInSketchMap.Add(mousePosition);
                }

                if (GUI.Button(new Rect(Screen.width - 10, Screen.height - 10, 10, 10), "开始路径规划")) {
                    //TODO,画线
                    GameObject robot = GameObject.Find($"Robot_{selectedRobotId}");
                    Vector2 robotPosition = new Vector2(robot.transform.position.x, robot.transform.position.z);
                    positionInWorld.Add(robotPosition);

                    //从屏幕坐标转化为世界坐标
                    for (int i = 0; i < positionInSketchMap.Count; i++) {
                        Vector2 position = new Vector2((positionInSketchMap[i].x - 23) / 23 * 61,
                            (positionInSketchMap[i].y - 17) / 17 * 82);
                        positionInWorld.Add(position);
                    }

                    List<Vector2> path = MDNavigationCenter.GetInstance().GetFinalPath(positionInWorld);
                    var input = new object[path.Count + 2];
                    input[0] = selectedRobotId;
                    input[1] = path.Count;
                    for (var i = 2; i < path.Count + 2; i++) input[i] = path[i - 2];
                    Events.Invoke(Events.M_ROBOT_NAVIGATION, input);
                }
            }
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
    }
}