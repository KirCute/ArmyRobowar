using Model.Equipment.Template;

namespace Model.Equipment {
    public class Sensor {
        public delegate void EquipDelegate(Sensor self, int robotId, int instIndex, bool processObject);

        public readonly SensorTemplate template;
        public int health { get; set; }  // client-server

        public Sensor(SensorTemplate template, int health) {
            this.template = template;
            this.health = health;
        }

        public Sensor(SensorTemplate template) : this(template, template.maxHealth) { }

        public void OnEquipped(int robotId, int instIndex, bool processObject) {
            template.onEquipped(this, robotId, instIndex, processObject);
        }

        public void OnUnloaded(int robotId, int instIndex, bool processObject) {
            template.onUnloaded(this, robotId, instIndex, processObject);
        }
    }
}