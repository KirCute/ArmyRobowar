using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement {
    public class MEBaseFlag : MonoBehaviourPun {
        public int baseId;
        [NonSerialized] public int flagColor = -1;

        private void OnEnable() {
            Events.AddListener(Events.M_CAPTURE_BASE, OnCapture);
            Events.AddListener(Events.F_BASE_DESTROYED, OnConquered);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CAPTURE_BASE, OnCapture);
            Events.RemoveListener(Events.F_BASE_DESTROYED, OnConquered);
        }

        private void OnCapture(object[] args) {
            if (baseId == (int) args[1]) {
                flagColor = (int) args[1];
                if (flagColor == Summary.team.teamColor) {
                    Summary.team.bases.Add(baseId, new Model.Equipment.Basement(baseId));
                    foreach (var obj in gameObject.GetComponentsInChildren<Transform>()
                                 .Where(t => t != transform)
                                 .Select(t => t.gameObject)) {
                        obj.SetActive(true);
                    }

                    if (Summary.isTeamLeader) {
                        foreach (var photon in GetComponentsInChildren<PhotonView>()) {
                            photon.TransferOwnership(PhotonNetwork.LocalPlayer);
                        }
                    }
                }
            }
        }

        private void OnConquered(object[] args) {
            if (baseId == (int) args[0]) {
                if (flagColor == Summary.team.teamColor) {
                    Summary.team.bases.Remove(baseId);
                    foreach (var obj in gameObject.GetComponentsInChildren<Transform>()
                                 .Where(t => t != transform)
                                 .Select(t => t.gameObject)) {
                        obj.SetActive(false);
                    }

                    if (Summary.isTeamLeader) {
                        foreach (var photon in GetComponentsInChildren<PhotonView>()) {
                            photon.TransferOwnership(PhotonNetwork.MasterClient);
                        }
                    }
                }

                flagColor = -1;
            }
        }
    }
}