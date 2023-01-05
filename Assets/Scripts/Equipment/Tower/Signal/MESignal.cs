using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Tower.Signal {
    /// <summary>
    /// 当机器人进入胶囊碰撞体时，为机器人附加信号，当机器人离开胶囊碰撞体时，使机器人减少信号
    /// </summary>
    public class MESignal : MonoBehaviourPun {
        private AbstractMESignalIdentifier identity;
        private readonly List<AbstractMTConnection> connecting = new();  // 正在被赋予信号的接收器
        private readonly List<AbstractMTConnection> triggering = new();  // 所有信号范围内的接收器
        // 之前试过用TriggerEnter和TriggerExit写，但是在信号塔损毁的时候会有大问题，故改用逐帧更新
        [SerializeField] private int strength;
        
        private void Awake() {
            identity = GetComponentInParent<AbstractMESignalIdentifier>();
        }

        private void Update() {
            for (var i = 0; i < connecting.Count; i++) {
                if (connecting[i] == null) connecting.RemoveAt(i--);  // 如果有损毁的接收器
                else if (connecting[i].GetTeamId() != identity.GetTeamId() || !triggering.Contains(connecting[i])) {  // 如果有赋予信号的接收器离开信号范围，或者不再属于本队（基地的从属可改变）
                    connecting[i].PlusSignal(-strength);
                    connecting.RemoveAt(i--);
                }
            }

            for (var i = 0; i < triggering.Count; i++) {
                if (triggering[i] == null) triggering.RemoveAt(i--);  // 如果有损毁的接收器
                else if (!connecting.Contains(triggering[i]) && triggering[i].GetTeamId() == identity.GetTeamId()) {  // 如果有范围内本队的接收器没有被赋予信号
                    triggering[i].PlusSignal(strength);
                    connecting.Add(triggering[i]);
                }
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) {
                    triggering.Add(receiver);
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (photonView.IsMine) {
                var receiver = other.GetComponent<AbstractMTConnection>();
                if (receiver != null) {
                    triggering.Remove(receiver);
                }
            }
        }

        private void OnDestroy() {
            foreach (var connector in connecting) {
                connector.PlusSignal(-strength);
            }
        }
    }
}