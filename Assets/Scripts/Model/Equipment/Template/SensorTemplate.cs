namespace Model.Equipment.Template {
    public class SensorTemplate {
        public readonly string nameOnTechnologyTree;
        public readonly string prefabName;
        public readonly string name;
        public readonly string description;
        public readonly int maxHealth;
        public readonly int cost;
        public readonly Sensor.EquipDelegate onEquipped;
        public readonly Sensor.EquipDelegate onUnloaded;
        
        public SensorTemplate(string technic, string prefabName, string name, string description, int maxHealth,
            int cost, Sensor.EquipDelegate onEquipped, Sensor.EquipDelegate onUnloaded) {
            this.nameOnTechnologyTree = technic;
            this.prefabName = prefabName;
            this.name = name;
            this.description = description;
            this.maxHealth = maxHealth;
            this.cost = cost;
            this.onEquipped = onEquipped;
            this.onUnloaded = onUnloaded;
        }

        public Sensor CreateOne() {
            return new Sensor(this);
        }
    }
}