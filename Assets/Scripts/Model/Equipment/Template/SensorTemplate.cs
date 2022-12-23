namespace Model.Equipment.Template {
    public abstract class SensorTemplate {
        public readonly string name;
        public readonly string description;

        public SensorTemplate(string name, string description) {
            this.name = name;
            this.description = description;
        }
        
        public abstract BaseSensor CreateOne(int id);
    }
}