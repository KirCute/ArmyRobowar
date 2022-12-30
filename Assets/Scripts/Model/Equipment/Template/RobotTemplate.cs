namespace Model.Equipment.Template {
    /// <summary>
    /// 机器人模板类
    /// </summary>
    public class RobotTemplate {
        public readonly string nameOnTechnologyTree;
        public readonly string prefabName;
        public readonly string name;
        public readonly int capacity; // 机器人可携带部件的最大数量
        public readonly int maxHealth;
        public readonly int cost;
        public readonly float makingTime;

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