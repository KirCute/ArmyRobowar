namespace Model.Equipment {
    public class Basement {
        private const int BASE_MAX_HEALTH = 60;
        
        public readonly int id;
        public int health { get; set; }  // client-server

        public Basement(int id) {
            this.id = id;
            this.health = BASE_MAX_HEALTH;
        }
    }
}