using System;
using Photon.Pun;
using UnityEngine;

namespace Model.Equipment.Template {
    public class SensorTemplate {
        public const int SENSOR_TYPE_CAMERA = 0;
        public const int SENSOR_TYPE_GUN = 1;
        public const int SENSOR_TYPE_LIDAR = 2;
        public const int SENSOR_TYPE_INVENTORY = 3;
        public const int SENSOR_TYPE_ARMOR = 4;
        public const int SENSOR_TYPE_ENGINEER = 5;
        
        public const string COMMON_COMPONENT_PREFAB = "CommonPickableComponent";
        public const string RARE_COMPONENT_PREFAB = "RarePickableComponent";
        public const string HEROIC_COMPONENT_PREFAB = "HeroicPickableComponent";
        
        public readonly string nameOnTechnologyTree;
        public readonly string name;
        public readonly string description;
        public readonly int type;
        public readonly int maxHealth;
        public readonly int cost;
        public readonly double dropProbability;
        public readonly Sensor.EquipDelegate onEquipped;
        public readonly Sensor.EquipDelegate onUnloaded;
        public readonly string pickablePrefabName;

        public SensorTemplate(string technic, string name, string description, int type, int maxHealth, int cost, 
            double dropProbability, Sensor.EquipDelegate onEquipped, Sensor.EquipDelegate onUnloaded, string pickable) {
            this.nameOnTechnologyTree = technic;
            this.name = name;
            this.description = description;
            this.type = type;
            this.maxHealth = maxHealth;
            this.cost = cost;
            this.dropProbability = dropProbability;
            this.onEquipped = onEquipped;
            this.onUnloaded = onUnloaded;
            this.pickablePrefabName = pickable;
        }

        public static Sensor.EquipDelegate COMMON_OBJECT_COMPONENT_ON_EQUIPPED(string prefab) {
            return (_, id, index, processObject) => {
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