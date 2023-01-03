using Equipment.Robot;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor {
    public class METowardsChanger : MonoBehaviourPun {
        private MERobotIdentifier identity;
        [SerializeField] private Vector2 rotationYClamp = new(-20f, 45f);
        [SerializeField] private Vector2 rotationZClamp = new(-60f, 60f);
        [SerializeField] private bool lockRotationY;
        [SerializeField] private bool lockRotationZ;

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
            if (identity.id == (int) args[0]) {
                var rotationY = transform.localEulerAngles.y;
                var rotationZ = transform.localEulerAngles.z;
                switch ((int) args[1]) {
                    case 0:
                        transform.localEulerAngles = new Vector3(
                            0f,
                            lockRotationY ? rotationY : 0f,
                            lockRotationZ ? rotationZ : 0f
                        );
                        break;
                    case 1:
                        var change = (Vector2) args[2];
						change.x = -change.x;
                        if (!lockRotationY) {
                            rotationY += change[0];
                            while (rotationY > 180f) rotationY -= 360f;
                            while (rotationY < -180f) rotationY += 360f;
                            if (rotationY < rotationYClamp.x) rotationY = rotationYClamp.x;
                            if (rotationY > rotationYClamp.y) rotationY = rotationYClamp.y;
                        }

                        if (!lockRotationZ) {
                            rotationZ += change[1];
                            while (rotationZ > 180f) rotationZ -= 360f;
                            while (rotationZ < -180f) rotationZ += 360f;
                            if (rotationZ < rotationZClamp.x) rotationZ = rotationZClamp.x;
                            if (rotationZ > rotationZClamp.y) rotationZ = rotationZClamp.y;
                        }

                        transform.localEulerAngles = new Vector3(0f, rotationY, rotationZ);
                        break;
                }
            }
        }
    }
}