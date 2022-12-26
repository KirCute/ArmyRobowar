using Photon.Pun;
using UnityEngine;

namespace System.Judgement {
    public class MEPickableCreator : MonoBehaviourPun {
        private const string PICKABLE_COINS_PREFAB_NAME = "PickableCoin";
        private const string PICKABLE_COMPONENT_PREFAB_NAME = "PickableComponent";
        
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
                PhotonNetwork.Instantiate(PICKABLE_COINS_PREFAB_NAME, (Vector3) args[1], Quaternion.identity, 0,
                    new[] {args[0]});
            }
        }

        private static void OnComponentCreating(object[] args) {
            if (PhotonNetwork.IsMasterClient) {
                PhotonNetwork.Instantiate(PICKABLE_COMPONENT_PREFAB_NAME, (Vector3) args[2], Quaternion.identity, 0,
                    new[] {args[0], args[1]});
            }
        }
    }
}