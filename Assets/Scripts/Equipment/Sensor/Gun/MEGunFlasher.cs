using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor.Gun {
    /// <summary>
    /// 当玩家开炮时，控制LineRenderer生成开炮特效
    /// </summary>
    public class MEGunFlasher : MonoBehaviourPun {
        [SerializeField] private double flashTime = 0.1;
        private MEComponentIdentifier identity;
        private LineRenderer lineRenderer;
        private double shootTime;  // non-sync

        private void Awake() {
            identity = GetComponentInParent<MEComponentIdentifier>();
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update() {
            if (lineRenderer.enabled && Time.time - shootTime > flashTime) {
                lineRenderer.enabled = false;
            }
        }

        private void OnEnable() {
            Events.AddListener(Events.F_ROBOT_FIRED, OnFire);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_ROBOT_FIRED, OnFire);
        }

        private void OnFire(object[] args) {
            if (identity.robotId == (int) args[0]) {
                var from = (Vector3) args[1];  // 开炮点
                var to = (Vector3) args[2];  // 炮击中点
                lineRenderer.SetPosition(0, from);
                lineRenderer.SetPosition(1, to);
                lineRenderer.enabled = true;
                shootTime = Time.time;
            }
        }
    }
}