using Model.Equipment.Template;
using UnityEngine;

namespace Model.Equipment {
    public class Sensor {
        public delegate bool EquipDelegate(Sensor self, GameObject robot, int robotId, int componentIndex);
        
        public readonly SensorTemplate template;
        public int health { get; set; }
        
        public Sensor(SensorTemplate template) {
            this.template = template;
            this.health = template.maxHealth;
        }

        public bool OnEquipped(GameObject robot, int robotId, int componentIndex) {
            return template.onEquipped(this, robot, robotId, componentIndex);
        }

        public bool OnUnloaded(GameObject robot, int robotId, int componentIndex) {
            return template.onUnloaded(this, robot, robotId, componentIndex);
        }
    }
}