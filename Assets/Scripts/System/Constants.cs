using System.Collections.Generic;
using Model.Equipment.Template;
using Model.Technology;

namespace System {
    public static class Constants {
        public static readonly IReadOnlyDictionary<string, Technic> TECHNOLOGY = new Dictionary<string, Technic> {
            {"iRobot", new Technic("基础底盘", "只含有两个装备槽", _ => { })},
            {"iiRobot", new Technic("进阶底盘", "拥有三个装备槽", team => team.availableRobotTemplates.Add("iiRobot"))},
            {"iiiRobot", new Technic("进阶底盘", "拥有三个装备槽", team => team.availableRobotTemplates.Add("iiiRobot"))}
        };

        public static readonly IReadOnlyDictionary<string, RobotTemplate> ROBOT_TEMPLATES = new Dictionary<string, RobotTemplate> {
            {"iRobot", new RobotTemplate("iRobot", "Robot", 2, 30, 2, 10.0f)},
            {"iiRobot", new RobotTemplate("iiRobot", "Robot", 3, 30, 3, 20.0f)},
            {"iiiRobot", new RobotTemplate("iiiRobot", "Robot", 4, 30, 4, 30.0f)}
        };

        public static readonly IReadOnlyDictionary<string, SensorTemplate> SENSOR_TEMPLATES =
            new Dictionary<string, SensorTemplate> { };
    }
}