using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Tower.Signal {
    public class MESignal : MonoBehaviourPun {
        private AbstractMESignalIdentifier identity;
        private readonly List<AbstractMTConnection> connecting = new();
        [SerializeField] private int strength;
        
        private void Awake() {
            identity = GetComponentInParent<AbstractMESignalIdentifier>();
        }
        
        private void OnTriggerEnter(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) {
                    receiver.PlusSignal(strength, identity.GetTeamId());
                    connecting.Add(receiver);
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) {
                    receiver.PlusSignal(-strength, identity.GetTeamId());
                    connecting.Remove(receiver);
                }
            }
        }

        private void OnDestroy() {
            foreach (var connector in connecting) {
                connector.PlusSignal(-strength, identity.GetTeamId());
            }
        }
    }
}