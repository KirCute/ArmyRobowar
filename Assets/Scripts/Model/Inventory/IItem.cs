namespace Model.Inventory {
    public interface IItem {
        public string name { get; }

        public void StoreIn();
    }
}