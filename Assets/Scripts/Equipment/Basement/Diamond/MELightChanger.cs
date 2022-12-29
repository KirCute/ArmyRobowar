using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Diamond {
    public class MELightChanger : MonoBehaviourPun {
        [SerializeField] private Color neutralLightColor;
        [SerializeField] private Material neutralParticleMaterial;
        [SerializeField] private Material neutralProjectorMaterial;
        [SerializeField] private List<Color> lightColors;
        [SerializeField] private List<Material> particleMaterials;
        [SerializeField] private List<Material> projectorMaterials;
        private MEBaseFlag identity;
        private int memorizingFlag;

        private void Awake() {
            identity = GetComponentInParent<MEBaseFlag>();
        }

        private void Update() {
            if (identity.flagColor != memorizingFlag) {
                memorizingFlag = identity.flagColor;
                foreach (var particle in GetComponentsInChildren<ParticleSystemRenderer>()) {
                    particle.material =
                        memorizingFlag == -1 ? neutralParticleMaterial : particleMaterials[memorizingFlag];
                }

                foreach (var lightComponent in GetComponentsInChildren<Light>()) {
                    lightComponent.color = memorizingFlag == -1 ? neutralLightColor : lightColors[memorizingFlag];
                }

                foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>()) {
                    meshRenderer.materials[2] = memorizingFlag == -1
                        ? neutralProjectorMaterial
                        : projectorMaterials[memorizingFlag];
                }
            }
        }
    }
}