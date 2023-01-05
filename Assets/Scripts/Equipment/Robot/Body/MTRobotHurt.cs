namespace Equipment.Robot.Body {
    /// <summary>
    /// 当机器人被击中时生效，提供机器人的扣血方式
    /// </summary>
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