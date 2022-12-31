using UnityEngine;

namespace UI {
    public class MEEnableWhenClick : MonoBehaviour {
        [SerializeField] private MonoBehaviour managingScript;

        public void OnClick() {
            managingScript.enabled = true;
        }
    }
}