namespace Model.Equipment.Template {
    public class BagTemplate : SensorTemplate {
        public int capacity { get; set; }

        public BagTemplate(BagTemplate other) {
            this.capacity = other.capacity;
        }
    }
}