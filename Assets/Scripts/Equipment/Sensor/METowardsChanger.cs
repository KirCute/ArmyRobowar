using Photon.Pun;
using UnityEngine;

namespace Equipment.Sensor {
    public class METowardsChanger : MonoBehaviourPun {
        private MEComponentIdentifier identity;

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_TOWARDS_CHANGE, OnTowardsChanging);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_TOWARDS_CHANGE, OnTowardsChanging);
        }

        private void OnTowardsChanging(object[] args) {
            if (identity.robotId == (int) args[0] && photonView.IsMine) {
                transform.forward = (Vector3) args[1];
            }
        }
    }
}