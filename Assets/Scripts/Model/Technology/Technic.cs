namespace Model.Technology {
    public class Technic {
        public delegate void UnlockDelegate(Team team);
        
        public readonly string name;
        public readonly string description;
        public readonly double cost;
        public readonly UnlockDelegate Unlock;

        public Technic(string name, string description, double cost, UnlockDelegate unlockFunc) {
            this.name = name;
            this.description = description;
            this.cost = cost;
            this.Unlock = unlockFunc;
        }
    }
}