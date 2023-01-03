using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Tower.Signal {
    public class MESignal : MonoBehaviourPun {
        private AbstractMESignalIdentifier identity;
        private readonly List<AbstractMTConnection> connecting = new();
        private readonly List<AbstractMTConnection> triggering = new();
        [SerializeField] private int strength;
        
        private void Awake() {
            identity = GetComponentInParent<AbstractMESignalIdentifier>();
        }

        private void Update() {
            for (var i = 0; i < connecting.Count; i++) {
                if (connecting[i] == null) connecting.RemoveAt(i--);
                else if (connecting[i].GetTeamId() != identity.GetTeamId() || !triggering.Contains(connecting[i])) {
                    connecting[i].PlusSignal(-strength);
                    connecting.RemoveAt(i--);
                }
            }

            for (var i = 0; i < triggering.Count; i++) {
                if (triggering[i] == null) triggering.RemoveAt(i--);
                else if (!connecting.Contains(triggering[i]) && triggering[i].GetTeamId() == identity.GetTeamId()) {
                    triggering[i].PlusSignal(strength);
                    connecting.Add(triggering[i]);
                }
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) {
                    triggering.Add(receiver);
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) {
                    triggering.Remove(receiver);
                }
            }
        }

        private void OnDestroy() {
            foreach (var connector in connecting) {
                connector.PlusSignal(-strength);
            }
        }
    }
}