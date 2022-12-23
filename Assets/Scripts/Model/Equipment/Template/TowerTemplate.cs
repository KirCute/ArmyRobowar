namespace Model.Equipment.Template {
    /// <summary>
    /// 信号塔模板类
    /// </summary>
    public class TowerTemplate {
        public readonly float range; // 信号塔的照射距离

        public TowerTemplate(float range) {
            this.range = range;
        }
    }
}