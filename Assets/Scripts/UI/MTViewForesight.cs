using UnityEngine;

namespace UI {
    public class MTViewForesight : MonoBehaviour {
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
    }
}