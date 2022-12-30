using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor.Gun {
    public class MEGunFlasher : MonoBehaviourPun {
        private const double FLASH_TIME = 0.1;
        private MEComponentIdentifier identity;
        private LineRenderer lineRenderer;
        private double shootTime;

        private void Awake() {
            identity = GetComponentInParent<MEComponentIdentifier>();
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update() {
            if (lineRenderer.enabled && Time.time - shootTime > FLASH_TIME) {
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
                var from = (Vector3) args[1];
                var to = (Vector3) args[2];
                lineRenderer.SetPosition(0, from);
                lineRenderer.SetPosition(1, to);
                lineRenderer.enabled = true;
                shootTime = Time.time;
            }
        }
    }
}