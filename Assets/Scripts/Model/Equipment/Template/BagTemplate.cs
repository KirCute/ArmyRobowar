namespace Model.Equipment.Template {
    public class BagTemplate : SensorTemplate {
        public readonly int capacity;

        public BagTemplate(string name, string description, int capacity) : base(name, description) {
            this.capacity = capacity;
        }

        public override BaseSensor CreateOne(int id) {
            return new Bag(id, this);
        }
    }
}