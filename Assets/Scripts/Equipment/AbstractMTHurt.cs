using Photon.Pun;

namespace Equipment {
    public abstract class AbstractMTHurt : MonoBehaviourPun {
        public abstract void Hurt(int damage, int team);
    }
}