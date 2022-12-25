using System;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;


namespace Model.Inventory {
    public class Coin : IItem{
        private readonly int value;

        public Coin(int value) {
            this.value = value;
        }
        
        public void StoreIn() {
            Events.Invoke(Events.F_TEAM_ACQUIRE_COINS, new object[] {Summary.team.teamColor, value});
        }

        public bool isPickable {
            get;
            set;
        } = true;
    }
}