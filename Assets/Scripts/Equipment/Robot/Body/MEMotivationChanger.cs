using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot.Body {
    /// <summary>
    /// 在接收到小车运动方式需要改变时，改变小车运动方式
    /// </summary>
    public class MEMotivationChanger : MonoBehaviourPun, IPunObservable {
        private int isVerticalPress = 0;
        private int isForward;
        private int isHorizontalPress = 0;
        private int isRightward;
        private const float LINEAR_SPEED = 1.5f;
        private const float DEGREES_PER_FRAME = 1.0f;
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MOTIVATION_CHANGE, ChangeRobotMotivation);
        }

        private void Update() {
            if (isVerticalPress == 1) {
                if (isForward == 1) {
                    //按下w
                    transform.Translate(Vector3.forward * LINEAR_SPEED * Time.deltaTime, Space.Self);
                }

                if (isForward == 0) {
                    //按下s
                    transform.Translate(Vector3.back * LINEAR_SPEED * Time.deltaTime, Space.Self);
                }
            }

            if (isHorizontalPress == 1) {
                if (isRightward == 1) {
                    //按下a
                    transform.Rotate(0.0f, DEGREES_PER_FRAME, 0.0f, Space.Self);
                }

                if (isRightward == 0) {
                    //按下d
                    transform.Rotate(0.0f, -DEGREES_PER_FRAME, 0.0f, Space.Self);
                }
            }
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MOTIVATION_CHANGE, ChangeRobotMotivation);
        }

        /// <summary>
        /// 仅用于将事件的参数解析并形成全局变量，便于Update()的使用
        /// </summary>
        /// <param name="args[0]">被改变运动模式的小车id</param>
        /// <param name="args[1]">前后键按下|前后键释放，向前|向后，左右键按下|左右键释放，向左|向右（二进制的后四位，前1后0）</param>
        public void ChangeRobotMotivation(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                isVerticalPress = ((char) args[1] >> 3) & 1;
                isForward = ((char) args[1] >> 2) & 1;
                isHorizontalPress = ((char) args[1] >> 1) & 1;
                isRightward = ((char) args[1] >> 0) & 1;
                Events.Invoke(Events.F_ROBOT_MOTIVATION_CHANGED, new object[] {args[0]});
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(isVerticalPress);
                stream.SendNext(isForward);
                stream.SendNext(isHorizontalPress);
                stream.SendNext(isRightward);
            } else {
                isVerticalPress = (int) stream.ReceiveNext();
                isForward = (int) stream.ReceiveNext();
                isHorizontalPress = (int) stream.ReceiveNext();
                isRightward = (int) stream.ReceiveNext();
            }
        }
    }
}