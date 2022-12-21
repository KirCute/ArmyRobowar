namespace Model.Equipment.Template {
    /// <summary>
    /// 部件模板基类
    /// </summary>
    public abstract class CollideSensorTemplate : SensorTemplate {
        public float maxHealth { get; set; } // 部件的最大血量

        public CollideSensorTemplate(float maxHealth) {
            this.maxHealth = maxHealth;
        }

        public CollideSensorTemplate(CollideSensorTemplate other) {
            this.maxHealth = other.maxHealth;
        }
    }
}