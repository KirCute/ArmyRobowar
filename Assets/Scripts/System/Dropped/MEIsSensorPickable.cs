using System.Collections.Generic;
using Model.Inventory;
using Photon.Pun;

namespace System {
    public class MTIsSensorPickable : MonoBehaviourPun,IPunObservable {
        private List<IItem> inventories;
        private void OnEnable() {
            Events.AddListener(Events.F_BODY_DESTROYED, SensorPickable);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_BODY_DESTROYED, SensorPickable);
        }

        public void SensorPickable(object[] args) {
            inventories =  Summary.team.robots[(int)args[0]].inventory;
            foreach (var inventory in inventories) {
                inventory.isPickable = true;
            }
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(inventories);
            }
            else {
                inventories = (List<IItem>)stream.ReceiveNext();
            }
        }
    }
}