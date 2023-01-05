using Photon.Pun;

namespace Equipment {
    /// <summary>
    /// 所有能赋予他人信号的设备的身份识别脚本基类
    /// </summary>
    public abstract class AbstractMESignalIdentifier : MonoBehaviourPun {
        /// <summary>
        /// 返回所属团队
        /// </summary>
        /// <returns>所属团队号</returns>
        public abstract int GetTeamId();
    }
}