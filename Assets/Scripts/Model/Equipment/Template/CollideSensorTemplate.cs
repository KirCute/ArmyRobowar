namespace Model.Equipment.Template {
    /// <summary>
    /// 部件模板基类
    /// </summary>
    public abstract class CollideSensorTemplate : SensorTemplate {
        public readonly float maxHealth; // 部件的最大血量

        public CollideSensorTemplate(string name, string description, float maxHealth) : base(name, description) {
            this.maxHealth = maxHealth;
        }
    }
}