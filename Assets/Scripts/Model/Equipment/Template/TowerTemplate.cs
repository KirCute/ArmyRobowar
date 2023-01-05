namespace Model.Equipment.Template {
    /// <summary>
    /// 信号塔模板类
    /// </summary>
    public class TowerTemplate {
        public readonly string nameOnTechnologyTree;  // 科技树上的名称
        public readonly string name;  // 显示名称
        public readonly string prefabName;  // 预制体名称
        public readonly int maxHealth;  // 最大血量
        public readonly int cost;  // 价格

        public TowerTemplate(string technic, string name, string prefabName, int maxHealth, int cost) {
            this.nameOnTechnologyTree = technic;
            this.name = name;
            this.prefabName = prefabName;
            this.maxHealth = maxHealth;
            this.cost = cost;
        }
    }
}