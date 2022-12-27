using System;

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
    }
}