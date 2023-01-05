using Model.Equipment.Template;

namespace Model.Equipment {
    /// <summary>
    /// 传感器
    /// </summary>
    public class Sensor {
        // 安装和卸载方式，安装和卸载时相应函数会在本队所有队员的计算机上被调用，参数sensor总为this，只有队长调用时processObject为true，可用其判断是否构造游戏物体
        public delegate void EquipDelegate(Sensor self, int robotId, int instIndex, bool processObject);

        public readonly SensorTemplate template;  // 模板
        public int health { get; set; }  // client-server，血量

        public Sensor(SensorTemplate template, int health) {
            this.template = template;
            this.health = health;
        }

        public Sensor(SensorTemplate template) : this(template, template.maxHealth) { }

        public void OnEquipped(int robotId, int instIndex, bool processObject) {
            template.onEquipped(this, robotId, instIndex, processObject);
        }

        public void OnUnloaded(int robotId, int instIndex, bool processObject) {
            template.onUnloaded(this, robotId, instIndex, processObject);
        }
    }
}