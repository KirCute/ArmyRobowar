using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using UnityEngine;

namespace Map.Navigation {
    public class MDNavigationCenter : MonoBehaviour {
        private static MDNavigationCenter INSTANCE;
        public MDNavigationPoint[] points;

        public static MDNavigationCenter GetInstance() {
            return INSTANCE;
        }

        public void Awake() {
            INSTANCE = this;
            points = GetComponentsInChildren<MDNavigationPoint>();
        }

        private MDNavigationPoint GetBestNavigation(Vector3 givenPoint)
        {
            //找到离给定位置最近的一个导航点
            var bestNavigation = points[0];
            var minPathLength = Vector3.Distance(givenPoint, bestNavigation.transform.position);
            foreach (var point in points) {
                
                if (Vector3.Distance(givenPoint, point.transform.position) <= minPathLength) {
                    bestNavigation = point;
                    minPathLength = Vector3.Distance(givenPoint, bestNavigation.transform.position);
                }
            }

            return bestNavigation;
        }


        private List<Vector3> GetPathFromNavigations(MDNavigationPoint fromNavigation,MDNavigationPoint toNavigation)
        {
            List<Vector3> pathList = new List<Vector3>();
            
            //定义两个列表开启列表和关闭列表，分别表示待搜索与搜索过的导航点
            var openList = new List<MDNavigationPoint>() { fromNavigation };
            var closeList = new List<MDNavigationPoint>();
            
            while (openList.Any()) {
                
                //找到F值最小的点cur，将其加入到关闭列表中
                var cur = openList[0];
                foreach (var t in openList) {
                    if (t.F < cur.F || Math.Abs(t.F - cur.F) < 0.0001 && t.H < cur.H) {
                        cur = t;
                    }
                }
                closeList.Add(cur);
                openList.Remove(cur);
                
                //如果最佳点位最终导航点，进行路径回溯，存放到pathList中
                if (cur.Equals(toNavigation)) {
                    var currentPathTile = toNavigation;
                    while (!currentPathTile.Equals(fromNavigation)) {
                        pathList.Add(currentPathTile.transform.position);
                        currentPathTile = currentPathTile.parent;
                    }
                    return pathList;
                }
                
                //遍历寻找相邻节点
                foreach (var neighbor in cur.GetNeighbors().Where(t => !closeList.Contains(t))) {
                    var inSearch = openList.Contains(neighbor);
            
                    var costToNeighbor = cur.G + Vector3.Distance(cur.transform.position, neighbor.transform.position);
                    if (!inSearch || costToNeighbor < neighbor.G) {
                        neighbor.SetG(costToNeighbor);
                        neighbor.SetParent(cur);
                        
                        if (!inSearch) {
                            neighbor.SetH(Vector3.Distance(neighbor.transform.position, toNavigation.transform.position));
                            openList.Add(neighbor);
                        }
                    }
                }
            }
            return null;
        }

        public List<Vector3> GetMatch(Vector3 startPos, Vector3 endPos)
        {
            MDNavigationPoint fromNavigation = GetBestNavigation(startPos);
            MDNavigationPoint toNavigation = GetBestNavigation(endPos);

            return GetPathFromNavigations(fromNavigation, toNavigation);
        }
    }
}