using Photon.Pun;
using UnityEngine;

namespace Equipment.Tower.Signal {
    public class METowerConnection : MonoBehaviourPun {
        private METowerIdentifier identity;
        [SerializeField] private int strength;
        
        private void Awake() {
            identity = GetComponentInParent<METowerIdentifier>();
        }
        
        private void OnTriggerEnter(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) receiver.PlusSignal(strength, identity.team);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) receiver.PlusSignal(-strength, identity.team);
            }
        }
    }
}