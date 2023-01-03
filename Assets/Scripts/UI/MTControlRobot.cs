using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace UI {
    public class MTControlRobot : MonoBehaviour {
        private const float SENSITIVITY = 7.5f;
        private const string FULL_INVENTORY_NOTIFY = "物品栏已满，无法捡拾新物品";
        private const string LACK_MONEY_TO_CAPTURE_NOTIFY = "货币不足，无法占领基地，请保留至少100货币";
        private const string LACK_MONEY_TO_BUILD_TOWER_NOTIFY = "货币不足，无法搭建信号塔，请保留至少20货币";
        private const string CONNECTION_WEAK_NOTIFY = "即将离开信号覆盖区";
        private const string BUILD_FAILED_NOTIFY = "未装备工程配件，无法进行建造活动";

        public MEErrorBroadcaster broadcaster;
        private int controllingRobot { get; set; } = -1;
        private Vector2 lastMotivation = Vector2.zero;
        private bool lockCamera;

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_CONTROL, OnControlled);
            Events.AddListener(Events.M_ROBOT_MONITOR, CheckMonitor);
            Events.AddListener(Events.F_ROBOT_WEAK_CONNECTION, OnWeakConnection);
            Events.AddListener(Events.F_ROBOT_STRONG_CONNECTION, OnConnectionRepair);
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_CONTROL, OnControlled);
            Events.RemoveListener(Events.M_ROBOT_MONITOR, CheckMonitor);
            Events.RemoveListener(Events.F_ROBOT_WEAK_CONNECTION, OnWeakConnection);
            Events.RemoveListener(Events.F_ROBOT_STRONG_CONNECTION, OnConnectionRepair);
        }

        private void OnControlled(object[] args) {
            if (controllingRobot == (int) args[0] && args[1] == null) {
                Cursor.lockState = CursorLockMode.None;
                controllingRobot = -1;
                broadcaster.RemoveLongTermMessage(CONNECTION_WEAK_NOTIFY);
            } else if (PhotonNetwork.LocalPlayer.Equals((Player) args[1])) {
                Cursor.lockState = CursorLockMode.Locked;
                controllingRobot = (int) args[0];
            }
        }

        private void CheckMonitor(object[] args) {
            if (controllingRobot == (int) args[0] && PhotonNetwork.LocalPlayer.Equals((Player) args[1]) &&
                !(bool) args[2]) {
                Events.Invoke(Events.M_ROBOT_CONTROL, new object[] {controllingRobot, null});
            }
        }

        private void Update() {
            if (controllingRobot == -1) return;
            var robot = Summary.team.robots[controllingRobot];
            if (Input.GetKeyDown(KeyCode.Y)) {
                if (!lockCamera) {
                    lockCamera = true;
                    Events.Invoke(Events.M_ROBOT_TOWARDS_CHANGE, new object[] {controllingRobot, 0});
                    Cursor.lockState = CursorLockMode.None;
                } else {
                    lockCamera = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            if (!lockCamera) {
                var mouseMove = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * SENSITIVITY;
                if (mouseMove.sqrMagnitude > 0.0001f) {
                    Events.Invoke(Events.M_ROBOT_TOWARDS_CHANGE, new object[] {controllingRobot, 1, mouseMove});
                }
            }

            if (Input.GetMouseButtonDown(0)) {
                Events.Invoke(Events.M_ROBOT_FIRE, new object[] {controllingRobot});
            }

            if (Input.GetMouseButtonDown(1)) {
                if (robot.inventory.Count >= robot.inventoryCapacity) {
                    broadcaster.Broadcast(FULL_INVENTORY_NOTIFY);
                } else {
                    Events.Invoke(Events.M_ROBOT_PICK, new object[] {controllingRobot});
                }
            }

            var motivation = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if ((motivation - lastMotivation).sqrMagnitude > 0.0001f) {
                Events.Invoke(Events.M_ROBOT_MOTIVATION_CHANGE, new object[] {controllingRobot, 0, motivation});
                lastMotivation = motivation;
            }

            if (Input.GetKeyDown(KeyCode.B)) {
                if (robot.allowBuild) {
                    if (robot.atBase != -1) {
                        if (Summary.team.coins < Constants.BASE_CAPTURE_COST) {
                            broadcaster.Broadcast(LACK_MONEY_TO_CAPTURE_NOTIFY);
                        } else {
                            Events.Invoke(Events.M_CAPTURE_BASE, new object[] {robot.atBase, Summary.team.teamColor});
                        }
                    } else {
                        if (Summary.team.coins < Constants.TOWER_TEMPLATES["BaseTower"].cost) {
                            broadcaster.Broadcast(LACK_MONEY_TO_BUILD_TOWER_NOTIFY);
                        } else {
                            var pos3 = GameObject.Find($"Robot_{controllingRobot}").transform.position;
                            var pos2 = new Vector2(pos3.x, pos3.z);
                            Events.Invoke(Events.M_CREATE_TOWER,
                                new object[] {Summary.team.teamColor, "BaseTower", pos2});
                        }
                    }
                } else {
                    broadcaster.Broadcast(BUILD_FAILED_NOTIFY);
                }
            }
        }

        private void OnWeakConnection(object[] args) {
            if (controllingRobot == (int) args[0]) broadcaster.AddLongTermMessage(CONNECTION_WEAK_NOTIFY);
        }

        private void OnConnectionRepair(object[] args) {
            if (controllingRobot == (int) args[0]) broadcaster.RemoveLongTermMessage(CONNECTION_WEAK_NOTIFY);
        }
        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }
    }
}