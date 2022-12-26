using Photon.Pun;
using UnityEngine;

namespace System.TeamHelper {
    public class METowerBuilder : MonoBehaviourPun {
        private const string TOWER_PREFAB_NAME = "Tower";
        private static int nextTowerId = -1;

        private void OnEnable() {
            Events.AddListener(Events.M_CREATE_TOWER, OnTowerCreating);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CREATE_TOWER, OnTowerCreating);
        }

        private void OnTowerCreating(object[] args) {
            nextTowerId++;
            if (Summary.team.teamColor == (int) args[0] && photonView.IsMine) {
                PhotonNetwork.Instantiate(TOWER_PREFAB_NAME, (Vector3) args[1], Quaternion.identity, 0,
                    new object[] {nextTowerId, Summary.team.teamColor}
                );
            }
        }
    }
}