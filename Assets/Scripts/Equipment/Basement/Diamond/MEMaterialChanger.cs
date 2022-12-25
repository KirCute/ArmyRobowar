using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Diamond {
    public class MEMaterialChanger : MonoBehaviourPun {
        [SerializeField] private Material neutralFlagMaterial;
        [SerializeField] private List<Material> flagMaterials;
        private MEBaseFlag identity;

        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }

        private void OnEnable() {
            GetComponent<Renderer>().material = flagMaterials[identity.flagColor];
        }

        private void OnDisable() {
            GetComponent<Renderer>().material = neutralFlagMaterial;
        }
    }
}