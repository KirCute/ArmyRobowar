﻿using Photon.Pun;
using UnityEngine;

namespace System.Judgement {
    public class MEPickableCreator : MonoBehaviour {
        private static int nextPickableId;

        private void OnEnable() {
            Events.AddListener(Events.M_CREATE_PICKABLE_COINS, OnCoinsCreating);
            Events.AddListener(Events.M_CREATE_PICKABLE_COMPONENT, OnComponentCreating);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CREATE_PICKABLE_COINS, OnCoinsCreating);
            Events.RemoveListener(Events.M_CREATE_PICKABLE_COMPONENT, OnComponentCreating);
        }

        private static void OnCoinsCreating(object[] args) {
            if (PhotonNetwork.IsMasterClient) {
                PhotonNetwork.Instantiate((string) args[0], (Vector3) args[1], Quaternion.identity, 0,
                    new object[] {nextPickableId++});
            }
        }

        private static void OnComponentCreating(object[] args) {
            if (PhotonNetwork.IsMasterClient) {
                var prefab = Constants.SENSOR_TEMPLATES[(string) args[0]].pickablePrefabName;
                PhotonNetwork.Instantiate(prefab, (Vector3) args[2], Quaternion.identity, 0,
                    new[] {nextPickableId++, args[0], args[1]});
            }
        }
    }
}