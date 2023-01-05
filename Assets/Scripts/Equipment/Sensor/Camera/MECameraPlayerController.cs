using Photon.Pun;

namespace Equipment.Sensor.Camera {
    /// <summary>
    /// 用于控制Camera组件是否启用
    /// </summary>
    public class MECameraPlayerController : MonoBehaviourPun {
        private MEComponentIdentifier identity;
        private bool viewing;

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_MONITOR, OnMonitored);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_MONITOR, OnMonitored);
        }

        private void OnMonitored(object[] args) {
            if (identity.robotId == (int) args[0] && PhotonNetwork.LocalPlayer.Equals(args[1])) {
                viewing = (bool) args[2];
                GetComponent<UnityEngine.Camera>().enabled = viewing;
            }
        }

        private void OnDestroy() {
            if (viewing) {  // 让客户端停止观察小车画面
                Events.Invoke(Events.M_ROBOT_MONITOR, new object[] {identity.robotId, PhotonNetwork.LocalPlayer, false});
            }
        }
    }
}