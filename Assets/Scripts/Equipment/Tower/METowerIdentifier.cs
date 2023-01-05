using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Tower {
    /// <summary>
    /// 用于记录信号塔ID和其所属团队，从而可以为正确的机器人附加信号
    /// </summary>
    public class METowerIdentifier : AbstractMESignalIdentifier, IPunObservable {
        [SerializeField] private List<Material> projectorMaterials;
        public int id { get; private set; }
        public int team { get; private set; }

        private void Awake() {
            id = (int) photonView.InstantiationData[0];
            team = (int) photonView.InstantiationData[1];
            gameObject.name = $"Tower_{id}";
            var materials = GetComponent<MeshRenderer>().materials;
            materials[1] = projectorMaterials[team];
            materials[3] = projectorMaterials[team];
            GetComponent<MeshRenderer>().materials = materials;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(id);
                stream.SendNext(team);
            } else {
                id = (int) stream.ReceiveNext();
                team = (int) stream.ReceiveNext();
            }
        }

        public override int GetTeamId() {
            return team;
        }
    }
}