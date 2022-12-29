using Photon.Pun;

namespace System.Pickable {
    public class MTPickableSensor : AbstractMTPickable, IPunObservable {
        private const double PICKABLE_SENSOR_DEAD_TIME = 180.0;
        public string nameOnTechnologyTree;
        public int health;
        private bool picked;
        private MEPickableIdentifier identity;
        public override string name => Constants.SENSOR_TEMPLATES[nameOnTechnologyTree].name;

        private void Awake() {
            identity = GetComponentInParent<MEPickableIdentifier>();
            nameOnTechnologyTree = (string) photonView.InstantiationData[1];
            health = (int) photonView.InstantiationData[2];
            GetComponent<MEPickableDestroyer>().deadTime = PhotonNetwork.Time + PICKABLE_SENSOR_DEAD_TIME;
        }

        public override void Pickup(int team, int robotId) {
            if (picked) return;
            picked = true;
            Events.Invoke(Events.F_ROBOT_ACQUIRE_COMPONENT, new object[] {team, robotId, nameOnTechnologyTree, health});
            Events.Invoke(Events.F_PICKABLE_PICKED, new object[] {identity.id});
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(nameOnTechnologyTree);
                stream.SendNext(health);
                stream.SendNext(picked);
            } else {
                nameOnTechnologyTree = (string) stream.ReceiveNext();
                health = (int) stream.ReceiveNext();
                picked = (bool) stream.ReceiveNext();
            }
        }
        
    }
}