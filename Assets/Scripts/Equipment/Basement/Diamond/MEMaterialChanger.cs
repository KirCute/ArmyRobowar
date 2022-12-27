using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Diamond {
    public class MEMaterialChanger : MonoBehaviourPun {
        [SerializeField] private Material neutralFlagMaterial;
        [SerializeField] private List<Material> flagMaterials;
        private MEBaseFlag identity;
        private int memorizingFlag;

        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }

        private void Update() {
            if (identity.flagColor != memorizingFlag) {
                memorizingFlag = identity.flagColor;
                GetComponent<Renderer>().material =
                    memorizingFlag == -1 ? neutralFlagMaterial : flagMaterials[memorizingFlag];
            }
        }
    }
}