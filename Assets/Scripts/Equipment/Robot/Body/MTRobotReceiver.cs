namespace Equipment.Robot.Body {
    /// <summary>
    /// 用于接受来自信号塔和基地的信号
    /// </summary>
    public class MTRobotReceiver : AbstractMTConnection {
        private MERobotIdentifier identity;
        
        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }
        
        public override void PlusSignal(int signal) {
            Events.Invoke(Events.M_ROBOT_CHANGE_CONNECTION, new object[] {identity.id, signal});
        }

        public override int GetTeamId() {
            return identity.team;
        }
    }
}