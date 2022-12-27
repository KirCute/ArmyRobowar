using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor {
    public class METowardsChanger : MonoBehaviourPun {
        private static readonly Vector2 ROTATION_X_CLAMP = new(-45f, 45f);
        private static readonly Vector2 ROTATION_Y_CLAMP = new(-60f, 60f);
        private MEComponentIdentifier identity;

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_TOWARDS_CHANGE, OnTowardsChanging);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_TOWARDS_CHANGE, OnTowardsChanging);
        }

        private void OnTowardsChanging(object[] args) {
            if (identity.robotId == (int) args[0] && photonView.IsMine) {
                var rotationX = transform.localEulerAngles.x;
                var rotationY = transform.localEulerAngles.y;
                var change = (Vector2) args[1];
                rotationX += change[0];
                rotationX = Mathf.Clamp(rotationX, ROTATION_X_CLAMP.x, ROTATION_X_CLAMP.y);
                rotationY += change[1];
                rotationY = Mathf.Clamp(rotationY, ROTATION_Y_CLAMP.x, ROTATION_Y_CLAMP.y);
                transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
            }
        }
    }
}