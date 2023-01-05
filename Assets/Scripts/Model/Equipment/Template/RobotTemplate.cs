namespace Model.Equipment.Template {
    /// <summary>
    /// 机器人模板类
    /// </summary>
    public class RobotTemplate {
        public readonly string nameOnTechnologyTree;  // 在科技树上的代号（key）
        public readonly string prefabName;  // 预制体名
        public readonly string name;  // 显示名称
        public readonly int capacity; // 机器人可携带部件的最大数量
        public readonly int maxHealth;  // 初始最大血量
        public readonly int cost;  // 价格
        public readonly float makingTime;  // 生产耗时

        public RobotTemplate(string technic, string name, string prefabName, int capacity, int maxHealth, int cost, float makingTime) {
            this.nameOnTechnologyTree = technic;
            this.name = name;
            this.prefabName = prefabName;
            this.capacity = capacity;
            this.cost = cost;
            this.maxHealth = maxHealth;
            this.makingTime = makingTime;
        }
    }
}