using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equiment {
    /// <summary>
    /// 在接收到小车运动方式需要改变时，改变小车运动方式
    /// </summary>
    public class MEChangeMotivation : MonoBehaviourPun, IPunObservable {
        private int isVerticalPress = 0;
        private int isForward;
        private int isHorizontalPress = 0;
        private int isRightward;
        private Vector3 position;
        private Quaternion rotation;
        private const float linearSpeed = 1.5f;
        private const float degreesPerFrame = 1.0f;

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MOTIVATION_CHANGE, ChangeRobotMotivation);
        }

        private void Update() {
            if (isVerticalPress == 1) {
                if (isForward == 1) {
                    //按下w
                    transform.Translate(Vector3.forward * linearSpeed * Time.deltaTime, Space.Self);
                }

                if (isForward == 0) {
                    //按下s
                    transform.Translate(Vector3.back * linearSpeed * Time.deltaTime, Space.Self);
                }

                position = transform.position;//以便能改变position值从而调用OnPhotonSerializeView
            }

            if (isHorizontalPress == 1) {
                if (isRightward == 1) {
                    //按下a
                    transform.Rotate(0.0f, degreesPerFrame, 0.0f, Space.Self);
                }

                if (isRightward == 0) {
                    //按下d
                    transform.Rotate(0.0f, -degreesPerFrame, 0.0f, Space.Self);
                }

                rotation = transform.rotation;
            }
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MOTIVATION_CHANGE, ChangeRobotMotivation);
        }

        /// <summary>
        /// 仅用于将事件的参数解析并形成全局变量，便于Update()的使用
        /// </summary>
        /// <param name="args[0]">前后键按下|前后键释放，向前|向后，左右键按下|左右键释放，向左|向右（二进制的后四位，前1后0）</param>
        public void ChangeRobotMotivation(object[] args) {
            if (photonView.IsMine) {
                isVerticalPress = ((char)args[0] >> 3) & 1;
                isForward = ((char)args[0] >> 2) & 1;
                isHorizontalPress = ((char)args[0] >> 1) & 1;
                isRightward = ((char)args[0] >> 0) & 1;
                Events.Invoke(Events.F_ROBOT_MOTIVATION_CHANGE, new object[] { args[0] });
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else {
                position = (Vector3)stream.ReceiveNext();
                rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
