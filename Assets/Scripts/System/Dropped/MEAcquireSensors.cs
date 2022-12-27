using Photon.Pun;

namespace System {
    public class MEAcquireSensors : MonoBehaviourPun{
        private void OnEnable() {
            Events.AddListener(Events.F_TEAM_ACQUIRE_COMPONENT, AcquireComponent);
        }

        private void OnDisable() {
            Events.AddListener(Events.F_TEAM_ACQUIRE_COMPONENT, AcquireComponent);
        }

        public void AcquireComponent(object[] args) {
            //TODO,在背包中添加
        }
    }
}