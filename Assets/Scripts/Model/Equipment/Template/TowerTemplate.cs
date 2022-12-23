namespace Model.Equipment.Template {
    /// <summary>
    /// 信号塔模板类
    /// </summary>
    public class TowerTemplate {
        public readonly string nameOnTechnologyTree;
        public readonly float range; // 信号塔的照射距离

        public TowerTemplate(string name, float range) {
            this.nameOnTechnologyTree = name;
            this.range = range;
        }
    }
}