namespace Equipment.Sensor {
    /// <summary>
    /// 当配件被击中时生效，提供配件的扣血方式
    /// </summary>
    public class MTComponentHurt : AbstractMTHurt {
        private MEComponentIdentifier identity;
        
        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        public override void Hurt(int damage, int team) {
            if (identity.team == team) return;
            if (enabled) Events.Invoke(Events.M_COMPONENT_DAMAGE, new object[] {identity.robotId, identity.index, -damage});
        }
    }
}