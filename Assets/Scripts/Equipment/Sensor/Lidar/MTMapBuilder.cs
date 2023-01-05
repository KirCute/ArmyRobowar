using System;
using System.Collections.Generic;
using Photon.Pun;

namespace Equipment.Sensor.Lidar {
    /// <summary>
    /// 机器人到达一个新区域时，发送探索区域延伸事件，使团队地图扩展
    /// </summary>
    public class MTMapBuilder : MonoBehaviourPun {
        public const int MAP_PIXEL_WIDTH = 920;
        public const int MAP_PIXEL_HEIGHT = 680;
        public const int MAP_PIXEL_CELL_WIDTH = 20;
        public const int MAP_PIXEL_CELL_HEIGHT = 20;
        public const int MAP_CELL_COLUMN_CNT = 46;
        public const int MAP_CELL_ROW_CNT = 34;
        public const float MAP_WORLD_HALF_WIDTH = 82.0F;
        public const float MAP_WORLD_HALF_HEIGHT = 61.0F;
        public const float MAP_WORLD_CELL_WIDTH = MAP_PIXEL_CELL_WIDTH * MAP_WORLD_HALF_WIDTH * 2.0F / MAP_PIXEL_WIDTH;
        public const float MAP_WORLD_CELL_HEIGHT = MAP_PIXEL_CELL_HEIGHT * MAP_WORLD_HALF_HEIGHT * 2.0F / MAP_PIXEL_HEIGHT;

        private MEComponentIdentifier identity;
        public int scanLayer = 1;  // 扫描半径
        private readonly List<int> pointBuffer = new();  // 缓存失联期间扫到的区域，获得连接后一口气发出来
        private int lastPositionX = -1;
        private int lastPositionY = -1;  // 上次位置

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        private void Update() {
            if (photonView.IsMine) {
                var fCellX = (int) ((transform.position.z + MAP_WORLD_HALF_WIDTH) / MAP_WORLD_CELL_WIDTH + 0.5f);
                var fCellY = MAP_CELL_ROW_CNT - (int) ((transform.position.x + MAP_WORLD_HALF_HEIGHT) / MAP_WORLD_CELL_HEIGHT + 0.5f);
                if (fCellX != lastPositionX || fCellY != lastPositionY) {  // 到达新区域
                    for (var i = fCellX - scanLayer; i < fCellX + scanLayer; i++) {  // 对扫描正方形区域内的所有点
                        for (var j = fCellY - scanLayer; j < fCellY + scanLayer; j++) {
                            AddPoint(i, j);  // 在缓存中添加新区域
                        }
                    }

                    lastPositionX = fCellX;
                    lastPositionY = fCellY;
                }

                if (pointBuffer.Count > 0 &&
                    Summary.team.robots[identity.robotId].status == Model.Equipment.Robot.STATUS_ACTIVE) {  // 如果可以连接到信号，清空缓存
                    var args = new object[pointBuffer.Count + 2];
                    args[0] = identity.team;
                    args[1] = pointBuffer.Count;
                    for (var i = 0; i < pointBuffer.Count; i++) {
                        args[i + 2] = pointBuffer[i];
                    }

                    Events.Invoke(Events.F_ROBOT_LIDAR_SYNC, args);
                    pointBuffer.Clear();
                }
            }
        }

        /// <summary>
        /// 在扫描缓存中添加点
        /// 可以过滤无效点，自动将位置换算成索引
        /// </summary>
        /// <param name="x">地图横轴方向格数</param>
        /// <param name="y">地图纵轴方向格数</param>
        private void AddPoint(int x, int y) {
            if (y is < 0 or >= MAP_CELL_ROW_CNT || x is < 0 or >= MAP_CELL_COLUMN_CNT) return;
            if (Summary.team.teamMap[y * MAP_CELL_COLUMN_CNT + x]) return;
            pointBuffer.Add(y * MAP_CELL_COLUMN_CNT + x);
        }
    }
}