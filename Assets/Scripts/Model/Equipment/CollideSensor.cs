using Model.Equipment.Template;

namespace Model.Equipment {
    public abstract class CollideBaseSensor : BaseSensor {
        public float health { get; set; }
        private readonly CollideSensorTemplate collideTemplate;

        public CollideBaseSensor(int id, CollideSensorTemplate template) : base(id, template) {
            collideTemplate = template;
            health = template.maxHealth;
        }
    }
}