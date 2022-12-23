namespace Model.Equipment.Template {
    /// <summary>
    /// 机器人模板类
    /// </summary>
    public class RobotTemplate {
        public readonly string nameOnTechnologyTree;
        public readonly int capacity; // 机器人可携带部件的最大数量

        public RobotTemplate(string name, int capacity) {
            this.nameOnTechnologyTree = name;
            this.capacity = capacity;
        }

        public Robot CreateOne(int id, string name) {
            return new Robot(id, name, this);
        }
    }
}