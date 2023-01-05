using Photon.Pun;
using UnityEngine;

namespace System {
    /// <summary>
    /// 掉落的货币生成点脚本
    /// </summary>
    public class MECoinGenerator : MonoBehaviourPun {
        private const int PICKABLE_LAYER = 10;
        private const double GENERATE_TIME_INTERVAL = 120.0;  // 生成周期
        private static Random rand;
        
        [SerializeField] private string prefabName;  // 预制体名
        private double lastGenerate;  // 上次生成时间

        private void OnEnable() {
            if (photonView.IsMine) Events.AddListener(Events.F_GAME_START, OnGameStart);
        }

        private void OnGameStart(object[] args) {
            rand ??= new Random(Guid.NewGuid().GetHashCode());
            lastGenerate = (double) args[0] - rand.NextDouble() * GENERATE_TIME_INTERVAL;  // 首枚货币选在0至GENERATE_TIME_INTERVAL之间随机的一个时间生成
            Events.RemoveListener(Events.F_GAME_START, OnGameStart);
        }

        private void Update() {
            if (Summary.isGameStarted && photonView.IsMine && PhotonNetwork.Time - lastGenerate >= GENERATE_TIME_INTERVAL) {  // 时机已到，生成
                Events.Invoke(Events.M_CREATE_PICKABLE_COINS, new object[] {prefabName, transform.position});
                lastGenerate = PhotonNetwork.Time;
            }
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.layer is PICKABLE_LAYER) {  // 只要生成的货币没被捡走，就永远不再次生成
                lastGenerate = PhotonNetwork.Time;
            }
        }
    }
}