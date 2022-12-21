using Photon.Pun;
using UnityEngine;

namespace System {
    public class MECoinGenerator : MonoBehaviourPun {
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
            if (PhotonNetwork.Time - lastGenerate >= GENERATE_TIME_INTERVAL && photonView.IsMine) {
                PhotonNetwork.Instantiate(generatePrefabName, transform.position, transform.rotation);
                lastGenerate = PhotonNetwork.Time;
            }
        }

        private void OnTriggerStay(Collider other) {
            lastGenerate = PhotonNetwork.Time;
        }
    }
}