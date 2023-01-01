using System;
using System.Collections.Generic;
using Photon.Pun;

namespace Equipment.Sensor.Lidar {
    public class MTMapBuilder : MonoBehaviourPun {
        private const int MAP_PIXEL_WIDTH = 920;
        private const int MAP_PIXEL_HEIGHT = 680;
        private const int MAP_PIXEL_CELL_WIDTH = 20;
        private const int MAP_PIXEL_CELL_HEIGHT = 20;
        private const int MAP_CELL_COLUMN_CNT = 46;
        private const int MAP_CELL_ROW_CNT = 34;
        private const float MAP_WORLD_HALF_WIDTH = 82.0F;
        private const float MAP_WORLD_HALF_HEIGHT = 61.0F;
        private const float MAP_WORLD_CELL_WIDTH = MAP_PIXEL_CELL_WIDTH * MAP_WORLD_HALF_WIDTH * 2.0F / MAP_PIXEL_WIDTH;

        private const float MAP_WORLD_CELL_HEIGHT =
            MAP_PIXEL_CELL_HEIGHT * MAP_WORLD_HALF_HEIGHT * 2.0F / MAP_PIXEL_HEIGHT;

        private MEComponentIdentifier identity;
        public int scanLayer = 1;
        private readonly List<int> pointBuffer = new();
        private int lastPositionX = -1;
        private int lastPositionY = -1;

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        private void Update() {
            if (photonView.IsMine) {
                var fCellX = (int) ((transform.position.z + MAP_WORLD_CELL_WIDTH) / MAP_WORLD_CELL_WIDTH + 0.5f);
                var fCellY = (int) ((transform.position.x + MAP_WORLD_CELL_HEIGHT) / MAP_WORLD_CELL_HEIGHT + 0.5f);
                if (fCellX != lastPositionX || fCellY != lastPositionY) {
                    for (var i = fCellX - scanLayer; i < fCellX + scanLayer; i++) {
                        for (var j = fCellY - scanLayer; j < fCellY + scanLayer; j++) {
                            AddPoint(i, j);
                        }
                    }

                    lastPositionX = fCellX;
                    lastPositionY = fCellY;
                }

                if (pointBuffer.Count > 0 &&
                    Summary.team.robots[identity.robotId].status == Model.Equipment.Robot.STATUS_ACTIVE) {
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

        private void AddPoint(int x, int y) {
            if (y is < 0 or >= MAP_CELL_ROW_CNT || x is < 0 or >= MAP_CELL_COLUMN_CNT) return;
            if (Summary.team.teamMap[y * MAP_CELL_COLUMN_CNT + x]) return;
            pointBuffer.Add(y * MAP_CELL_COLUMN_CNT + x);
        }
    }
}