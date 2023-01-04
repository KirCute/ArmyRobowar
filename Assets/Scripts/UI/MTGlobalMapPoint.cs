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

        private const double K_X = 500.0 / 166.0;   //测试阶段调整
        private const double K_Y = 340.0 / 124.0;
        public static List<int> enemyList = new List<int>(); //包含所有检测到的敌车的ID
        private bool[] isCreatedF = new bool[1000];
        private bool[] isCreatedE = new bool[1000];
        public static Dictionary<int, GameObject> FPList = new Dictionary<int, GameObject>();
        public static Dictionary<int, GameObject> EPList = new Dictionary<int, GameObject>();

        private void OnEnable()
        {
            Events.AddListener(Events.F_ROBOT_SEIZE_ENEMY, AddEnemy);
            Events.AddListener(Events.F_ROBOT_LOST_SEIZE_ENEMY, DleteEnemy);
        }

        private void OnDisable()
        {
            Events.RemoveListener(Events.F_ROBOT_SEIZE_ENEMY, AddEnemy);
            Events.RemoveListener(Events.F_ROBOT_LOST_SEIZE_ENEMY, DleteEnemy);
        }

        private void Update()
        {
            if (Summary.isGameStarted)
            {
                FriendPoint();
                EnemyPoint();

            }

        }

        void AddEnemy(object[] args)
        {
            if (Summary.team.teamColor == (int)args[0])
            {
                enemyList.Add((int)args[1]);
            }
        }

        void DleteEnemy(object[] args)
        {
            if (Summary.team.teamColor == (int)args[0])
            {
                foreach (int enemy in enemyList)
                {
                    if (enemy == (int)args[1])
                    {
                        enemyList.Remove(enemy);
                        
                        //删除enemyList中的ID时同时删除该ID对应的point（GameObject）及EPList中的key
                        foreach (var ep in EPList)
                        {
                            if (ep.Key == enemy)
                            {
                                Destroy(ep.Value);
                            }
                        }
                        EPList.Remove(enemy);
                        isCreatedE[enemy] = false;
                    }
                }
            }
            
        }

        //在地图上标出友方车辆
        void FriendPoint()
        {
            for (int k = 0; k < Summary.team.robots.Count; k++)
            {
                if (Summary.team.robots[k] != null && Summary.team.robots[k].status == Robot.STATUS_ACTIVE) //己方有这辆车且未失联
                {
                    //如果未生成对应point则生成point
                    if (!isCreatedF[k])
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
                        isCreatedF[k] = true;
                    }
                    
                    //已创建后，有信号更新无信号删除
                    else if (isCreatedF[k])
                    {
                        if (Summary.team.robots[k].connection > 0)
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
                        else if (Summary.team.robots[k].connection <= 0)
                        {
                            foreach (var fp in FPList)
                            {
                                if (fp.Key == Summary.team.robots[k].id)
                                {
                                    
                                    Destroy(fp.Value);
                                    //删除对应point后置为false
                                    isCreatedF[k] = false;
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
                //未生成对应point则创建point
                if (!isCreatedE[enemy])
                {
                    int[] mapPose = World2Map(GameObject.Find($"Robot_{enemy}").transform.position.z,
                        GameObject.Find($"Robot_{enemy}").transform.position.x);

                    GameObject enemyPointGameObject = Instantiate(enemyPoint,
                        transform.Find("Canvas").Find("Naviga").gameObject.GetComponent<RectTransform>());

                    enemyPointGameObject.GetComponent<RectTransform>().localPosition =
                        new Vector2(mapPose[0], mapPose[1]);
                    enemyPointGameObject.SetActive(false);
                    EPList.Add(enemy,enemyPointGameObject);
                    isCreatedF[enemy] = true;
                }
                //已有对应point则更新位置
                else if(isCreatedF[enemy])
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
    }
}