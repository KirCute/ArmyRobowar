using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.Navigation {
    /// <summary>
    /// 单例获取导航路径
    /// </summary>
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
        /// <summary>
        /// 获得离给定点最近的导航点
        /// </summary>
        /// <param name="givenPoint">给定的任意点</param>
        /// <returns>返回最近导航点</returns>
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

        /// <summary>
        /// A*算法
        /// </summary>
        /// <param name="from">开始导航点</param>
        /// <param name="to">目标导航点</param>
        /// <returns>返回最短路径</returns>
        private List<Vector2> GetPathFromNavigations(MDNavigationPoint from, MDNavigationPoint to) {
            var pathList = new List<Vector2>();

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
                    Vector3 pos;
                    while (!currentPathTile.Equals(from)) {
                        pos = currentPathTile.transform.position;
                        pathList.Add(new Vector2(pos.x, pos.z));
                        currentPathTile = parentDic[currentPathTile];
                    }

                    pos = from.transform.position;
                    pathList.Add(new Vector2(pos.x, pos.z));
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
        /// <summary>
        /// 获取F值
        /// </summary>
        /// <param name="point">导航点参数</param>
        /// <returns>返回路径F值</returns>
        private float GetF(MDNavigationPoint point) {
            //获得路径F值
            return gDic[point] + hDic[point];
        }
        /// <summary>
        /// 最终最短路径列表
        /// </summary>
        /// <param name="posList">给定任意点列表</param>
        /// <returns>返回最终最短路径列表</returns>
        public List<Vector2> GetFinalPath(IEnumerable<Vector2> posList) {
            return GetFinalPath(posList.Select(point => new Vector3(point.x, 0f, point.y)));
        }

        /// <summary>
        /// 最终最短路径列表
        /// </summary>
        /// <param name="posList">给定任意点列表</param>
        /// <returns>返回最终最短路径列表</returns>
        private List<Vector2> GetFinalPath(IEnumerable<Vector3> posList) {
            //定义导航点列表
            var navigationList = posList.Select(GetBestNavigation).ToList();

            //先获得第一条路径
            var from = navigationList[0];
            var to = navigationList[1];

            var finalList = GetPathFromNavigations(from, to);

            for (var i = 1; i < navigationList.Count - 1; i++) {
                from = navigationList[i];
                to = navigationList[i + 1];
                //跳过路径的第一个导航点
                finalList.AddRange(GetPathFromNavigations(from, to).Skip(1).ToList());
            }

            return finalList;
        }
        
        /// <summary>
        /// 两个导航点的欧氏距离
        /// </summary>
        /// <param name="a">第一个导航点参数</param>
        /// <param name="b">第二个导航点参数</param>
        /// <returns></returns>
        private static float GetDistance(MDNavigationPoint a, MDNavigationPoint b) {
            return Vector3.Distance(a.transform.position, b.transform.position);
        }
    }
}