using Photon.Pun;
using UnityEngine;

namespace System.TeamHelper {
    public class MERobotUpdater : MonoBehaviourPun {
        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_RELEASE_INVENTORY, OnReleaseInventory);
            Events.AddListener(Events.M_ROBOT_INSTALL_COMPONENT, OnInstallingComponent);
            Events.AddListener(Events.M_ROBOT_UNINSTALL_COMPONENT, OnUninstallingComponent);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_RELEASE_INVENTORY, OnReleaseInventory);
            Events.RemoveListener(Events.M_ROBOT_INSTALL_COMPONENT, OnInstallingComponent);
            Events.RemoveListener(Events.M_ROBOT_UNINSTALL_COMPONENT, OnUninstallingComponent);
        }

        private void OnReleaseInventory(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                var id = (int) args[1];
                if (photonView.IsMine) {
                    foreach (var item in Summary.team.robots[id].inventory) item.StoreIn();
                }

                Summary.team.robots[id].inventory.Clear();
            }
        }

        private void OnInstallingComponent(object[] args) {
            var team = (int) args[0];
            if (Summary.team.teamColor == team) {
                var id = (int) args[1];
                var instIndex = (int) args[2];
                var repoIndex = (int) args[3];
                var sensor = Summary.team.components[repoIndex];
                Summary.team.components.RemoveAt(repoIndex);
                Summary.team.robots[id].equippedComponents[instIndex] = sensor;
                sensor.OnEquipped(id, instIndex, photonView.IsMine);
            }
        }

        private void OnUninstallingComponent(object[] args) {
            var team = (int) args[0];
            if (Summary.team.teamColor == team) {
                var id = (int) args[1];
                var instIndex = (int) args[2];
                var sensor = Summary.team.robots[id].equippedComponents[instIndex];
                if (photonView.IsMine) Events.Invoke(Events.F_TEAM_ACQUIRE_COMPONENT,
                    new object[] {team, sensor.template.nameOnTechnologyTree, sensor.health}
                );
                sensor.OnUnloaded(id, instIndex, photonView.IsMine);
                Summary.team.robots[id].equippedComponents[instIndex] = null;
            }
        }
    }
}