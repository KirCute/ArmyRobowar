using System;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace Equipment.Robot {
    /// <summary>
    /// 用于处理机器人血量耗尽时的销毁工作（含物品栏掉落）
    /// </summary>
    public class MERobotDestroyer : MonoBehaviourPun {
        private const float DROP_RANGE = 1.5F;
        private static Random rand;
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.F_BODY_DESTROYED, OnDestroying);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_BODY_DESTROYED, OnDestroying);
        }

        private void OnDestroying(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                PhotonNetwork.Destroy(photonView);
            }
        }

        private void OnDestroy() {
            if (Summary.team.teamColor == identity.team) {
                Summary.team.robots[identity.id].connection = 0;
                Summary.team.robots[identity.id].atHome = false;
                if (photonView.IsMine) {
                    rand ??= new Random(Guid.NewGuid().GetHashCode());
                    foreach (var sensor in Summary.team.robots[identity.id].equippedComponents) {
                        if (sensor != null && rand.NextDouble() < sensor.template.dropProbability) {  // 依照概率，如果运气好该掉落了
                            Events.Invoke(Events.M_CREATE_PICKABLE_COMPONENT, new object[] {
                                sensor.template.nameOnTechnologyTree, sensor.health, GenerateDropPos()
                            });
                        }
                    }

                    foreach (var item in Summary.team.robots[identity.id].inventory) {  // 物品栏中的物品一定会掉落
                        item.DropAt(GenerateDropPos());
                    }
                }

                // 销毁配件和物品栏数据
                for (var i = 0; i < Summary.team.robots[identity.id].equippedComponents.Length; i++) {
                    Summary.team.robots[identity.id].equippedComponents[i] = null;
                }
                Summary.team.robots[identity.id].inventory.Clear();
            }
        }

        /// <summary>
        /// 随机生成掉落位置，方法为得到均匀分布的方向和均匀分布的距离
        /// </summary>
        /// <returns>随机出来的位置</returns>
        private Vector3 GenerateDropPos() {
            var angle = (float) (rand.NextDouble() * Mathf.PI * 2.0);
            var distance = (float) (rand.NextDouble() * DROP_RANGE);
            var pos = new Vector3(distance * Mathf.Cos(angle), 0f, distance * Mathf.Sin(angle));
            return transform.position + pos;
        }
    }
}    