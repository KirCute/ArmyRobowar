namespace Model.Technology {
    /// <summary>
    /// 科技
    /// </summary>
    public class Technic {
        public delegate void UnlockDelegate(Team team);
        
        public readonly string name;  // 显示名称
        public readonly string description;  // 描述
        public readonly double cost;  // 消耗科技点数
        public readonly UnlockDelegate Unlock;  // 科技被研发时会发生什么

        public Technic(string name, string description, double cost, UnlockDelegate unlockFunc) {
            this.name = name;
            this.description = description;
            this.cost = cost;
            this.Unlock = unlockFunc;
        }
    }
}