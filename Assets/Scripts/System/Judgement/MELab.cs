using UnityEngine;

namespace System.Judgement {
    /// <summary>
    /// 研发科技点事件应答脚本
    /// </summary>
    public class MELab : MonoBehaviour {
        private void OnEnable() {
            Events.AddListener(Events.M_TECHNOLOGY_RESEARCH, OnResearching);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_TECHNOLOGY_RESEARCH, OnResearching);
        }

        private static void OnResearching(object[] args) {
            if (Summary.team.teamColor == (int) args[0]) {
                var technic = (string) args[1];
                if (Constants.TECHNOLOGY.ContainsKey(technic) && !Summary.team.achievedTechnics.Contains(technic)) {
                    Summary.team.achievedTechnics.Add(technic);
                    Constants.TECHNOLOGY[technic].Unlock(Summary.team);
                    Summary.team.researchPoint -= Constants.TECHNOLOGY[technic].cost;
                }
            }
        }
    }
}