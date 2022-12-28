using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot {
    /// <summary>
    /// 在接收到小车运动方式需要改变时，改变小车运动方式
    /// </summary>
    public class MEMotivationChanger : MonoBehaviourPun/*, IPunObservable*/ {
        private const float LINEAR_SPEED = 2.0f;
        private const float ANGULAR_SPEED = 3.5f;

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
                    // TODO
                    break;
            }
        }

        private void ChangeRobotMotivation(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                mode = (int) args[1];
                var moti = (Vector2) args[2];
                switch (mode) {
                    case 0:
                        this.motivation = moti;
                        break;
                    case 1:
                        target = moti;
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