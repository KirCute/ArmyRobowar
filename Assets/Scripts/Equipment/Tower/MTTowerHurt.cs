namespace Equipment.Tower {
    /// <summary>
    /// 当信号塔被击中时生效，提供信号塔的扣血方式
    /// </summary>
    public class MTTowerHurt : AbstractMTHurt {
        private METowerIdentifier identity;
        
        private void Awake() {
            identity = GetComponent<METowerIdentifier>();
        }

        public override void Hurt(int damage, int team) {
            if (identity.team == team) return;
            if (enabled) Events.Invoke(Events.M_TOWER_DAMAGE, new object[] {identity.id, -damage});
        }
        
    }
}