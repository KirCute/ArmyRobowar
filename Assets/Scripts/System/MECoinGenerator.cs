using Photon.Pun;
using UnityEngine;

namespace System {
    public class MECoinGenerator : MonoBehaviourPun {
        private const int ROBOT_LAYER = 6;
        private const int PICKABLE_LAYER = 8;
        private const double GENERATE_TIME_INTERVAL = 300.0;

        [SerializeField] private int value;
        private double lastGenerate;

        private void OnEnable() {
            Events.AddListener(Events.F_GAME_START, OnGameStart);
        }

        private void OnGameStart(object[] args) {
            lastGenerate = (double) args[0];
            Events.RemoveListener(Events.F_GAME_START, OnGameStart);
        }

        private void Update() {
            if (Summary.isGameStarted && PhotonNetwork.Time - lastGenerate >= GENERATE_TIME_INTERVAL && photonView.IsMine) {
                Events.Invoke(Events.M_CREATE_PICKABLE_COINS, new object[] {value, transform.position});
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