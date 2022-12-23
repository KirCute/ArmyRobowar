using System;
using System.Collections.Generic;
using Model.Equipment.Template;
using Model.Inventory;
using UnityEngine;

namespace Model.Equipment {
    public class Robot {
        public const byte STATUS_ACTIVE = 0;
        public const byte STATUS_MISSING = 1;
        private const byte DEFAULT_ROBOT_HEALTH = 10;

        public GameObject gameObject { get; set; }
        public readonly string name;
        public float maxHealth { get; set; }
        public float health { get; set; }

        public byte status => connection > 0 ? STATUS_ACTIVE : STATUS_MISSING;
        public IReadOnlyList<Sensor> equippedComponents => components;
        public IReadOnlyList<IItem> inventory => writableInventory;
        public int connection { get; set; }
        public int inventoryCapacity { get; set; }
        public readonly List<IItem> writableInventory;
        private readonly Sensor[] components;
        public readonly int id;

        public Robot(int id, string name, RobotTemplate template) {
            this.id = id;
            this.name = name;
            this.maxHealth = DEFAULT_ROBOT_HEALTH;
            this.health = maxHealth;
            this.components = new Sensor[template.capacity];
            this.writableInventory = new List<IItem>();
            this.inventoryCapacity = 0;
        }
    }
}