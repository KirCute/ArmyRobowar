using System;
using Photon.Pun;

namespace Equipment.Basement {
    /// <summary>
    /// 用于处理基地的占领和摧毁事件，从而可以为其所属的团队服务
    /// </summary>
    public class MEBaseFlag : AbstractMESignalIdentifier {
        public int baseId;
        [NonSerialized] public int flagColor = -1;

        private void OnEnable() {
            Events.AddListener(Events.M_CAPTURE_BASE, OnCapture);
            Events.AddListener(Events.F_BASE_DESTROYED, OnConquered);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_CAPTURE_BASE, OnCapture);
            Events.RemoveListener(Events.F_BASE_DESTROYED, OnConquered);
        }

        private void OnCapture(object[] args) {
            if (baseId == (int) args[0]) {
                CaptureBy((int) args[1]);
            }
        }

        private void OnConquered(object[] args) {
            if (baseId == (int) args[0]) {
                if (flagColor == Summary.team.teamColor) {
                    if (Summary.isTeamLeader) {
                        foreach (var photon in GetComponentsInChildren<PhotonView>()) {
                            photon.TransferOwnership(PhotonNetwork.MasterClient);
                        }
                    }

                    Summary.team.bases.Remove(baseId);
                }

                flagColor = -1;
            }
        }

        public override int GetTeamId() {
            return flagColor;
        }
        
        /// <summary>
        /// 跳过事件系统占领基地，只在开始游戏时使用
        /// </summary>
        /// <param name="team">队伍号</param>
        public void CaptureBy(int team) {
            flagColor = team;
            if (flagColor == Summary.team.teamColor) {
                Summary.team.bases.Add(baseId, new Model.Equipment.Basement(baseId));

                if (Summary.isTeamLeader) {
                    foreach (var photon in GetComponentsInChildren<PhotonView>()) {
                        photon.TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                }
            }
        }
    }
}