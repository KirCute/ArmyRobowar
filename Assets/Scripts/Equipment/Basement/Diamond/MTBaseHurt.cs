namespace Equipment.Basement.Diamond {
    public class MTBaseHurt : AbstractMTHurt {
        private MEBaseFlag identity;
        
        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }

        public override void Hurt(int damage) {
            if (enabled) Events.Invoke(Events.M_BASE_CHANGE_HEALTH, new object[] {identity.baseId, -damage});
        }
    }
}