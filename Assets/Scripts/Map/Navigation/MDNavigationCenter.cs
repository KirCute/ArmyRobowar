using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Map.Navigation {
    public class MDNavigationCenter : MonoBehaviour {
        private static MDNavigationCenter instance;
        public MDNavigationPoint[] points;
        private readonly Dictionary<MDNavigationPoint, float> gDic = new();
        private readonly Dictionary<MDNavigationPoint, float> hDic = new();
        private readonly Dictionary<MDNavigationPoint, MDNavigationPoint> parentDic = new();

        public static MDNavigationCenter GetInstance() {
            return instance;
        }

        public void Awake() {
            instance = this;
            points = GetComponentsInChildren<MDNavigationPoint>();
            foreach (var point in points) {
                gDic.Add(point, 0f);
                hDic.Add(point, 0f);
                parentDic.Add(point, null);
            }
        }

        private MDNavigationPoint GetBestNavigation(Vector3 givenPoint) {
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


        private List<Vector3> GetPathFromNavigations(MDNavigationPoint from, MDNavigationPoint to) {
            var pathList = new List<Vector3>();

            //定义两个列表开启列表和关闭列表，分别表示待搜索与搜索过的导航点
            var openList = new List<MDNavigationPoint> {from};
            var closeList = new List<MDNavigationPoint>();

            gDic[from] = 0;
            hDic[from] = GetDistance(from, to);
            while (openList.Count > 0) {
                //找到F值最小的点cur，将其加入到关闭列表中
                var cur = openList[0];
                foreach (var t in from t in openList
                         where GetF(t) < GetF(cur) || Math.Abs(GetF(t) - GetF(cur)) < 0.0001 && hDic[t] < hDic[cur]
                         select t) {
                    cur = t;
                }

                closeList.Add(cur);
                openList.Remove(cur);

                //如果最佳点位最终导航点，进行路径回溯，存放到pathList中
                if (cur.Equals(to)) {
                    var currentPathTile = to;
                    while (!currentPathTile.Equals(from)) {
                        pathList.Add(currentPathTile.transform.position);
                        currentPathTile = parentDic[currentPathTile];
                    }
                    pathList.Add(from.transform.position);
                    
                    pathList.Reverse();
                    return pathList;
                    
                }

                //遍历寻找相邻节点
                foreach (var neighbor in cur.GetNeighbors().Where(t => !closeList.Contains(t))) {
                    var inSearch = openList.Contains(neighbor);

                    var costToNeighbor = gDic[cur] + GetDistance(cur, neighbor);
                    
                    if (!inSearch || costToNeighbor < gDic[neighbor]) {
                        gDic[neighbor] = costToNeighbor;
                        parentDic[neighbor] = cur;

                        if (!inSearch) {
                            hDic[neighbor] = GetDistance(neighbor, to);
                            openList.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        private float GetF(MDNavigationPoint point) {
            return gDic[point] + hDic[point];
        }

        public List<Vector3> GetMatch(Vector3 startPos, Vector3 endPos) {
            var fromNavigation = GetBestNavigation(startPos);
            var toNavigation = GetBestNavigation(endPos);

            return GetPathFromNavigations(fromNavigation, toNavigation);
        }

        private static float GetDistance(MDNavigationPoint a, MDNavigationPoint b) {
            return Vector3.Distance(a.transform.position, b.transform.position);
        }
    }
}