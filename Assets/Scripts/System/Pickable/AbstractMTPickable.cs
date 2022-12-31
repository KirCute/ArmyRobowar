using Photon.Pun;

namespace System.Pickable {
    public abstract class AbstractMTPickable : MonoBehaviourPun {
        public abstract string pickableName { get; }
        public abstract void Pickup(int team, int robotId);
    }
}