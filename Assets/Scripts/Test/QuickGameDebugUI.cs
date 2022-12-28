using System;
using UnityEngine;

namespace Test {
    public class QuickGameDebugUI : MonoBehaviour {
        private int selected = 4;
        private void OnGUI() {
            if (!Summary.isGameStarted) return;
            GUILayout.Window(0, new Rect(60, 80, 500, 20), id => {
                selected = GUILayout.Toolbar(selected, new[] {"团队", "机器人", "仓库", "建筑", "收起"});
                switch (selected) {
                    case 0:
                        GUILayout.Label($"队伍号：{Summary.team.teamColor} ({(Summary.isTeamLeader ? "队长" : "队员")})");
                        GUILayout.Label($"资源：{Summary.team.coins}");
                        GUILayout.Label($"科研点：{Summary.team.researchPoint}");
                        GUILayout.Label("团队成员：");
                        GUILayout.BeginVertical("Box");
                        foreach (var member in Summary.team.members)
                            GUILayout.Label($"{member.ActorNumber} - {member.NickName} - {member.IsLocal} - {member.IsMasterClient}");
                        GUILayout.EndVertical();
                        break;
                    case 1:
                        foreach (var robot in Summary.team.robots.Values) {
                            GUILayout.BeginVertical("Box");
                            GUILayout.Label($"{robot.name} ({robot.id})");
                            GUILayout.Label($"血量：{robot.health}/{robot.maxHealth}");
                            GUILayout.Label($"状态：{robot.status}");
                            GUILayout.Label($"控制者：{(robot.controller == null ? "无" : $"{robot.controller.NickName} ({robot.controller.ActorNumber})")}");
                            GUILayout.Label($"已安装配件 (容量：{robot.template.capacity})：");
                            GUILayout.BeginVertical("Box");
                            foreach (var component in robot.equippedComponents) GUILayout.Label(component == null
                                ? "-"
                                : $"{component.template.name} - {component.template.nameOnTechnologyTree} - {component.health}/{component.template.maxHealth}");
                            GUILayout.EndVertical();
                            GUILayout.Label($"物品栏 (容量：{robot.inventoryCapacity})：");
                            GUILayout.BeginVertical("Box");
                            foreach (var item in robot.inventory) GUILayout.Label($"{item.name}");
                            GUILayout.EndVertical();
                            GUILayout.EndVertical();
                        }
                        break;
                    case 2:
                        foreach (var component in Summary.team.components) {
                            GUILayout.Label($"{component.template.name} - {component.template.nameOnTechnologyTree} - {component.health}/{component.template.maxHealth}");
                        }
                        break;
                    case 3:
                        GUILayout.Label("基地：");
                        GUILayout.BeginVertical("Box");
                        foreach (var basement in Summary.team.bases.Values) {
                            GUILayout.Label($"{basement.id} - {basement.health}");
                        }
                        GUILayout.EndVertical();
                        GUILayout.Label("信号塔：");
                        GUILayout.BeginVertical("Box");
                        foreach (var tower in Summary.team.towers.Values) {
                            GUILayout.Label($"{tower.template.nameOnTechnologyTree} - {tower.health}/{tower.template.maxHealth}");
                        }
                        GUILayout.EndVertical();
                        break;
                }
            }, "测试窗口");
        }
    }
}
