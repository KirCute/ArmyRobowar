using Photon.Pun;

namespace System {
    public class MEAcquireCoins : MonoBehaviourPun{
        private void OnEnable() {
            Events.AddListener(Events.F_TEAM_ACQUIRE_COINS, AcquireCoins);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_TEAM_ACQUIRE_COINS, AcquireCoins);
        }

        public void AcquireCoins(object[] args) {
            //TODO,在背包里面添加
        }
    }
}