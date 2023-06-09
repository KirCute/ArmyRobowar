﻿using System;
using System.Linq;
using Map.Navigation;
using Photon.Pun;
using UnityEngine;

namespace Test {
    public class QuickGameOperator : MonoBehaviour {
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Events.Invoke(Events.M_TEAM_BUY_COMPONENT, new object[] {Summary.team.teamColor, "BaseCamera"});
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Events.Invoke(Events.M_TEAM_BUY_COMPONENT, new object[] {Summary.team.teamColor, "BaseGun"});
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                var baseId = Summary.team.bases.Keys.First();
                Events.Invoke(Events.M_CREATE_ROBOT, new object[] {baseId, "BaseRobot", $"TestCar_{Summary.team.teamColor}"});
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                var robotId = Summary.team.robots.Keys.First();
                var place = Summary.team.robots[robotId].equippedComponents[0] == null ? 0 : 1;
                Events.Invoke(Events.M_ROBOT_INSTALL_COMPONENT, new object[] {Summary.team.teamColor, robotId, place, 0});
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                var robotId = Summary.team.robots.Keys.First();
                Events.Invoke(Events.M_ROBOT_MONITOR, new object[] {Summary.team.robots[robotId].id, PhotonNetwork.LocalPlayer, true});
                Events.Invoke(Events.M_ROBOT_CONTROL, new object[] {Summary.team.robots[robotId].id, PhotonNetwork.LocalPlayer});
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                var robotId = Summary.team.robots.Keys.First();
                Events.Invoke(Events.M_ROBOT_MONITOR, new object[] {Summary.team.robots[robotId].id, PhotonNetwork.LocalPlayer, false});
                Events.Invoke(Events.M_ROBOT_CONTROL, new object[] {Summary.team.robots[robotId].id, null});
            }
            if (Input.GetKeyDown(KeyCode.Alpha7)) {
                var robotId = Summary.team.robots.Keys.First();
                Events.Invoke(Events.M_ROBOT_RELEASE_INVENTORY, new object[] {Summary.team.teamColor, Summary.team.robots[robotId].id});
            }
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                var robotId = Summary.team.robots.Keys.First();
                var from3 = GameObject.Find($"Robot_{robotId}").transform.position;
                var from = new Vector2(from3.x, from3.z);
                var to = Vector2.zero;
                var path = MDNavigationCenter.GetInstance().GetFinalPath(new [] {from, to});
                var input = new object[path.Count + 2];
                input[0] = robotId;
                input[1] = path.Count;
                for (var i = 2; i < path.Count + 2; i++) input[i] = path[i - 2];
                Events.Invoke(Events.M_ROBOT_NAVIGATION, input);
            }
        }
    }
}