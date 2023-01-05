using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Basement.Diamond {
    /// <summary>
    /// 根据基地当前的所属情况更改基地灯光的颜色
    /// </summary>
    public class MELightChanger : MonoBehaviourPun {
        [SerializeField] private Color neutralLightColor;
        [SerializeField] private Material neutralParticleMaterial;
        [SerializeField] private Material neutralProjectorMaterial;
        [SerializeField] private List<Color> lightColors;
        [SerializeField] private List<Material> particleMaterials;
        [SerializeField] private List<Material> projectorMaterials;
        // 以上列表的索引为队伍号
        private MEBaseFlag identity;
        private int memorizingFlag;  // 上一帧基地所属的团队

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
                    var materials = meshRenderer.materials;
                    materials[2] = memorizingFlag == -1 ? neutralProjectorMaterial : projectorMaterials[memorizingFlag];
                    meshRenderer.materials = materials;
                }
            }
        }
    }
}