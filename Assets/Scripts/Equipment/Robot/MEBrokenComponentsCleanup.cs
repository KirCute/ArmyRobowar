using System;
using Photon.Pun;

namespace Equipment.Robot {
    public class MEBrokenComponentsCleanup : MonoBehaviourPun {
        private MERobotIdentifier identity;

        private void Awake() {
            identity = GetComponent<MERobotIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.F_COMPONENT_DESTROYED, OnDestroying);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_COMPONENT_DESTROYED, OnDestroying);
        }

        private void OnDestroying(object[] args) {
            if (identity.id == (int) args[0] && identity.team == Summary.team.teamColor) {
                var index = (int) args[1];
                Summary.team.robots[identity.id].equippedComponents[index] = null;
            }
        }
    }
}