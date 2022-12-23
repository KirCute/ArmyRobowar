using Photon.Pun;

namespace Equipment.Robot.Body {
    public class MTRobotReceiver : AbstractMTConnection {
        private MERobotIdentifier identity;
        
        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }
        
        public override void PlusSignal(int signal, int team) {
            if (identity.team != team) return;
            Events.Invoke(Events.M_ROBOT_CHANGE_CONNECTION, new object[] {identity.id, signal});
        }
    }
}