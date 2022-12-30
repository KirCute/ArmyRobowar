using Equipment.Robot;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor {
    public class METowardsChanger : MonoBehaviourPun {
        private MERobotIdentifier identity;
        [SerializeField] private Vector2 rotationXClamp = new(-20f, 45f);
        [SerializeField] private Vector2 rotationYClamp = new(-60f, 60f);
        [SerializeField] private bool lockRotationX;
        [SerializeField] private bool lockRotationY;

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
                switch ((int) args[1]) {
                    case 0:
                        transform.localEulerAngles = new Vector3(
                            lockRotationX ? rotationX : 0f,
                            lockRotationY ? rotationY : 0f,
                            0f
                        );
                        break;
                    case 1:
                        var change = (Vector2) args[2];
                        if (!lockRotationX) {
                            rotationX += change[0];
                            while (rotationX > 180f) rotationX -= 360f;
                            while (rotationX < -180f) rotationX += 360f;
                            if (rotationX < rotationXClamp.x) rotationX = rotationXClamp.x;
                            if (rotationX > rotationXClamp.y) rotationX = rotationXClamp.y;
                        }

                        if (!lockRotationY) {
                            rotationY += change[1];
                            while (rotationY > 180f) rotationY -= 360f;
                            while (rotationY < -180f) rotationY += 360f;
                            if (rotationY < rotationYClamp.x) rotationY = rotationYClamp.x;
                            if (rotationY > rotationYClamp.y) rotationY = rotationYClamp.y;
                        }

                        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
                        break;
                }
            }
        }
    }
}