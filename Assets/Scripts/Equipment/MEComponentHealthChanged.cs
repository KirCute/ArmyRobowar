using System;
using System.Collections;
using System.Collections.Generic;
using Model.Equipment.Template;
using Photon.Pun;
using UnityEngine;

namespace Equiment {
    public class MEComponentHealthChanged : MonoBehaviourPun,IPunObservable {
        public int componentBloodVolume = 10;//TODO

        private void OnEnable() {
            Events.AddListener(Events.F_COMPONENT_DAMAGE, DeductBloodVolume);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(componentBloodVolume);
            }
            else {
                componentBloodVolume = (int)stream.ReceiveNext();
            }
        }

        public void DeductBloodVolume(object[] args) {
            if (photonView.IsMine) {
                if (componentBloodVolume - 1 < 0) {//TODO,这个1也要改
                    componentBloodVolume = 0;
                }
                else {
                    componentBloodVolume -= 1;//TODO,需要根据所用炮管的等级
                }

                Events.Invoke(Events.F_COMPONENT_HEALTH_CHANGED, new object[] { });
            }
        }
    }
}