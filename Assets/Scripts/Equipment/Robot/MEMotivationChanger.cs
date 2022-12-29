using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot {
    /// <summary>
    /// 在接收到小车运动方式需要改变时，改变小车运动方式
    /// </summary>
    public class MEMotivationChanger : MonoBehaviourPun {
        private const float NAVIGATION_MOVE_ANGULAR_ERROR = 5f;
        [SerializeField] private float linearSpeed = 2.0f;
        [SerializeField] private float angularSpeed = 2.5f;

        private Vector2 motivation;
        private Vector2 target;
        private int mode;
        private MERobotIdentifier identity;
        private Rigidbody rbody;

        private void Awake() {
            identity = GetComponent<MERobotIdentifier>();
            rbody = GetComponent<Rigidbody>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MOTIVATION_CHANGE, ChangeRobotMotivation);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MOTIVATION_CHANGE, ChangeRobotMotivation);
        }

        private void Update() {
            switch (mode) {
                case 0:
                    rbody.velocity = transform.forward * motivation.y * linearSpeed;
                    rbody.angularVelocity = new Vector3(0f, motivation.x * angularSpeed, 0f);
                    break;
                case 1:
                    var pos = new Vector2(transform.position.x, transform.position.z);
                    var dis = (target - pos).normalized;
                    var forward = new Vector2(transform.forward.x, transform.forward.z);
                    var angle = Vector2.Angle(forward, dis);
                    if (angle > NAVIGATION_MOVE_ANGULAR_ERROR) {
                        rbody.velocity = Vector3.zero;
                        rbody.angularVelocity =
                            new Vector3(0f, (Vector3.Cross(dis, forward).z > 0 ? 1f : -1f) * angularSpeed, 0f);
                    } else {
                        rbody.velocity = new Vector3(dis.x, 0f, dis.y) * linearSpeed;
                    }

                    break;
            }
        }

        private void ChangeRobotMotivation(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                mode = (int) args[1];
                var m = (Vector2) args[2];
                switch (mode) {
                    case 0:
                        motivation = m;
                        target = Vector2.zero;
                        break;
                    case 1:
                        motivation = Vector2.zero;
                        target = m;
                        break;
                }

                Events.Invoke(Events.F_ROBOT_MOTIVATION_CHANGED, args);
            }
        }
    }
}