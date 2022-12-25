using System.Collections.Generic;
using Model.Inventory;
using Photon.Pun;
using UnityEngine;

namespace System {
    /// <summary>
    /// 拾取掉落物
    /// </summary>
    public class MEPickUpDropped : MonoBehaviourPun {
        private List<GameObject> transportNear = new List<GameObject>();

        private void OnTriggerEnter(Collider other) {
            //TODO,如果是运输车
            transportNear.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other) {
            //TODO
            transportNear.Remove(other.gameObject);
        }

        private void OnMouseDown() {
            if (transportNear.Count > 0 && GetComponent<IItem>().isPickable) {
                IItem dropped = GetComponent<IItem>();
                dropped.StoreIn();
            }
        }
    }
}