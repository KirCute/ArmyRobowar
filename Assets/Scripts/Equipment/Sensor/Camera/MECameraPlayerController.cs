using Photon.Pun;

namespace Equipment.Sensor.Camera {
    public class MECameraPlayerController : MonoBehaviourPun {
        private MEComponentIdentifier identity;

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
                GetComponent<UnityEngine.Camera>().enabled = (bool) args[2];
            }
        }
    }
}