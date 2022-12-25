using Photon.Pun;
using UnityEngine;

namespace System {
    public class MECoinGenerator : MonoBehaviourPun {
        private const int ROBOT_LAYER = 6;
        private const int PICKABLE_LAYER = 8;
        private const double GENERATE_TIME_INTERVAL = 300.0;

        [SerializeField] private string generatePrefabName;
        [SerializeField] private bool generateAtFirst;
        private double lastGenerate;

        private void Start() {
            if (!generateAtFirst) {
                lastGenerate = PhotonNetwork.Time;
            }
        }

        private void Update() {
            if (PhotonNetwork.Time - lastGenerate >= GENERATE_TIME_INTERVAL && photonView.IsMine &&
                generatePrefabName.Length > 0) {
                PhotonNetwork.Instantiate(generatePrefabName, transform.position, transform.rotation);
                lastGenerate = PhotonNetwork.Time;
            }
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.layer is ROBOT_LAYER or PICKABLE_LAYER) {
                lastGenerate = PhotonNetwork.Time;
            }
        }
    }
}