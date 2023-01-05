using Photon.Pun;

namespace System.Pickable {
    /// <summary>
    /// 一切掉落物的基类
    /// </summary>
    public abstract class AbstractMTPickable : MonoBehaviourPun {
        public abstract string pickableName { get; }  // 掉落物的名称
        public abstract void Pickup(int team, int robotId);  // 掉落物如何被捡起
    }
}