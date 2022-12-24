namespace Model.Equipment.Template {
    public class SensorTemplate {
        public readonly string nameOnTechnologyTree;
        public readonly string name;
        public readonly string description;
        public readonly int maxHealth;
        public readonly Sensor.EquipDelegate onEquipped;
        public readonly Sensor.EquipDelegate onUnloaded;
        
        public SensorTemplate(string technic, string name, string description, int maxHealth,
            Sensor.EquipDelegate onEquipped, Sensor.EquipDelegate onUnloaded) {
            this.nameOnTechnologyTree = technic;
            this.name = name;
            this.description = description;
            this.maxHealth = maxHealth;
            this.onEquipped = onEquipped;
            this.onUnloaded = onUnloaded;
        }

        public Sensor CreateOne() {
            return new Sensor(this);
        }
    }
}