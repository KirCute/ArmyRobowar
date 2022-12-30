using System;
using UnityEngine;

namespace Model.Inventory {
    public class Coin : IItem{
        private readonly int value;
        public string name => $"金币 * {value}";

        public Coin(int value) {
            this.value = value;
        }
        
        public void StoreIn() {
            Events.Invoke(Events.F_TEAM_ACQUIRE_COINS, new object[] {Summary.team.teamColor, value});
        }

        public void DropAt(Vector3 pos) {
            Events.Invoke(Events.M_CREATE_PICKABLE_COINS,
                new object[] {value >= 30 ? "PickableGoldCoin" : "PickableSilverCoin", pos});
        }
    }
}