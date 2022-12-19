namespace Model.Equipment.Template {
    /// <summary>
    /// 机器人模板类
    /// </summary>
    public class RobotTemplate {
        public int capacity { get; set; } // 机器人可携带部件的最大数量

        public RobotTemplate(int capacity) {
            this.capacity = capacity;
        }

        public RobotTemplate(RobotTemplate other) {
            this.capacity = other.capacity;
        }
    }
}