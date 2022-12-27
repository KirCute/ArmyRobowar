using Equipment.Robot;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor {
    public class METowardsChanger : MonoBehaviourPun {
        private static readonly Vector2 ROTATION_X_CLAMP = new(-20f, 45f);
        private static readonly Vector2 ROTATION_Y_CLAMP = new(-60f, 60f);
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_TOWARDS_CHANGE, OnTowardsChanging);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_TOWARDS_CHANGE, OnTowardsChanging);
        }

        private void OnTowardsChanging(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                var rotationX = transform.localEulerAngles.x;
                var rotationY = transform.localEulerAngles.y;
                var change = (Vector2) args[1];
                rotationX += change[0];
                while (rotationX > 180f) rotationX -= 360f;
                while (rotationX < -180f) rotationX += 360f;
                if (rotationX < ROTATION_X_CLAMP.x) rotationX = ROTATION_X_CLAMP.x;
                if (rotationX > ROTATION_X_CLAMP.y) rotationX = ROTATION_X_CLAMP.y;
                rotationY += change[1];
                while (rotationY > 180f) rotationY -= 360f;
                while (rotationY < -180f) rotationY += 360f;
                if (rotationY < ROTATION_Y_CLAMP.x) rotationY = ROTATION_Y_CLAMP.x;
                if (rotationY > ROTATION_Y_CLAMP.y) rotationY = ROTATION_Y_CLAMP.y;
                transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
            }
        }
    }
}