namespace Model.Equipment.Template {
    /// <summary>
    /// 信号塔模板类
    /// </summary>
    public class TowerTemplate {
        public readonly string nameOnTechnologyTree;
        public readonly string prefabName;
        public readonly int maxHealth;
        public readonly int cost;

        public TowerTemplate(string name, string prefabName, int maxHealth, int cost) {
            this.nameOnTechnologyTree = name;
            this.prefabName = prefabName;
            this.maxHealth = maxHealth;
            this.cost = cost;
        }
    }
}