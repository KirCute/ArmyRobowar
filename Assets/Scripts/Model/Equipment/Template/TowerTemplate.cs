namespace Model.Equipment.Template {
    /// <summary>
    /// 信号塔模板类
    /// </summary>
    public class TowerTemplate {
        public float range { get; set; } // 信号塔的照射距离

        public TowerTemplate(float range) {
            this.range = range;
        }

        public TowerTemplate(TowerTemplate other) {
            this.range = other.range;
        }
    }
}