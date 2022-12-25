using System.Collections.Generic;
using Model.Equipment.Template;
using Model.Inventory;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Model.Equipment {
    public class Robot {
        public const byte STATUS_ACTIVE = 0;
        public const byte STATUS_MISSING = 1;
        public const byte STATUS_MANUFACTURING = 2;

        public GameObject gameObject { get; set; }
        public readonly string name;
        public int maxHealth { get; set; }
        public int health { get; set; }

        public byte status => manufacturing ? STATUS_MANUFACTURING : (connection > 0 ? STATUS_ACTIVE : STATUS_MISSING);
        public readonly RobotTemplate template;
        public readonly Sensor[] equippedComponents;
        public readonly List<IItem> inventory;
        public int connection { get; set; }
        public bool manufacturing { get; set; }
        public double createTime { get; set; }
        public int inventoryCapacity { get; set; }
        public Player controller { get; set; }
        public readonly int id;

        public Robot(int id, string name, RobotTemplate template) {
            this.id = id;
            this.name = name;
            this.maxHealth = template.maxHealth;
            this.health = maxHealth;
            this.template = template;
            this.equippedComponents = new Sensor[template.capacity];
            this.inventory = new List<IItem>();
            this.inventoryCapacity = 0;
            this.connection = 0;
            this.manufacturing = true;
            this.createTime = PhotonNetwork.Time;
            this.controller = null;
        }
    }
}