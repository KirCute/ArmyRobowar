using Photon.Pun;
using UnityEngine;

namespace Equipment.Tower.Signal {
    public class MESignal : MonoBehaviourPun {
        private AbstractMESignalIdentifier identity;
        [SerializeField] private int strength;
        
        private void Awake() {
            identity = GetComponentInParent<AbstractMESignalIdentifier>();
        }
        
        private void OnTriggerEnter(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) receiver.PlusSignal(strength, identity.GetTeamId());
            }
        }

        private void OnTriggerExit(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) receiver.PlusSignal(-strength, identity.GetTeamId());
            }
        }
    }
}