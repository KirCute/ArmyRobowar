using Photon.Pun;

namespace Equipment.Sensor {
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