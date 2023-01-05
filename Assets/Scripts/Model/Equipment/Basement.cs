namespace Model.Equipment {
    /// <summary>
    /// 基地
    /// </summary>
    public class Basement {
        public const int BASE_MAX_HEALTH = 100;
        
        public readonly int id;
        public int health { get; set; }  // client-server，血量

        public Basement(int id) {
            this.id = id;
            this.health = BASE_MAX_HEALTH;
        }
    }
}