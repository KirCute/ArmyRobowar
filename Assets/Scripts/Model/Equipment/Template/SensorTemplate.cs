using System;
using Photon.Pun;
using UnityEngine;

namespace Model.Equipment.Template {
    /// <summary>
    /// 配件的模板类
    /// </summary>
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
        
        public readonly string nameOnTechnologyTree;  // 科技树上的名称
        public readonly string name;  // 显示名称
        public readonly string description;  //描述
        public readonly int type;  // 类型，取值参考上述SENSOR_TYPE_XXX，用于防止同类配件重复安装
        public readonly int maxHealth;  // 最大血量
        public readonly int cost;  // 价格
        public readonly double dropProbability;  // 机器人死亡时掉落概率
        public readonly Sensor.EquipDelegate onEquipped;  // 如何被安装在机器人上
        public readonly Sensor.EquipDelegate onUnloaded;  // 如何从机器人上卸载
        public readonly string pickablePrefabName;  // 掉落物预制体名，约等于稀有度

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

        /// <summary>
        /// 返回大部分有游戏物体的配件的安装方式
        /// </summary>
        /// <param name="prefab">预制体名</param>
        /// <returns>安装方式</returns>
        public static Sensor.EquipDelegate COMMON_OBJECT_COMPONENT_ON_EQUIPPED(string prefab) {
            return (_, id, index, processObject) => {
                if (processObject) {
                    PhotonNetwork.Instantiate(prefab, Vector3.zero, Quaternion.identity, 0,
                        new object[] {id, index, Summary.team.teamColor}
                    );
                }
            };
        }

        /// <summary>
        /// 大部分有游戏物体的配件的卸载方式
        /// </summary>
        public static readonly Sensor.EquipDelegate COMMON_OBJECT_COMPONENT_ON_UNLOADED = (_, id, index, processObject) => {
            if (processObject) Events.Invoke(Events.F_COMPONENT_DESTROYED, new object[] {id, index});
        };
    }
}