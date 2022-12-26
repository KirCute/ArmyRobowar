using System;
using Model.Equipment;
using Model.Equipment.Template;

namespace Model.Inventory {
    public class SensorItemAdapter : IItem {
        private readonly string nameOnTechnologyTree;
        private readonly int health;
        public string name => Constants.SENSOR_TEMPLATES[nameOnTechnologyTree].name;

        public SensorItemAdapter(string nameOnTechnologyTree, int health) {
            this.nameOnTechnologyTree = nameOnTechnologyTree;
            this.health = health;
        }

        public SensorItemAdapter(Sensor sensor) : this(sensor.template.nameOnTechnologyTree, sensor.health) { }

        public void StoreIn() {
            Events.Invoke(Events.F_TEAM_ACQUIRE_COMPONENT, new object[] {
                Summary.team.teamColor, nameOnTechnologyTree, health
            });
        }
    }
}