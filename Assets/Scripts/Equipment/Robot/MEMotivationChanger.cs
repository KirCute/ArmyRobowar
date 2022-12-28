using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot {
    /// <summary>
    /// 在接收到小车运动方式需要改变时，改变小车运动方式
    /// </summary>
    public class MEMotivationChanger : MonoBehaviourPun /*, IPunObservable*/ {
        private const float LINEAR_SPEED = 2.0f;
        private const float ANGULAR_SPEED = 2.5f;
        private const float NAVIGATION_MOVE_ANGULAR_ERROR = 5f;

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
                    rbody.velocity = transform.forward * motivation.y * LINEAR_SPEED;
                    rbody.angularVelocity = new Vector3(0f, motivation.x * ANGULAR_SPEED, 0f);
                    break;
                case 1:
                    var pos = new Vector2(transform.position.x, transform.position.z);
                    var dis = (target - pos).normalized;
                    var forward = new Vector2(transform.forward.x, transform.forward.z);
                    var angle = Vector2.Angle(forward, dis);
                    if (angle > NAVIGATION_MOVE_ANGULAR_ERROR) {
                        rbody.velocity = Vector3.zero;
                        rbody.angularVelocity =
                            new Vector3(0f, (Vector3.Cross(dis, forward).z > 0 ? 1f : -1f) * ANGULAR_SPEED, 0f);
                    } else {
                        rbody.velocity = new Vector3(dis.x, 0f, dis.y) * LINEAR_SPEED;
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
        /*
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(motivation);
                stream.SendNext(target);
                stream.SendNext(mode);
            } else {
                motivation = (Vector2) stream.ReceiveNext();
                target = (Vector2) stream.ReceiveNext();
                mode = (int) stream.ReceiveNext();
            }
        }
		*/
    }
}