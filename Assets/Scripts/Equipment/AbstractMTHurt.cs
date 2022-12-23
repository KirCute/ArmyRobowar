using Photon.Pun;
using UnityEngine;

namespace Equipment {
    public abstract class AbstractMTHurt : MonoBehaviourPun {
        public abstract void Hurt(int damage);
    }
}