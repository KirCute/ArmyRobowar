using System;
using System.Numerics;
using Photon.Pun;

namespace Model.Inventory {
    public class CoinPickUp : MonoBehaviourPun, IItem{
        private readonly int value;

        public CoinPickUp(int value) {
            this.value = value;
        }
        
        public void StoreIn() {
            Events.Invoke(Events.F_TEAM_ACQUIRE_COINS, new object[] {Summary.team.teamColor, value});
        }
        
        /// <summary>
        /// 界面处理的事件，在地图中某处产生一定数目的金币(每个一段时间产生一点）
        /// </summary>
        /// <param name="position">产生金币的位置</param>
        /// <param name="totalAmount">该处生产金币总数</param>
        public void ProduceCoins(Vector3 position,int totalAmount) {
            //Events.Invoke(Events.M_PRODUCE_COINS, new object[] { position, totalAmount });
        }
    }
}