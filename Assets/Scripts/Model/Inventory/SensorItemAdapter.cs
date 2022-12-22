using System;
using Model.Equipment;
using Model.Equipment.Template;

namespace Model.Inventory {
    public class SensorItemAdapter : IItem {
        private readonly string name;
        private readonly float health;

        public SensorItemAdapter(string name, float health) {
            this.name = name;
            this.health = health;
        }

        public SensorItemAdapter(Sensor sensor) : this(sensor.template.nameOnTechnologyTree, sensor.health) { }

        public void StoreIn() {
            Events.Invoke(Events.F_TEAM_ACQUIRE_COMPONENT, new object[] {
                Summary.team.teamColor, name, health
            });
        }
    }
}