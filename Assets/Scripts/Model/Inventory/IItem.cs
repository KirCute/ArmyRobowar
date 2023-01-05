using UnityEngine;

namespace Model.Inventory {
    /// <summary>
    /// 所有物品的接口
    /// </summary>
    public interface IItem {
        public string name { get; }  // 物品的名称

        public void StoreIn();  // 物品如何被卸货

        public void DropAt(Vector3 pos);  // 物品如何掉落成为掉落物
    }
}