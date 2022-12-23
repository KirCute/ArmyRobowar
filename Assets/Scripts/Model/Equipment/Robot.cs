using System.Collections.Generic;
using Model.Inventory;
using UnityEngine;

namespace Model.Equipment {
    public class Robot {
        public const byte STATUS_ACTIVE = 0;
        public const byte STATUS_MISSING = 1;
        public const byte STATUS_MANUFACTURING = 2;
        private const byte DEFAULT_ROBOT_HEALTH = 10;

        public GameObject gameObject { get; set; }
        public readonly string name;
        public int maxHealth { get; set; }
        public int health { get; set; }

        public byte status => manufacturing ? STATUS_MANUFACTURING : (connection > 0 ? STATUS_ACTIVE : STATUS_MISSING);
        public readonly Sensor[] equippedComponents;
        public readonly List<IItem> inventory;
        public int connection { get; set; }
        public bool manufacturing { get; set; }
        public int inventoryCapacity { get; set; }
        public readonly int id;

        public Robot(int id, string name, int capacity) {
            this.id = id;
            this.name = name;
            this.maxHealth = DEFAULT_ROBOT_HEALTH;
            this.health = maxHealth;
            this.equippedComponents = new Sensor[capacity];
            this.inventory = new List<IItem>();
            this.inventoryCapacity = 0;
        }
    }
}