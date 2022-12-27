using System;
using Photon.Pun;
using UnityEngine;

namespace Model.Equipment.Template {
    public class SensorTemplate {
        public readonly string nameOnTechnologyTree;
        public readonly string name;
        public readonly string description;
        public readonly int maxHealth;
        public readonly int cost;
        public readonly Sensor.EquipDelegate onEquipped;
        public readonly Sensor.EquipDelegate onUnloaded;

        public SensorTemplate(string technic, string name, string description, int maxHealth,
            int cost, Sensor.EquipDelegate onEquipped, Sensor.EquipDelegate onUnloaded) {
            this.nameOnTechnologyTree = technic;
            this.name = name;
            this.description = description;
            this.maxHealth = maxHealth;
            this.cost = cost;
            this.onEquipped = onEquipped;
            this.onUnloaded = onUnloaded;
        }

        public static Sensor.EquipDelegate COMMON_OBJECT_COMPONENT_ON_EQUIPPED(string prefab) {
            return (self, id, index, processObject) => {
                if (processObject) {
                    PhotonNetwork.Instantiate(prefab, Vector3.zero, Quaternion.identity, 0,
                        new object[] {id, index, Summary.team.teamColor}
                    );
                }
            };
        }

        public static readonly Sensor.EquipDelegate COMMON_OBJECT_COMPONENT_ON_UNLOADED = (_, id, index, processObject) => {
            if (processObject) Events.Invoke(Events.F_COMPONENT_DESTROYED, new object[] {id, index});
        };
    }
}