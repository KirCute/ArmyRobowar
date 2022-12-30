using Model.Equipment;
using Photon.Pun;
using UnityEngine;

namespace System.Judgement {
    public class METowerBuilder : MonoBehaviour {
        private static int nextTowerId = -1;

        private void OnEnable() {
            Events.AddListener(Events.M_CREATE_TOWER, OnTowerCreating);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CREATE_TOWER, OnTowerCreating);
        }

        private static void OnTowerCreating(object[] args) {
            nextTowerId++;
            if (Summary.team.teamColor == (int) args[0]) {
                var template = Constants.TOWER_TEMPLATES[(string) args[1]];
                Summary.team.towers.Add(nextTowerId, new Tower(template));
                if (Summary.isTeamLeader) {
                    PhotonNetwork.Instantiate(template.prefabName, (Vector3) args[2], Quaternion.identity, 0,
                        new object[] {nextTowerId, Summary.team.teamColor}
                    );
                }
            }
        }
    }
}