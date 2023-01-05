using Model.Equipment.Template;

namespace Model.Equipment {
    /// <summary>
    /// 信号塔
    /// </summary>
    public class Tower {
        
        public readonly TowerTemplate template;  // 模板
        public int health { get; set; }  // client-server，血量
        
        public Tower(TowerTemplate template) {
            this.template = template;
            this.health = template.maxHealth;
        }
    }
}