﻿using System;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot.Body {
    /// <summary>
    /// 用于处理机器人的扣血，并同步血量数据（含发布机器人损坏事件）
    /// </summary>
    public class MERobotHealthChanger : MonoBehaviourPun, IPunObservable {
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_BODY_DAMAGE, OnHealthChanging);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_BODY_DAMAGE, OnHealthChanging);
        }

        private void OnHealthChanging(object[] args) {
            if (identity.id == (int) args[0] && identity.team == Summary.team.teamColor && photonView.IsMine) {
                var health = Summary.team.robots[identity.id].health;
                health = Mathf.Max(health + (int) args[1], 0);
                health = Mathf.Min(health, Summary.team.robots[identity.id].maxHealth);
                Summary.team.robots[identity.id].health = health;
                Events.Invoke(Events.F_BODY_HEALTH_CHANGED, new object[] {identity.id, health});
                if (health <= 0) {
                    Events.Invoke(Events.F_BODY_DESTROYED, new object[] {identity.id});
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (identity.team == Summary.team.teamColor) {
                if (stream.IsWriting) {
                    stream.SendNext(Summary.team.robots[identity.id].health);
                } else {
                    Summary.team.robots[identity.id].health = (int) stream.ReceiveNext();
                }
            }
        }
    }
}