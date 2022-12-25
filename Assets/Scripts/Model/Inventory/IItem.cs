namespace Model.Inventory {
    public interface IItem {
        void StoreIn();
        
        bool isPickable { get; set; } 
    }
}