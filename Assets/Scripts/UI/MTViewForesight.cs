using Photon.Pun;
using UnityEngine;

namespace UI {
    public class MTViewForesight : MonoBehaviourPunCallbacks {
        private GameObject foresightObject;

        private void Awake() {
            foresightObject = transform.Find("Image").gameObject;
            foresightObject.SetActive(false);
        }

        private void Update() {
            if (foresightObject.activeSelf != (Cursor.lockState == CursorLockMode.Locked)) {
                foresightObject.SetActive(Cursor.lockState == CursorLockMode.Locked);
            }
        }
        /*private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);*/
        //}
    }
}