using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace UI {
    public class MTViewTechnologyTree : MonoBehaviourPunCallbacks {
        private const int VIEW_TECH_PAGE_ID = 0;
        private const float VIEW_TECH_PAGE_WIDTH = 0.8F;
        private const float VIEW_TECH_PAGE_HEIGHT = 0.7F;
        private const string VIEW_TECH_PAGE_TITLE = "科技树";
        private Vector2 scroll = Vector2.zero;
        private Texture2D armorImg;
        private Texture2D cameraImg;
        private Texture2D gunImg;
        private Texture2D lidarImg;
        private Texture2D inventoryImg;
        private Texture2D robotImg;
        private Texture2D towerImg;
        private Texture2D engineerImg;

        private void OnGUI() {
            var dim = new Rect(
                Screen.width * (1 - VIEW_TECH_PAGE_WIDTH) / 2, Screen.height * (1 - VIEW_TECH_PAGE_WIDTH) / 2,
                Screen.width * VIEW_TECH_PAGE_WIDTH, Screen.height * VIEW_TECH_PAGE_WIDTH
            );

            GUILayout.Window(VIEW_TECH_PAGE_ID, dim, _ => {
                GUILayout.Label($"当前科技点：{Summary.team.researchPoint:0.00}");
                scroll = GUILayout.BeginScrollView(scroll, false, false,
                    GUILayout.Height(Screen.height * VIEW_TECH_PAGE_HEIGHT));
                GUILayout.BeginVertical("Box");

                foreach (var tech in Constants.TECHNOLOGY.Keys.Where(tech =>
                             !Summary.team.achievedTechnics.Contains(tech))) {
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(GetImage(tech), GUILayout.ExpandWidth(false));
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label($"{Constants.TECHNOLOGY[tech].name} ({Constants.TECHNOLOGY[tech].cost:0.00})");
                    if (Constants.TECHNIC_TOPOLOGY[tech].Any(t => !Summary.team.achievedTechnics.Contains(t))) {
                        GUILayout.Label("需先学习前置科技", GUILayout.ExpandWidth(false));
                    } else if (Summary.team.researchPoint < Constants.TECHNOLOGY[tech].cost) {
                        GUILayout.Label("科技点不足", GUILayout.ExpandWidth(false));
                    } else if (GUILayout.Button("学习", GUILayout.ExpandWidth(false))) {
                        Events.Invoke(Events.M_TECHNOLOGY_RESEARCH,
                            new object[] {Summary.team.teamColor, tech}
                        );
                    }

                    GUILayout.EndHorizontal();
                    var styleTemp = new GUIStyle(GUI.skin.label) {
                        fontSize = 15,
                        normal = {
                            textColor = Color.yellow
                        }
                    };
                    GUILayout.Label(Constants.TECHNOLOGY[tech].description, styleTemp);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }

                foreach (var tech in Summary.team.achievedTechnics) {
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(GetImage(tech), GUILayout.ExpandWidth(false));
                    GUILayout.Label(Constants.TECHNOLOGY[tech].name);
                    GUILayout.Label("已学习", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            }, VIEW_TECH_PAGE_TITLE);
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) {
                enabled = false;
                GetComponent<MEMainCameraController>().active = true;
            }
        }

        private Texture2D GetImage(string tech) {
            if (Constants.ROBOT_TEMPLATES.ContainsKey(tech)) {
                return robotImg;
            }

            if (Constants.SENSOR_TEMPLATES.ContainsKey(tech)) {
                return Constants.SENSOR_TEMPLATES[tech].type switch {
                    0 => cameraImg,
                    1 => gunImg,
                    2 => lidarImg,
                    3 => inventoryImg,
                    4 => armorImg,
                    5 => engineerImg,
                    _ => null
                };
            }

            return towerImg;
        }

        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
            armorImg = Resources.Load("ShopImage/armor") as Texture2D;
            cameraImg = Resources.Load("ShopImage/camera") as Texture2D;
            gunImg = Resources.Load("ShopImage/gun") as Texture2D;
            inventoryImg = Resources.Load("ShopImage/inventory") as Texture2D;
            lidarImg = Resources.Load("ShopImage/lidar") as Texture2D;
            robotImg = Resources.Load("ShopImage/robot") as Texture2D;
            towerImg = Resources.Load("ShopImage/tower") as Texture2D;
            engineerImg = Resources.Load("ShopImage/engineer") as Texture2D;
        }

        public override void OnDisable() {
            base.OnDisable();
            Events.RemoveListener(Events.F_GAME_OVER, OnGameOver);
        }
    }
}