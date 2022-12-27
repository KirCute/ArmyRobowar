namespace Equipment.Basement.Diamond {
    public class MTBaseHurt : AbstractMTHurt {
        private MEBaseFlag identity;
        
        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }

        public override void Hurt(int damage, int team) {
            if (identity.flagColor == -1 || identity.flagColor == team) return;
            if (enabled) Events.Invoke(Events.M_BASE_DAMAGE, new object[] {identity.baseId, -damage});
        }
    }
}