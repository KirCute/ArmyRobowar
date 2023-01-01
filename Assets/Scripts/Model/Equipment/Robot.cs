using System.Collections.Generic;
using Model.Equipment.Template;
using Model.Inventory;
using Photon.Pun;
using Photon.Realtime;

namespace Model.Equipment {
    public class Robot {
        public const byte STATUS_ACTIVE = 0;
        public const byte STATUS_MISSING = 1;
        public const byte STATUS_MANUFACTURING = 2;

        public readonly int id;
        public readonly string name;
        public readonly double createTime;
        public readonly RobotTemplate template;
        public readonly Sensor[] equippedComponents;  // peer-to-peer
        public readonly List<IItem> inventory;  // peer-to-peer
        public bool atHome { get; set; }  // peer-to-peer
        public int atBase { get; set; }  // peer-to-peer
        public int maxHealth { get; set; }  // peer-to-peer
        public int health { get; set; }  // client-server
        public int connection { get; set; }  // client-server
        public bool manufacturing { get; set; }  // peer-to-peer
        public bool allowBuild { get; set; }  // peer-to-peer
        public int inventoryCapacity { get; set; }  // peer-to-peer
        public Player controller { get; set; }  // client-server
        public double lastRecoveryTime { get; set; }  // peer-to-peer
        public byte status => manufacturing ? STATUS_MANUFACTURING : (connection > 0 ? STATUS_ACTIVE : STATUS_MISSING);

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
            this.lastRecoveryTime = PhotonNetwork.Time;
            this.controller = null;
        }
    }
}