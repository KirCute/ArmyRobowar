using Photon.Pun;

namespace Equipment {
    public abstract class AbstractMTConnection : MonoBehaviourPun {
        public abstract void PlusSignal(int signal, int team);
    }
}