﻿using System.Pickable;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot.Arm {
    /// <summary>
    /// 用于检测机器人的可拾取状态，处理玩家的拾取指令
    /// </summary>
    public class MTRobotArm : MonoBehaviourPun {
        private const float MAX_GRAB_DISTANCE = 7.5f;
		private const int GRAB_LAYER_MASK = 1 << 10;
        
        private MERobotIdentifier identity;
        private AbstractMTPickable lastFound;  // 缓存看到的物体

        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
            lastFound = null;
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_PICK, OnPick);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_PICK, OnPick);
        }

        private void Update() {
            if (photonView.IsMine) {
                var ret = Physics.Raycast(transform.position, transform.forward, out var hit, MAX_GRAB_DISTANCE, GRAB_LAYER_MASK);
                var found = ret ? hit.collider.GetComponent<AbstractMTPickable>() : null;
                if (found != lastFound) {  // 看到的物体发生改变
                    if (found == null) Events.Invoke(Events.F_ROBOT_LOST_FOUND_PICKABLE, new object[] {identity.id});
                    else Events.Invoke(Events.F_ROBOT_FOUND_PICKABLE, new object[] {identity.id, found.pickableName});
                }

                lastFound = found;
            }
        }

        private void OnPick(object[] args) {
            if (lastFound != null && identity.id == (int) args[0]) {
                lastFound.Pickup(identity.team, identity.id);
            }
        }
    }
}