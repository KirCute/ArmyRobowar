using System;
using Microsoft.Unity.VisualStudio.Editor;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace UI {
    public class MTViewForesight : MonoBehaviour{
        private GameObject foresightObject;
        private Image foresight;//标靶准星

        private void Awake() {
            foresightObject = transform.Find("Foresight").gameObject;
            foresight = foresightObject.GetComponent<Image>();
            foresightObject.SetActive(false);
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MONITOR, OnMonitor);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MONITOR, OnMonitor);
        }

        private void OnMonitor(object[] args) {
            if (Equals((Player) args[1], PhotonNetwork.LocalPlayer)) {
                if ((bool) args[2]) {
                    foresightObject.SetActive(true);
                } else {
                    foresightObject.SetActive(false);
                }
            }
        }
        
    }
}