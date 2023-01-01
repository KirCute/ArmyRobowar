using System.Collections.Generic;
using Model.Equipment.Template;
using Model.Technology;
using UnityEngine;

namespace System {
    public static class Constants {
        public const int BASE_COUNT = 6;
        public const int BASE_CAPTURE_COST = 100;
        
        public static readonly IReadOnlyDictionary<string, Technic> TECHNOLOGY = new Dictionary<string, Technic> {
            {"BaseRobot", new Technic("基础底盘", "最基础的底盘，血量和机动中规中矩，可以携带三个配件。", 0.0, _ => { })},
            {"iLightRobot", new Technic("轻质底盘", "向机动特化的底盘，拥有更大的移动速度，转速和射界，但血量更低。", 2.0, team => team.availableRobotTemplates.Add("iLightRobot"))},
            {"iiLightRobot", new Technic("全向机动底盘", "拥有全向射界的底盘，移动速度和转速更快，但最多只能携带两个配件。", 5.0, team => team.availableRobotTemplates.Add("iiLightRobot"))},
            {"iTruckRobot", new Technic("载物底盘", "可携带四个配件的底盘，移动速度相比基础底盘更慢。", 2.0, team => team.availableRobotTemplates.Add("iTruckRobot"))},
            {"iiTruckRobot", new Technic("专用载物底盘", "可携带五个配件的底盘，射界相比基础底盘更小。", 5.0, team => team.availableRobotTemplates.Add("iiTruckRobot"))},
            {"iHeavyRobot", new Technic("重型底盘", "拥有更高血量的底盘，转速相比基础底盘更慢。", 2.0, team => team.availableRobotTemplates.Add("iHeavyRobot"))},
            {"iiHeavyRobot", new Technic("超重型底盘", "拥有非常高血量的底盘，可携带四个配件的底盘，但移动速度和转速都受到限制。", 5.0, team => team.availableRobotTemplates.Add("iiHeavyRobot"))},
            {"BaseCamera", new Technic("基础摄像机", "最基础的摄像机，拥有20m的可视距离。", 0.0, _ => { })},
            {"iFarCamera", new Technic("远距摄像机", "拥有30m的可视距离。", 2.0, team => team.availableSensorTemplates.Add("iFarCamera"))},
            {"iiFarCamera", new Technic("超远距摄像机", "拥有40m的可视距离。", 5.0, team => team.availableSensorTemplates.Add("iiFarCamera"))},
            {"iDepthCamera", new Technic("深度摄像机", "拥有20m的可视距离，当敌人进入视野时，可将其标记在小地图上。", 2.0, team => team.availableSensorTemplates.Add("iDepthCamera"))},
            {"iiDepthCamera", new Technic("热成像摄像机", "拥有30m的可视距离，索敌时无视障碍物的阻挡。", 5.0, team => team.availableSensorTemplates.Add("iiDepthCamera"))},
            {"BaseGun", new Technic("基础炮", "最基础的炮，射速和伤害中规中矩。", 0.0, _ => { })},
            {"iDpmGun", new Technic("速射炮", "拥有更快的射速但伤害更低的炮，每分钟伤害比基础炮高。", 2.0, team => team.availableSensorTemplates.Add("iDpmGun"))},
            {"iiDpmGun", new Technic("高级速射炮", "射速特别快但伤害非常低的炮，每分钟伤害更高。", 5.0, team => team.availableSensorTemplates.Add("iiDpmGun"))},
            {"iDpsGun", new Technic("大口径炮", "拥有更高的伤害但射速更慢的炮，每分钟伤害比基础炮低。", 2.0, team => team.availableSensorTemplates.Add("iDpsGun"))},
            {"iiDpsGun", new Technic("超大口径炮", "伤害特别高但装填非常久的炮，每分钟伤害更低。", 5.0, team => team.availableSensorTemplates.Add("iiDpsGun"))},
            {"iInventory", new Technic("小型载物架", "机器人的最大载物量+2。", 0.0, _ => { })},
            {"iiInventory", new Technic("中型载物架", "机器人的最大载物量+3。", 2.0, team => team.availableSensorTemplates.Add("iiInventory"))},
            {"iiiInventory", new Technic("大型载物架", "机器人的最大载物量+4。", 5.0, team => team.availableSensorTemplates.Add("iiiInventory"))},
            {"iArmor", new Technic("小型装甲", "机器人的最大血量+5。", 0.0, team => team.availableSensorTemplates.Add("iArmor"))},
            {"iiArmor", new Technic("中型装甲", "机器人的最大血量+10。", 2.0, team => team.availableSensorTemplates.Add("iiArmor"))},
            {"iiiArmor", new Technic("大型装甲", "机器人的最大血量+15。", 5.0, team => team.availableSensorTemplates.Add("iiiArmor"))},
            {"iLidar", new Technic("近程雷达", "提供近距离小地图，且机器人扫描过的地方将被记录进团队地图。", 0.0, team => team.availableSensorTemplates.Add("iLidar"))},
            {"iiLidar", new Technic("中程雷达", "提供中等距离小地图，扫描距离增大。", 2.0, team => team.availableSensorTemplates.Add("iiLidar"))},
            {"iiiLidar", new Technic("远程雷达", "提供远距离小地图，扫描距离进一步增大。", 5.0, team => team.availableSensorTemplates.Add("iiiLidar"))},
            {"Engineer", new Technic("工程配件", "使机器人可以建造信号塔和占领基地。", 0.0, _ => { })},
            {"BaseTower", new Technic("信号塔", "为一定范围内的机器人提供信号。", 0.0, _ => { })}
        };

        public static readonly IReadOnlyDictionary<string, RobotTemplate> ROBOT_TEMPLATES =
            new Dictionary<string, RobotTemplate> {
                {"BaseRobot", new RobotTemplate("BaseRobot", "基础底盘", "BaseRobot", 3, 30, 20, 10.0f)},
                {"iLightRobot", new RobotTemplate("iLightRobot", "轻质底盘", "iLightRobot", 3, 20, 30, 20.0f)},
                {"iiLightRobot", new RobotTemplate("iiLightRobot", "全向机动底盘", "iiLightRobot", 2, 20, 40, 30.0f)},
                {"iTruckRobot", new RobotTemplate("iTruckRobot", "载物底盘", "iTruckRobot", 4, 30, 30, 20.0f)},
                {"iiTruckRobot", new RobotTemplate("iiTruckRobot", "专用载物底盘", "iiTruckRobot", 5, 30, 40, 30.0f)},
                {"iHeavyRobot", new RobotTemplate("iHeavyRobot", "重型底盘", "iHeavyRobot", 3, 40, 30, 20.0f)},
                {"iiHeavyRobot", new RobotTemplate("iiHeavyRobot", "超重型底盘", "iiHeavyRobot", 4, 50, 40, 30.0f)}
            };

        public static readonly IReadOnlyDictionary<string, SensorTemplate> SENSOR_TEMPLATES =
            new Dictionary<string, SensorTemplate> {
                {
                    "BaseCamera", new SensorTemplate("BaseCamera", "基础摄像机", "最基础的摄像机，拥有20m的可视距离。",
                        SensorTemplate.SENSOR_TYPE_CAMERA, 6, 10, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("BaseCamera"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.COMMON_COMPONENT_PREFAB
                    )
                }, {
                    "iFarCamera", new SensorTemplate("iFarCamera", "远距摄像机", "拥有30m的可视距离。",
                        SensorTemplate.SENSOR_TYPE_CAMERA, 6, 20, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iFarCamera"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.RARE_COMPONENT_PREFAB
                    )
                }, {
                    "iiFarCamera", new SensorTemplate("iiFarCamera", "超远距摄像机", "拥有40m的可视距离。",
                        SensorTemplate.SENSOR_TYPE_CAMERA, 9, 40, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iiFarCamera"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.HEROIC_COMPONENT_PREFAB
                    )
                }, {
                    "iDepthCamera", new SensorTemplate("iDepthCamera", "深度摄像机", "拥有20m的可视距离，当敌人进入视野时，可将其标记在小地图上。",
                        SensorTemplate.SENSOR_TYPE_CAMERA, 6, 20, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iDepthCamera"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.RARE_COMPONENT_PREFAB
                    )
                }, {
                    "iiDepthCamera", new SensorTemplate("iiDepthCamera", "热成像摄像机", "拥有30m的可视距离，索敌时无视障碍物的阻挡。",
                        SensorTemplate.SENSOR_TYPE_CAMERA, 9, 40, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iiDepthCamera"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.HEROIC_COMPONENT_PREFAB
                    )
                }, {
                    "BaseGun", new SensorTemplate("BaseGun", "基础炮", "最基础的炮，射速和伤害中规中矩。", 
                        SensorTemplate.SENSOR_TYPE_GUN, 6, 10, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("BaseGun"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.COMMON_COMPONENT_PREFAB
                    )
                }, {
                    "iDpmGun", new SensorTemplate("iDpmGun", "速射炮", "拥有更快的射速但伤害更低的炮，每分钟伤害比基础炮高。", 
                        SensorTemplate.SENSOR_TYPE_GUN, 6, 20, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iDpmGun"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.RARE_COMPONENT_PREFAB
                    )
                }, {
                    "iiDpmGun", new SensorTemplate("iiDpmGun", "高级速射炮", "射速特别快但伤害非常低的炮，每分钟伤害更高。", 
                        SensorTemplate.SENSOR_TYPE_GUN, 9, 40, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iiDpmGun"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.HEROIC_COMPONENT_PREFAB
                    )
                }, {
                    "iDpsGun", new SensorTemplate("iDpsGun", "大口径炮", "拥有更高的伤害但射速更慢的炮，每分钟伤害比基础炮低。", 
                        SensorTemplate.SENSOR_TYPE_GUN, 6, 20, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iDpsGun"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.RARE_COMPONENT_PREFAB
                    )
                }, {
                    "iiDpsGun", new SensorTemplate("iiDpsGun", "超大口径炮", "伤害特别高但装填非常久的炮，每分钟伤害更低。", 
                        SensorTemplate.SENSOR_TYPE_GUN, 9, 40, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iiDpsGun"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.HEROIC_COMPONENT_PREFAB
                    )
                }, {
                    "iInventory", new SensorTemplate("iInventory", "小型载物架", "机器人的最大载物量+2。", 
                        SensorTemplate.SENSOR_TYPE_INVENTORY, 1, 20, 0.3,
                        (_, id, _, _) => Summary.team.robots[id].inventoryCapacity += 2,
                        (_, id, _, _) => Summary.team.robots[id].inventoryCapacity -= 2,
                        SensorTemplate.COMMON_COMPONENT_PREFAB
                    )
                }, {
                    "iiInventory", new SensorTemplate("iiInventory", "中型载物架", "机器人的最大载物量+3。", 
                        SensorTemplate.SENSOR_TYPE_INVENTORY, 1, 30, 0.3,
                        (_, id, _, _) => Summary.team.robots[id].inventoryCapacity += 3,
                        (_, id, _, _) => Summary.team.robots[id].inventoryCapacity -= 3,
                        SensorTemplate.RARE_COMPONENT_PREFAB
                    )
                }, {
                    "iiiInventory", new SensorTemplate("iiiInventory", "大型载物架", "机器人的最大载物量+4。", 
                        SensorTemplate.SENSOR_TYPE_INVENTORY, 1, 40, 0.3,
                        (_, id, _, _) => Summary.team.robots[id].inventoryCapacity += 4,
                        (_, id, _, _) => Summary.team.robots[id].inventoryCapacity -= 4,
                        SensorTemplate.HEROIC_COMPONENT_PREFAB
                    )
                }, {
                    "iArmor", new SensorTemplate("iArmor", "小型装甲", "机器人的最大血量+5。", 
                        SensorTemplate.SENSOR_TYPE_ARMOR, 1, 20, 0.3,
                        (_, id, _, _) => Summary.team.robots[id].maxHealth += 5,
                        (_, id, _, _) => {
                            Summary.team.robots[id].maxHealth -= 5;
                            Summary.team.robots[id].health = Mathf.Min(Summary.team.robots[id].maxHealth,Summary.team.robots[id].health);
                        },
                        SensorTemplate.COMMON_COMPONENT_PREFAB
                    )
                }, {
                    "iiArmor", new SensorTemplate("iiArmor", "中型装甲", "机器人的最大血量+10。", 
                        SensorTemplate.SENSOR_TYPE_ARMOR, 1, 40, 0.3,
                        (_, id, _, _) => Summary.team.robots[id].maxHealth += 10,
                        (_, id, _, _) => {
                            Summary.team.robots[id].maxHealth -= 10;
                            Summary.team.robots[id].health = Mathf.Min(Summary.team.robots[id].maxHealth,Summary.team.robots[id].health);
                        },
                        SensorTemplate.RARE_COMPONENT_PREFAB
                    )
                }, {
                    "iiiArmor", new SensorTemplate("iiiArmor", "大型装甲", "机器人的最大血量+15。", 
                        SensorTemplate.SENSOR_TYPE_ARMOR, 1, 60, 0.3,
                        (_, id, _, _) => Summary.team.robots[id].maxHealth += 15,
                        (_, id, _, _) => {
                            Summary.team.robots[id].maxHealth -= 15;
                            Summary.team.robots[id].health = Mathf.Min(Summary.team.robots[id].maxHealth,Summary.team.robots[id].health);
                        },
                        SensorTemplate.HEROIC_COMPONENT_PREFAB
                    )
                }, {
                    "iLidar", new SensorTemplate("iLidar", "近程雷达", "提供近距离小地图，且机器人扫描过的地方将被记录进团队地图。", 
                        SensorTemplate.SENSOR_TYPE_LIDAR, 6, 10, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iLidar"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.COMMON_COMPONENT_PREFAB
                    )
                }, {
                    "iiLidar", new SensorTemplate("iiLidar", "中程雷达", "提供中等距离小地图，扫描距离增大。", 
                        SensorTemplate.SENSOR_TYPE_LIDAR, 6, 20, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iiLidar"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.RARE_COMPONENT_PREFAB
                    )
                }, {
                    "iiiLidar", new SensorTemplate("iiiLidar", "远程雷达", "提供远距离小地图，扫描距离进一步增大。", 
                        SensorTemplate.SENSOR_TYPE_LIDAR, 9, 40, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("iiiLidar"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.HEROIC_COMPONENT_PREFAB
                    )
                }, {
                    "Engineer", new SensorTemplate("Engineer", "工程配件", "使机器人可以建造信号塔和占领基地。", 
                        SensorTemplate.SENSOR_TYPE_ENGINEER, 1, 20, 0.3,
                        (_, id, _, _) => Summary.team.robots[id].allowBuild = true,
                        (_, id, _, _) => Summary.team.robots[id].allowBuild = false,
                        SensorTemplate.COMMON_COMPONENT_PREFAB
                    )
                }
            };

        public static readonly IReadOnlyDictionary<string, TowerTemplate> TOWER_TEMPLATES =
            new Dictionary<string, TowerTemplate> {
                {"BaseTower", new TowerTemplate("BaseTower", "信号塔", "Tower", 45, 20)}
            };

        public static readonly IReadOnlyDictionary<string, List<string>>  TECHNIC_TOPOLOGY = new Dictionary<string, List<string>> {
            {"BaseRobot", new List<string>()},
            {"iLightRobot", new List<string> {"BaseRobot"}},
            {"iiLightRobot", new List<string> {"iLightRobot"}},
            {"iTruckRobot", new List<string> {"BaseRobot"}},
            {"iiTruckRobot", new List<string> {"iTruckRobot"}},
            {"iHeavyRobot", new List<string> {"BaseRobot"}},
            {"iiHeavyRobot", new List<string> {"iHeavyRobot"}},
            {"BaseCamera", new List<string>()},
            {"iFarCamera", new List<string> {"BaseCamera"}},
            {"iiFarCamera", new List<string> {"iFarCamera"}},
            {"iDepthCamera", new List<string> {"BaseCamera"}},
            {"iiDepthCamera", new List<string> {"iDepthCamera", "iFarCamera"}},
            {"BaseGun", new List<string>()},
            {"iDpmGun", new List<string> {"BaseGun"}},
            {"iiDpmGun", new List<string> {"iDpmGun"}},
            {"iDpsGun", new List<string> {"BaseGun"}},
            {"iiDpsGun", new List<string> {"iDpsGun"}},
            {"iInventory", new List<string>()},
            {"iiInventory", new List<string> {"iInventory"}},
            {"iiiInventory", new List<string> {"iiInventory"}},
            {"iArmor", new List<string>()},
            {"iiArmor", new List<string> {"iArmor"}},
            {"iiiArmor", new List<string> {"iiArmor"}},
            {"iLidar", new List<string>()},
            {"iiLidar", new List<string> {"iLidar"}},
            {"iiiLidar", new List<string> {"iiLidar"}},
            {"Engineer", new List<string>()},
            {"BaseTower", new List<string>()}
        };
    }
}