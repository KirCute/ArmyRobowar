using Model.Equipment.Template;

namespace Model.Equipment {
    public class Tower {
        
        public readonly TowerTemplate template;
        public int health { get; set; }
        
        public Tower(TowerTemplate template) {
            this.template = template;
            this.health = template.maxHealth;
        }
    }
}