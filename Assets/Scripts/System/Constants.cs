using System.Collections.Generic;
using Model.Equipment.Template;
using Model.Technology;

namespace System {
    public static class Constants {
        public static readonly IReadOnlyDictionary<string, Technic> TECHNOLOGY = new Dictionary<string, Technic> {
            {"iRobot", new Technic("基础底盘", "只含有两个装备槽", _ => { })},
            {"iiRobot", new Technic("进阶底盘", "拥有三个装备槽", team => team.availableRobotTemplates.Add("iiRobot"))},
            {"iiiRobot", new Technic("进阶底盘", "拥有三个装备槽", team => team.availableRobotTemplates.Add("iiiRobot"))},
            {"BaseCamera", new Technic("基础摄像机", "", _ => { })},
            {"BaseGun", new Technic("基础炮", "", _ => { })}
        };

        public static readonly IReadOnlyDictionary<string, RobotTemplate> ROBOT_TEMPLATES =
            new Dictionary<string, RobotTemplate> {
                {"iRobot", new RobotTemplate("iRobot", "Robot", 2, 30, 2, 2.0f)}, // TODO
                {"iiRobot", new RobotTemplate("iiRobot", "Robot", 3, 30, 3, 20.0f)},
                {"iiiRobot", new RobotTemplate("iiiRobot", "Robot", 4, 30, 4, 30.0f)}
            };

        public static readonly IReadOnlyDictionary<string, SensorTemplate> SENSOR_TEMPLATES =
            new Dictionary<string, SensorTemplate> {
                {
                    "BaseCamera", new SensorTemplate("BaseCamera", "摄像头", "基础摄像头", 6, 1, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("BaseCamera"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.COMMON_COMPONENT_PREFAB)
                }, {
                    "BaseGun", new SensorTemplate("BaseGun", "炮", "基础炮", 6, 1, 0.3,
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_EQUIPPED("BaseGun"),
                        SensorTemplate.COMMON_OBJECT_COMPONENT_ON_UNLOADED,
                        SensorTemplate.COMMON_COMPONENT_PREFAB)
                }
            };

        public static readonly IReadOnlyDictionary<string, List<string>> TECHNIC_TOPOLOGY = new Dictionary<string, List<string>> {
            {"iRobot", new List<string> {""}},
            {"iiRobot", new List<string> {"iRobot"}},
            {"iiiRobot", new List<string> {"iiRobot"}},
            {"BaseCamera", new List<string> {""}},
            {"BaseGun", new List<string> {""}}
        };
    }
}