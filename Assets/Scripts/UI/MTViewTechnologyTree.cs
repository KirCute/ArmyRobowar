using System;
using System.Linq;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

namespace UI {
    public class MTViewTechnologyTree : MonoBehaviourPunCallbacks {
        private const int VIEW_TECH_PAGE_ID = 0;
        private const float VIEW_TECH_PAGE_WIDTH = 0.8F;
        private const float VIEW_TECH_PAGE_HEIGHT = 0.7F;
        private const string VIEW_TECH_PAGE_TITLE = "";
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
                    GUILayout.Label(getImage(tech),GUILayout.ExpandWidth(false));
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
                    GUIStyle styleTemp = new GUIStyle(GUI.skin.label);
                    styleTemp.fontSize = 15;
                    styleTemp.normal.textColor = Color.yellow;
                    GUILayout.Label(Constants.TECHNOLOGY[tech].description, styleTemp);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    
                    GUILayout.EndVertical();
                }

                foreach (var tech in Summary.team.achievedTechnics) {
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(getImage(tech),GUILayout.ExpandWidth(false));
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

        private Texture2D getImage(string tech) {
            if (Constants.ROBOT_TEMPLATES.ContainsKey(tech)) {
                return robotImg;
            }else if (Constants.SENSOR_TEMPLATES.ContainsKey(tech)) {
                switch (Constants.SENSOR_TEMPLATES[tech].type) {
                    case 0:
                        return cameraImg;
                    case 1:
                        return gunImg;
                    case 2:
                        return lidarImg;
                    case 3:
                        return inventoryImg;
                    case 4:
                        return armorImg;
                    case 5:
                        return engineerImg;
                    default:
                        return null;
                }
            }else {
                return towerImg;
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
            armorImg = Resources.Load("image/armor") as Texture2D;
            cameraImg = Resources.Load("image/camera") as Texture2D;
            gunImg = Resources.Load("image/gun") as Texture2D;
            inventoryImg = Resources.Load("image/inventory") as Texture2D;
            lidarImg = Resources.Load("image/lidar") as Texture2D;
            robotImg = Resources.Load("image/robot") as Texture2D;
            towerImg = Resources.Load("image/tower") as Texture2D;
            engineerImg = Resources.Load("image/engineer") as Texture2D;
            /*armorImg = ResizeTexture(armorImg,32,32);
            cameraImg = ResizeTexture(cameraImg,32,32);
            gunImg = ResizeTexture(gunImg,32,32);
            inventoryImg = ResizeTexture(inventoryImg,32,32);
            lidarImg = ResizeTexture(lidarImg,32,32);
            robotImg = ResizeTexture(robotImg,32,32);
            towerImg = ResizeTexture(towerImg,32,32);
            engineerImg = ResizeTexture(engineerImg,32,32);*/
            
           /*
           // Debug.Log(engineerImg.isReadable);
            armorImg.Reinitialize(32, 32);
            cameraImg.Reinitialize(32, 32);
            gunImg.Reinitialize(32, 32);
            inventoryImg.Reinitialize(32, 32);
            lidarImg.Reinitialize(32, 32);
            robotImg.Reinitialize(32, 32);
            towerImg.Reinitialize(64, 64);
            engineerImg.Reinitialize(32, 32);*/
        }
        /*private static Texture2D ResizeTexture(Texture2D source, int width, int height)
        {
            if (source != null)
            {
                // 创建临时的RenderTexture
                RenderTexture renderTex = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
                // 复制source的纹理到RenderTexture里
                Graphics.Blit(source, renderTex);
                // 开启当前RenderTexture激活状态
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                // 创建修改后的纹理，保持与源纹理相同压缩格式
                Texture2D resizedTexture = new Texture2D(width, height, source.format, false);
                // 读取像素到创建的纹理中
                resizedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                // 应用修改到GPU上
                resizedTexture.Apply();
                // 停止当前RenderTexture工作
                RenderTexture.active = previous;
                // 释放内存
                RenderTexture.ReleaseTemporary(renderTex);
                return resizedTexture;
            }
            else
            {
                return null;
            }
        }*/

        public override void OnDisable() {
            base.OnDisable();
            Events.RemoveListener(Events.F_GAME_OVER, OnGameOver);
        }
        /*private Texture2D duplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }*/
    }
}