using UnityEngine;

namespace Model.Inventory {
    public interface IItem {
        public string name { get; }

        public void StoreIn();

        public void DropAt(Vector3 pos);
    }
}