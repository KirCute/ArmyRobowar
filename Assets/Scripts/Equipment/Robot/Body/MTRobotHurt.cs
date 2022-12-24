namespace Equipment.Robot.Body {
    public class MTRobotHurt : AbstractMTHurt {
        private MERobotIdentifier identity;
        
        private void Awake() {
            identity = GetComponentInParent<MERobotIdentifier>();
        }

        public override void Hurt(int damage, int team) {
            if (identity.team == team) return;
            if (enabled) Events.Invoke(Events.M_BODY_DAMAGE, new object[] {identity.id, -damage});
        }
    }
}