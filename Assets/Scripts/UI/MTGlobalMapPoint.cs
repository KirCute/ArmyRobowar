using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MTGlobalMapPoint : MonoBehaviour
    {
        public GameObject friendPoint;
        public GameObject enemyPoint;
        public GameObject towerPoint;
        
        private const double K_X = 500.0 / 166.0;   //map像素长度与世界场景长度之比
        private const double K_Y = 340.0 / 124.0;   //map像素宽度与世界场景宽度之比
        public  List<int> enemyList = new List<int>(); //存放所有检测到的敌车的ID
        private HashSet<int> isCreatedF = new();    //友方车辆是否已在map上标出
        private HashSet<int> isCreatedE = new();    //敌方车辆是否已在map上标出
        private HashSet<int> isCreatedT = new();    //友方信号塔是否已在map上标出
        public Dictionary<int, GameObject> FPList = new Dictionary<int, GameObject>();
        public Dictionary<int, GameObject> EPList = new Dictionary<int, GameObject>();
        public Dictionary<int, GameObject> TPList = new Dictionary<int, GameObject>();

        private void OnEnable()
        {
            Events.AddListener(Events.F_ROBOT_SEIZE_ENEMY, AddEnemy);
            Events.AddListener(Events.F_ROBOT_LOST_SEIZE_ENEMY, DleteEnemy);
            Events.AddListener(Events.F_BODY_DESTROYED, DleteDestroyedEnemy);
            Events.AddListener(Events.F_TOWER_DESTROYED, DleteDestroyedTower);
        }

        private void OnDisable()
        {
            Events.RemoveListener(Events.F_ROBOT_SEIZE_ENEMY, AddEnemy);
            Events.RemoveListener(Events.F_ROBOT_LOST_SEIZE_ENEMY, DleteEnemy);
            Events.RemoveListener(Events.F_BODY_DESTROYED, DleteDestroyedEnemy);
            Events.RemoveListener(Events.F_TOWER_DESTROYED, DleteDestroyedTower);
        }

        private void Update()
        {
            if (Summary.isGameStarted)
            {
                FriendPoint();
                EnemyPoint();
                TowerPoint();
            }

        }

        void AddEnemy(object[] args)
        {
            if (Summary.team.teamColor == (int)args[0])
            {
                Debug.Log("666");
                Debug.Log((int)args[1]);
                enemyList.Add((int)args[1]);
            }
        }

        void DleteEnemy(object[] args)
        {
            if (Summary.team.teamColor == (int)args[0])
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    int enemy = enemyList[i];
                    if (enemy == (int)args[1])
                    {
                        enemyList.RemoveAt(i--);
                        
                        //删除enemyList中的ID时同时删除该ID对应的point（GameObject）及EPList中的key
                        foreach (var ep in EPList)
                        {
                            if (ep.Key == enemy)
                            {
                                Destroy(ep.Value);
                            }
                        }
                        EPList.Remove(enemy);
                        isCreatedE.Remove(enemy);
                    }
                }
            }
        }

        void DleteDestroyedEnemy(object[] args)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                int enemy = enemyList[i];
                if (enemy == (int)args[0])
                {
                    enemyList.RemoveAt(i--);
                        
                    //删除enemyList中的ID时同时删除该ID对应的point（GameObject）及EPList中的key
                    foreach (var ep in EPList)
                    {
                        if (ep.Key == enemy)
                        {
                            Destroy(ep.Value);
                        }
                    }
                    EPList.Remove(enemy);
                    isCreatedE.Remove(enemy);
                }
            }
        }

        void DleteDestroyedTower(object[] args)
        {
            foreach (var tp in TPList)
            {
                if (tp.Key == (int)args[0])
                {
                    Destroy(tp.Value);
                }
            }
            TPList.Remove((int)args[0]);
        }

        //在地图上标出友方车辆
        void FriendPoint()
        {
            foreach (var k in Summary.team.robots.Keys)
            {
                if (Summary.team.robots[k] != null && Summary.team.robots[k].status != Robot.STATUS_MANUFACTURING) //己方有这辆车且未失联
                {
                    //如果未生成对应point则生成point
                    if (!isCreatedF.Contains(k) && Summary.team.robots[k].connection > 1)
                    {
                        int[] mapPose = World2Map(
                            GameObject.Find($"Robot_{Summary.team.robots[k].id}").transform.position.z,
                            GameObject.Find($"Robot_{Summary.team.robots[k].id}").transform.position.x);


                        GameObject friendPointGameObject = Instantiate(friendPoint,
                            transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());

                        friendPointGameObject.GetComponent<RectTransform>().localPosition =
                            new Vector2(mapPose[0], mapPose[1]);

                        friendPointGameObject.SetActive(false);
                        
                        FPList.Add(Summary.team.robots[k].id, friendPointGameObject);
                        //对应置为true
                        isCreatedF.Add(k);
                    }
                    
                    //已创建后，有信号更新无信号删除
                    else if (isCreatedF.Contains(k))
                    {
                        if (Summary.team.robots[k].connection > 1)
                        {
                            foreach (var fp in FPList)
                            {
                                if (fp.Key == Summary.team.robots[k].id)
                                {
                                    int[] mapPose =
                                        World2Map(
                                            GameObject.Find($"Robot_{Summary.team.robots[k].id}").transform.position.z,
                                            GameObject.Find($"Robot_{Summary.team.robots[k].id}").transform.position.x);
                                    fp.Value.GetComponent<RectTransform>().localPosition =
                                        new Vector2(mapPose[0], mapPose[1]);
                                }
                            }
                        }
                        else if (Summary.team.robots[k].connection <= 1)
                        {
                            foreach (var fp in FPList)
                            {
                                if (fp.Key == Summary.team.robots[k].id)
                                {
                                    Destroy(fp.Value);
                                    //删除对应point后置为false
                                    isCreatedF.Remove(k);
                                }
                            }
                            FPList.Remove(Summary.team.robots[k].id);
                        }
                    }
                }
            }
        }

        //世界坐标转换到地图坐标
        public static int[] World2Map(double worldZ, double worldX)
        {
            int[] pose = new int[2];
            pose[0] = (int)(worldZ * K_X);
            pose[1] = (int)(-(worldX) * K_Y);
            return pose;
        }

        //在地图上标出敌方方车辆
        void EnemyPoint()
        {
            foreach (int enemy in enemyList)
            {
                Debug.Log(enemy);
                //未生成对应point则创建point
                if (!isCreatedE.Contains(enemy))
                {
                    Debug.Log("ok");
                    int[] mapPose = World2Map(GameObject.Find($"Robot_{enemy}").transform.position.z,
                        GameObject.Find($"Robot_{enemy}").transform.position.x);

                    GameObject enemyPointGameObject = Instantiate(enemyPoint,
                        transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());

                    enemyPointGameObject.GetComponent<RectTransform>().localPosition =
                        new Vector2(mapPose[0], mapPose[1]);
                    enemyPointGameObject.SetActive(false);
                    EPList.Add(enemy,enemyPointGameObject);
                    isCreatedE.Add(enemy);
                }
                //已有对应point则更新位置
                else if(isCreatedF.Contains(enemy))
                {
                    foreach (var ep in EPList)
                    {
                        int[] mapPose = World2Map(GameObject.Find($"Robot_{enemy}").transform.position.z,
                            GameObject.Find($"Robot_{enemy}").transform.position.x);
                        ep.Value.GetComponent<RectTransform>().localPosition =
                            new Vector2(mapPose[0], mapPose[1]);
                    }
                }
            }
        }

        void TowerPoint()
        {
            foreach (var k in Summary.team.towers.Keys)
            {
                if (Summary.team.towers[k] != null)
                {
                    if (!isCreatedT.Contains(k))
                    {
                        int[] mapPose = World2Map(
                            GameObject.Find($"Tower_{k}").transform.position.z,
                            GameObject.Find($"Tower_{k}").transform.position.x);
                        
                        GameObject towerPointGameObject = Instantiate(towerPoint,
                            transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());

                        towerPointGameObject.GetComponent<RectTransform>().localPosition =
                            new Vector2(mapPose[0], mapPose[1]);

                        towerPointGameObject.SetActive(false);
                        
                        TPList.Add(k, towerPointGameObject);
                        //对应置为true
                        isCreatedT.Add(k);
                    }
                }
            }
        }
    }
}