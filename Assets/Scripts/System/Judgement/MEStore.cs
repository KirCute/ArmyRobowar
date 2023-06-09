﻿using System.Linq;
using Photon.Pun;

namespace System.Judgement {
    /// <summary>
    /// 一切扣费事件应答脚本
    /// </summary>
    public class MEStore : MonoBehaviourPun {
        private void OnEnable() {
            Events.AddListener(Events.M_CREATE_ROBOT, OnCreateRobot);
            Events.AddListener(Events.M_TEAM_BUY_COMPONENT, OnBuyingComponent);
            Events.AddListener(Events.M_CREATE_TOWER, OnCreatingTower);
            Events.AddListener(Events.M_CAPTURE_BASE, OnCapturing);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CREATE_ROBOT, OnCreateRobot);
            Events.RemoveListener(Events.M_TEAM_BUY_COMPONENT, OnBuyingComponent);
            Events.RemoveListener(Events.M_CREATE_TOWER, OnCreatingTower);
            Events.RemoveListener(Events.M_CAPTURE_BASE, OnCapturing);
        }

        private static void OnCreateRobot(object[] args) {
            // 购买机器人的扣费
            if (Summary.team.bases.Keys.Contains((int) args[0])) {
                Summary.team.coins -= Constants.ROBOT_TEMPLATES[(string) args[1]].cost;
            }
        }

        private void OnBuyingComponent(object[] args) {
            // 购买配件的扣费
            var template = Constants.SENSOR_TEMPLATES[(string) args[1]];
            if (Summary.team.teamColor == (int) args[0]) {
                Summary.team.coins -= template.cost;
            }

            if (photonView.IsMine) Events.Invoke(Events.F_TEAM_ACQUIRE_COMPONENT, new[] {
                args[0], args[1], template.maxHealth
            });
        }

        private static void OnCreatingTower(object[] args) {
            // 造信号塔的扣费
            if (Summary.team.teamColor == (int) args[0]) {
                Summary.team.coins -= Constants.TOWER_TEMPLATES[(string) args[1]].cost;
            }
        }

        private static void OnCapturing(object[] args) {
            // 占领基地的扣费
            if (Summary.team.teamColor == (int) args[1]) {
                Summary.team.coins -= Constants.BASE_CAPTURE_COST;
            }
        }
    }
}