using UnityEngine;

namespace System.Judgement {
    /// <summary>
    /// 机器人改装和卸货事件应答脚本
    /// </summary>
    public class MERobotUpdater : MonoBehaviour {
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

        private static void OnReleaseInventory(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                var id = (int) args[1];
                if (Summary.isTeamLeader) {
                    foreach (var item in Summary.team.robots[id].inventory) item.StoreIn();
                }

                Summary.team.robots[id].inventory.Clear();
            }
        }

        private static void OnInstallingComponent(object[] args) {
            var team = (int) args[0];
            if (Summary.team.teamColor == team) {
                var id = (int) args[1];
                var instIndex = (int) args[2];
                var repoIndex = (int) args[3];
                var sensor = Summary.team.components[repoIndex];
                Summary.team.components.RemoveAt(repoIndex);
                Summary.team.robots[id].equippedComponents[instIndex] = sensor;
                sensor.OnEquipped(id, instIndex, Summary.isTeamLeader);
            }
        }

        private static void OnUninstallingComponent(object[] args) {
            var team = (int) args[0];
            if (Summary.team.teamColor == team) {
                var id = (int) args[1];
                var instIndex = (int) args[2];
                var sensor = Summary.team.robots[id].equippedComponents[instIndex];
                if (Summary.isTeamLeader) {  // 卸下的配件收入仓库
                    Events.Invoke(Events.F_TEAM_ACQUIRE_COMPONENT,
                        new object[] {team, sensor.template.nameOnTechnologyTree, sensor.health}
                    );
                }

                sensor.OnUnloaded(id, instIndex, Summary.isTeamLeader);
                Summary.team.robots[id].equippedComponents[instIndex] = null;
            }
        }
    }
}