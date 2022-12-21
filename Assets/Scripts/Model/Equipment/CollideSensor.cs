using Model.Equipment.Template;

namespace Model.Equipment {
    public abstract class CollideBaseSensor<T> : BaseSensor<T> where T : CollideSensorTemplate {
        public float health { get; set; }

        public CollideBaseSensor(int id, CollideSensorTemplate template) : base(id) {
            health = template.maxHealth;
        }
    }
}