using UnityEngine;

namespace Equipment.Robot.Wheel {
    public class MEWheelAnimator : MonoBehaviour {
        [SerializeField] private float linearEffect = -1.0f;
        [SerializeField] private float angularEffect = -1.0f;
        private Transform robot;
        private Vector3 lastPos;
        private Vector3 lastRot;
        
        private void Awake() {
            robot = GetComponentInParent<Rigidbody>().transform;
            lastPos = robot.position;
            lastRot = robot.localEulerAngles;
        }

        private void Update() {
            var euler = transform.localEulerAngles;
            var pos = robot.position;
            var rot = robot.localEulerAngles;
            euler.y += Vector3.Dot(pos - lastPos, robot.forward) * linearEffect;
            euler.y += (rot.y - lastRot.y) * angularEffect;
            transform.localEulerAngles = euler;
            lastPos = pos;
            lastRot = rot;
        }
    }
}