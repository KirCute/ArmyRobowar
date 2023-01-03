using Photon.Pun;

namespace System.Judgement {
    public class MTScoreboard : MonoBehaviourPun {
        private int cntBase = 1;
        
        private void OnEnable() {
            Events.AddListener(Events.M_CAPTURE_BASE, OnBaseCaptured);
            Events.AddListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CAPTURE_BASE, OnBaseCaptured);
            Events.RemoveListener(Events.F_BASE_DESTROYED, OnBaseDestroyed);
        }

        private void OnBaseDestroyed(object[] args) {
            if (Summary.team.teamColor == (int) args[1]) {
                cntBase--;
                if (cntBase == 0 && Summary.isTeamLeader) {
                    Events.Invoke(Events.F_GAME_OVER, new object[] {1 - Summary.team.teamColor});
                }
            }
        }

        private void OnBaseCaptured(object[] args) {
            if (Summary.team.teamColor == (int) args[1]) {
                cntBase++;
            }
        }
    }
}