using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor.Gun {
    /// <summary>
    /// 处理玩家的开炮请求
    /// 当玩家开炮时，会向前发射激光，与激光碰撞到的AbstractMTHurt实现脚本交互，使可扣血物体扣血
    /// </summary>
    public class MEFire : MonoBehaviourPun {
        private const float MAX_SHOOT_DISTANCE = 20f;
        private const int SHOOT_MASK = (1 << 3) + (1 << 6) + (1 << 7) + (1 << 8) + (1 << 9);  // Terrain, Body, Component, Tower, Diamond
        
        [SerializeField] private int damage = 3;
        public double loadingTime = 4.0;
        private MEComponentIdentifier identity;
        private double lastShoot;  // peer-to-peer，上次开炮事件，避免在Update里重复计算装填逻辑
        public float loadProcess {  // 装填进度，最大为1，刚开完炮为0，用来控制装填UI
            get {
                var p = (float) ((PhotonNetwork.Time - lastShoot) / loadingTime);
                return p > 1.0f ? 1.0f : p;
            }
        }

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
            lastShoot = PhotonNetwork.Time;
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_FIRE, OnFire);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_FIRE, OnFire);
        }

        private void OnFire(object[] args) {
            if (identity.robotId == (int) args[0] && PhotonNetwork.Time - lastShoot > loadingTime) {
                if (photonView.IsMine) {
                    if (Physics.Raycast(transform.position, transform.forward, out var hit, MAX_SHOOT_DISTANCE,
                            SHOOT_MASK)) {  // 开炮
                        var hurt = hit.collider.GetComponent<AbstractMTHurt>();
                        if (hurt != null) hurt.Hurt(damage, identity.team);  // 如果打中的东西能扣血
                    }

                    Events.Invoke(Events.F_ROBOT_FIRED, new object[] {identity.robotId, transform.position, hit.point});
                }

                lastShoot = PhotonNetwork.Time;
            }
        }
    }
}