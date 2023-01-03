using Photon.Pun;

namespace Equipment {
    public abstract class AbstractMTConnection : MonoBehaviourPun {
        public abstract void PlusSignal(int signal);
        public abstract int GetTeamId();
    }
}