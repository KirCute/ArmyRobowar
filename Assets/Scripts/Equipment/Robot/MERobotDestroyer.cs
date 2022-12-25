using System;
using Photon.Pun;

namespace Equipment.Robot {
    public class MERobotDestroyer : MonoBehaviourPun {
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.F_BODY_DESTROYED, OnDestroying);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_BODY_DESTROYED, OnDestroying);
        }

        private void OnDestroying(object[] args) {
            if (identity.id == (int) args[0] && photonView.IsMine) {
                PhotonNetwork.Destroy(photonView);
            }
        }

        private void OnDestroy() {
            if (Summary.team.teamColor == identity.team) {
                //Summary.team.robots[identity.id].equippedComponents[0].
                Summary.team.robots[identity.id].connection = 0;
                Summary.team.robots[identity.id].gameObject = null;
                Events.Invoke(Events.F_CREATE_ITEM,new object[] { identity.team, identity.id });
            }
        }
    }
}    