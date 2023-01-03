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
        
        private const double K_X = 164 / 640;
        private const double K_Y = 122 / 340;
        private List<int> enemyList;    //包含所有检测到的敌车的ID
       
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
                    }
                }
            }
        }
        
        //在地图上标出友方车辆
        void FriendPoint()
        {
            for (int k = 0; k < Summary.team.robots.Count; k++)
            {
                if (Summary.team.robots[k] != null && Summary.team.robots[k].connection >= 0)   //己方有这辆车且未失联
                {
                    
                    int[] mapPose = World2Map(GameObject.Find($"Robot_{Summary.team.robots[k].id}").transform.position.z,
                        GameObject.Find($"Robot_{Summary.team.robots[k].id}").transform.position.x);
                    
                    GameObject friendPointGameObject = Instantiate(friendPoint, GetComponent<RectTransform>());
                    
                    friendPointGameObject.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(mapPose[0], mapPose[1]);
                    friendPointGameObject.SetActive(true);
                }
            }
        }
        
        //世界坐标转换到地图坐标
        public static int[] World2Map(double worldZ,double worldX)
        {
            int[] pose = new int[2];
            pose[0] = (int)(worldZ / K_X);
            pose[1] = (int)(-(worldX) / K_Y);
            return pose;
        }

        //在地图上标出敌方方车辆
        void EnemyPoint()
        {
            foreach (int enemy in enemyList)
            {
                int[] mapPose = World2Map(GameObject.Find($"Robot_{enemy}").transform.position.z,
                    GameObject.Find($"Robot_{enemy}").transform.position.x);
                
                GameObject enemyPointGameObject = Instantiate(enemyPoint, GetComponent<RectTransform>());
                
                enemyPointGameObject.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(mapPose[0], mapPose[1]);
                enemyPointGameObject.SetActive(true);
            }
        }
    }
}