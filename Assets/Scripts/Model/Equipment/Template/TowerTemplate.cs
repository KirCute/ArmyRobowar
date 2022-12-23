namespace Model.Equipment.Template {
    /// <summary>
    /// 信号塔模板类
    /// </summary>
    public class TowerTemplate {
        public readonly string nameOnTechnologyTree;
        public readonly string pprefabName;
        public readonly int maxHealth;

        public TowerTemplate(string name, string prefabName, int maxHealth) {
            this.nameOnTechnologyTree = name;
            this.pprefabName = prefabName;
            this.maxHealth = maxHealth;
        }
    }
}