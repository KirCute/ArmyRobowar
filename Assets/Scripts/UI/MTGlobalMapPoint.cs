using System;
using System.Collections.Generic;
using System.Linq;
using Model.Equipment;
using UnityEngine;

namespace UI {
    public class MTGlobalMapPoint : MonoBehaviour {
        public GameObject friendPoint;
        public GameObject enemyPoint;
        public GameObject towerPoint;

        private const double K_X = 750.0 / 166.0; //map像素长度与世界场景长度之比
        private const double K_Y = 510.0 / 124.0; //map像素宽度与世界场景宽度之比
        public List<int> enemyList = new(); //存放所有检测到的敌车的ID
        private readonly HashSet<int> createdFriends = new(); //友方车辆是否已在map上标出
        private readonly HashSet<int> createdEnemies = new(); //敌方车辆是否已在map上标出
        private readonly HashSet<int> createdTowers = new(); //友方信号塔是否已在map上标出
        public readonly Dictionary<int, GameObject> friendPointList = new();
        public readonly Dictionary<int, GameObject> enemyPointList = new();
        public readonly Dictionary<int, GameObject> towerPointList = new();

        private void OnEnable() {
            Events.AddListener(Events.F_ROBOT_SEIZE_ENEMY, AddEnemy);
            Events.AddListener(Events.F_ROBOT_LOST_SEIZE_ENEMY, DeleteEnemy);
            Events.AddListener(Events.F_BODY_DESTROYED, DeleteDestroyedEnemy);
            Events.AddListener(Events.F_TOWER_DESTROYED, DeleteDestroyedTower);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_ROBOT_SEIZE_ENEMY, AddEnemy);
            Events.RemoveListener(Events.F_ROBOT_LOST_SEIZE_ENEMY, DeleteEnemy);
            Events.RemoveListener(Events.F_BODY_DESTROYED, DeleteDestroyedEnemy);
            Events.RemoveListener(Events.F_TOWER_DESTROYED, DeleteDestroyedTower);
        }

        private void Update() {
            if (Summary.isGameStarted) {
                UpdateFriendPoints();
                UpdateEnemyPoints();
                UpdateTowerPoints();
            }
        }

        /// <summary>
        /// 索敌后记录敌人ID
        /// </summary>
        /// <param name="args"></param>
        private void AddEnemy(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                enemyList.Add((int) args[1]);
            }
        }

        /// <summary>
        /// 失去索敌后删除敌人ID
        /// </summary>
        /// <param name="args"></param>
        private void DeleteEnemy(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                for (var i = 0; i < enemyList.Count; i++) {
                    var enemy = enemyList[i];
                    if (enemy == (int) args[1]) {
                        enemyList.RemoveAt(i--);

                        //删除enemyList中的ID时同时删除该ID对应的point（GameObject）及EPList中的key
                        foreach (var ep in enemyPointList.Where(ep => ep.Key == enemy)) {
                            Destroy(ep.Value);
                        }

                        enemyPointList.Remove(enemy);
                        createdEnemies.Remove(enemy);
                    }
                }
            }
        }

        /// <summary>
        /// 敌人死亡后删去敌人ID
        /// </summary>
        /// <param name="args"></param>
        private void DeleteDestroyedEnemy(object[] args) {
            for (var i = 0; i < enemyList.Count; i++) {
                var enemy = enemyList[i];
                if (enemy == (int) args[0]) {
                    enemyList.RemoveAt(i--);
                    //删除enemyList中的ID时同时删除该ID对应的point（GameObject）及EPList中的key
                    foreach (var ep in enemyPointList.Where(ep => ep.Key == enemy)) {
                        Destroy(ep.Value);
                    }

                    enemyPointList.Remove(enemy);
                    createdEnemies.Remove(enemy);
                }
            }
        }

        /// <summary>
        /// 信号塔被摧毁后销毁对应标记
        /// </summary>
        /// <param name="args"></param>
        private void DeleteDestroyedTower(object[] args) {
            foreach (var tp in towerPointList.Where(tp => tp.Key == (int) args[0])) {
                Destroy(tp.Value);
            }

            towerPointList.Remove((int) args[0]);
        }

        /// <summary>
        /// 在地图上标出友方车辆
        /// </summary>
        private void UpdateFriendPoints() {
            foreach (var k in Summary.team.robots.Keys.Where(k =>
                         Summary.team.robots[k] != null &&
                         Summary.team.robots[k].status != Robot.STATUS_MANUFACTURING)) {
                //如果未生成对应point则生成point
                if (!createdFriends.Contains(k) && Summary.team.robots[k].connection > 1) {
                    var robotPosition = GameObject.Find($"Robot_{Summary.team.robots[k].id}").transform.position;
                    var mapPose = World2Map(robotPosition.z, robotPosition.x);
                    var friendPointGameObject = Instantiate(friendPoint,
                        transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
                    friendPointGameObject.GetComponent<RectTransform>().localPosition =
                        new Vector2(mapPose[0], mapPose[1]);
                    friendPointGameObject.SetActive(false);
                    friendPointList.Add(Summary.team.robots[k].id, friendPointGameObject);
                    //对应置为true
                    createdFriends.Add(k);
                } else if (createdFriends.Contains(k)) {  //已创建后，有信号更新无信号删除
                    switch (Summary.team.robots[k].connection) {
                        case > 0: {
                            foreach (var (key, value) in friendPointList) {
                                if (key == Summary.team.robots[k].id) {
                                    var robotTransform = GameObject.Find($"Robot_{Summary.team.robots[k].id}").transform;
                                    var mapPose = World2Map(robotTransform.position.z, robotTransform.position.x);
                                    value.GetComponent<RectTransform>().localPosition =
                                        new Vector2(mapPose[0], mapPose[1]);
                                }
                            }

                            break;
                        }
                        case <= 0: {
                            foreach (var fp in friendPointList.Where(fp => fp.Key == Summary.team.robots[k].id)) {
                                Destroy(fp.Value);
                                //删除对应point后置为false
                                createdFriends.Remove(k);
                            }

                            friendPointList.Remove(Summary.team.robots[k].id);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 世界坐标转换到地图坐标
        /// </summary>
        /// <param name="worldZ">世界坐标z</param>
        /// <param name="worldX">世界坐标x</param>
        /// <returns>map坐标</returns>
        public static int[] World2Map(double worldZ, double worldX) {
            var pose = new int[2];
            pose[0] = (int) (worldZ * K_X);
            pose[1] = (int) (-worldX * K_Y);
            return pose;
        }

        /// <summary>
        /// 在地图上标出敌方方车辆
        /// </summary>
        private void UpdateEnemyPoints() {
            foreach (var enemy in enemyList) {
                //未生成对应point则创建point
                if (!createdEnemies.Contains(enemy)) {
                    var robotPosition = GameObject.Find($"Robot_{enemy}").transform.position;
                    var mapPose = World2Map(robotPosition.z, robotPosition.x);
                    var enemyPointGameObject = Instantiate(enemyPoint,
                        transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
                    enemyPointGameObject.GetComponent<RectTransform>().localPosition =
                        new Vector2(mapPose[0], mapPose[1]);
                    enemyPointGameObject.SetActive(false);
                    enemyPointList.Add(enemy, enemyPointGameObject);
                    createdEnemies.Add(enemy);
                } else if (createdFriends.Contains(enemy)) {  //已有对应point则更新位置
                    foreach (var ep in enemyPointList) {
                        var mapPose = World2Map(GameObject.Find($"Robot_{enemy}").transform.position.z,
                            GameObject.Find($"Robot_{enemy}").transform.position.x);
                        ep.Value.GetComponent<RectTransform>().localPosition =
                            new Vector2(mapPose[0], mapPose[1]);
                    }
                }
            }
        }

        /// <summary>
        /// 对于新Tower在地图上建立标记
        /// </summary>
        private void UpdateTowerPoints() {
            foreach (var k in Summary.team.towers.Keys) {
                if (Summary.team.towers[k] != null) {
                    if (!createdTowers.Contains(k)) {
                        var towerPosition = GameObject.Find($"Tower_{k}").transform.position;
                        var mapPose = World2Map(towerPosition.z, towerPosition.x);
                        var towerPointGameObject = Instantiate(towerPoint,
                            transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());
                        towerPointGameObject.GetComponent<RectTransform>().localPosition =
                            new Vector2(mapPose[0], mapPose[1]);
                        towerPointGameObject.SetActive(false);
                        towerPointList.Add(k, towerPointGameObject);
                        //对应置为true
                        createdTowers.Add(k);
                    }
                }
            }
        }
    }
}