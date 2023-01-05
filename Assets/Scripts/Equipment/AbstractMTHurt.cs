using Photon.Pun;

namespace Equipment {
    /// <summary>
    /// 所有能受伤的设备，Hurt脚本的基类
    /// </summary>
    public abstract class AbstractMTHurt : MonoBehaviourPun {
        /// <summary>
        /// 造成伤害方法
        /// </summary>
        /// <param name="damage">伤害量</param>
        /// <param name="team">伤害来源队伍号，用于过滤友伤</param>
        public abstract void Hurt(int damage, int team);
    }
}