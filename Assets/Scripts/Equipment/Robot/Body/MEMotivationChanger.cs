using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot.Body {
    /// <summary>
    /// 在接收到小车运动方式需要改变时，改变小车运动方式
    /// </summary>
    public class MEMotivationChanger : MonoBehaviourPun, IPunObservable {
        private const float LINEAR_SPEED = 2.0f;
        private const float ANGULAR_SPEED = 50f;

        private float motivationHorizontal;
        private float motivationVertical;
        private Vector2 target;
        private int mode;
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
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
                    identity.transform.Translate(Vector3.forward * motivationVertical * LINEAR_SPEED * Time.deltaTime, Space.Self);
                    identity.transform.Rotate(0.0f, motivationHorizontal * ANGULAR_SPEED * Time.deltaTime, 0.0f, Space.Self);
                    break;
                case 1:
                    // TODO
                    break;
            }
        }

        private void ChangeRobotMotivation(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                mode = (int) args[1];
                var motivation = (Vector2) args[2];
                switch (mode) {
                    case 0:
                        motivationHorizontal = motivation.x;
                        motivationVertical = motivation.y;
                        break;
                    case 1:
                        target = motivation;
                        break;
                }

                Events.Invoke(Events.F_ROBOT_MOTIVATION_CHANGED, args);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(motivationHorizontal);
                stream.SendNext(motivationVertical);
                stream.SendNext(target);
                stream.SendNext(mode);
            } else {
                motivationHorizontal = (float) stream.ReceiveNext();
                motivationVertical = (float) stream.ReceiveNext();
                target = (Vector2) stream.ReceiveNext();
                mode = (int) stream.ReceiveNext();
            }
        }
    }
}