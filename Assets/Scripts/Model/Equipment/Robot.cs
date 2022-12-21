using Model.Equipment.Template;
using UnityEngine;

namespace Model.Equipment {
    public class Robot {
        public const byte STATUS_ACTIVE = 0;
        public const byte STATUS_MISSING = 1;
        private const byte DEFAULT_ROBOT_HEALTH = 10;

        public RobotTemplate template { get; set; }
        public GameObject gameObject { get; set; }
        public string name { get; set; }
        public float maxHealth { get; set; }
        public float health { get; set; }
        public byte status { get; set; }
        public readonly int id;

        public Robot(int id, string name, RobotTemplate template) {
            this.id = id;
            this.name = name;
            this.template = template;
            this.maxHealth = DEFAULT_ROBOT_HEALTH;
            this.health = maxHealth;
            this.status = STATUS_ACTIVE;
        }
    }
}

