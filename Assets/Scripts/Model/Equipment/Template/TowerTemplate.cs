namespace Model.Equipment.Template {
    /// <summary>
    /// 信号塔模板类
    /// </summary>
    public class TowerTemplate {
        public readonly string nameOnTechnologyTree;
        public readonly string name;
        public readonly string prefabName;
        public readonly int maxHealth;
        public readonly int cost;

        public TowerTemplate(string technic, string name, string prefabName, int maxHealth, int cost) {
            this.nameOnTechnologyTree = technic;
            this.name = name;
            this.prefabName = prefabName;
            this.maxHealth = maxHealth;
            this.cost = cost;
        }
    }
}