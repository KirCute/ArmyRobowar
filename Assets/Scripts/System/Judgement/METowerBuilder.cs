using Model.Equipment;
using Photon.Pun;
using UnityEngine;

namespace System.Judgement {
    public class METowerBuilder : MonoBehaviour {
        private const float TOWER_BUILD_HEIGHT = 4F;
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
                    var pos2d = (Vector2) args[2];
                    var pos = new Vector3(pos2d.x, TOWER_BUILD_HEIGHT, pos2d.y);
                    PhotonNetwork.Instantiate(template.prefabName, pos, Quaternion.identity, 0,
                        new object[] {nextTowerId, Summary.team.teamColor}
                    );
                }
            }
        }
    }
}