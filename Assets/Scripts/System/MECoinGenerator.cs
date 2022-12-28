using Photon.Pun;
using UnityEngine;

namespace System {
    public class MECoinGenerator : MonoBehaviourPun {
        private const int PICKABLE_LAYER = 10;
        private const double GENERATE_TIME_INTERVAL = 120.0;
        private static Random rand;
        
        [SerializeField] private int value;
        private double lastGenerate;

        private void OnEnable() {
            Events.AddListener(Events.F_GAME_START, OnGameStart);
        }

        private void OnGameStart(object[] args) {
            if (photonView.IsMine) {
                rand ??= new Random(Guid.NewGuid().GetHashCode());
                lastGenerate = (double) args[0] - rand.NextDouble() * GENERATE_TIME_INTERVAL;
            }

            Events.RemoveListener(Events.F_GAME_START, OnGameStart);
        }

        private void Update() {
            if (Summary.isGameStarted && photonView.IsMine && PhotonNetwork.Time - lastGenerate >= GENERATE_TIME_INTERVAL) {
                Events.Invoke(Events.M_CREATE_PICKABLE_COINS, new object[] {value, transform.position});
                lastGenerate = PhotonNetwork.Time;
            }
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.layer is PICKABLE_LAYER) {
                lastGenerate = PhotonNetwork.Time;
            }
        }
    }
}