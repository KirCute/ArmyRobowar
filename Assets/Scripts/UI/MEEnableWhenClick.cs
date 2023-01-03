using Photon.Pun;
using UnityEngine;

namespace UI {
    public class MEEnableWhenClick : MonoBehaviourPunCallbacks {
        [SerializeField] private MonoBehaviour managingScript;

        public void OnClick() {
            managingScript.enabled = true;
        }
        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }

        public override void OnDisable() {
            base.OnDisable();
            Events.RemoveListener(Events.F_GAME_OVER, OnGameOver);
        }
    }
}