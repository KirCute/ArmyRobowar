using Photon.Pun;

namespace Equipment {
    /// <summary>
    /// 所有能连接信号的设备的连接器的基类
    /// </summary>
    public abstract class AbstractMTConnection : MonoBehaviourPun {
        /// <summary>
        /// 附加信号的方法
        /// </summary>
        /// <param name="signal">附加的信号，注意可正可负</param>
        public abstract void PlusSignal(int signal);
        /// <summary>
        /// 得到所属团队
        /// </summary>
        /// <returns>所属团队号</returns>
        public abstract int GetTeamId();
    }
}