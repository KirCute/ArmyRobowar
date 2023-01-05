using System.Collections;
using System.Collections.Generic;
using Model.Equipment;
using Photon.Pun;
using Photon.Realtime;

namespace Model {
    /// <summary>
    /// 团队
    /// 从Summary获取本类在整局游戏中的唯一对象
    /// </summary>
    public class Team {
        private const int TEAM_MAP_WIDTH = 46;
        private const int TEAM_MAP_HEIGHT = 34;
        private const int INIT_COUNT_OF_COINS = 200;
        private const double INIT_RESEARCH_POINT = 13.0;
        private const double RESEARCH_POINT_INCREMENT_PER_MIN = 2.5;

        public Team() {
            this.members = new List<Player>();
            this.robots = new Dictionary<int, Robot>();
            this.components = new List<Sensor>();
            this.bases = new Dictionary<int, Basement>();
            this.towers = new Dictionary<int, Tower>();
            this.availableRobotTemplates = new HashSet<string> {"BaseRobot"};
            this.availableSensorTemplates = new HashSet<string> {"BaseCamera", "BaseGun", "iInventory", "Engineer"};
            this.achievedTechnics = new HashSet<string> {"BaseRobot", "BaseCamera", "BaseGun", "iInventory", "Engineer", "BaseTower"};
            this.achievedTower = new HashSet<string> {"BaseTower"};
            this.coins = INIT_COUNT_OF_COINS;
            this.teamMap = new BitArray(TEAM_MAP_WIDTH * TEAM_MAP_HEIGHT, false);
        }

        public int teamColor { get; set; } = -1;  // peer-to-peer，团队号
        public List<Player> members { get; }  // peer-to-peer，团队成员
        public Dictionary<int, Robot> robots { get; }  // peer-to-peer，已生产的机器人（含已损毁），key为id
        public List<Sensor> components { get; }  // peer-to-peer，未被使用的配件
        public Dictionary<int, Basement> bases { get; }  // peer-to-peer，已占领的基地，key为基地号
        public Dictionary<int, Tower> towers { get; }  // peer-to-peer，已拥有的信号塔，key为id
        public ISet<string> availableRobotTemplates { get; }  // peer-to-peer，可购买的机器人模板
        public ISet<string> availableSensorTemplates { get; }  // peer-to-peer，可购买的配件模板
        public ISet<string> achievedTechnics { get; }  // peer-to-peer。已达成的科技
        public ISet<string> achievedTower { get; }  // peer-to-peer，可购买的信号塔模板
        public int coins { get; set; }  // peer-to-peer，团队货币数
        public BitArray teamMap { get; }  // peer-to-peer，团队地图
        private double usedResearchTime;  // 无非常直接的意义，请查看researchPoint
        public double researchPoint {
            get => (PhotonNetwork.Time - usedResearchTime) * RESEARCH_POINT_INCREMENT_PER_MIN / 60.0;
            set => usedResearchTime = PhotonNetwork.Time - value * 60.0 / RESEARCH_POINT_INCREMENT_PER_MIN;
        }  // peer-to-peer，团队科技点数

        public double startTime {
            set => usedResearchTime = value - INIT_RESEARCH_POINT * 60.0 / RESEARCH_POINT_INCREMENT_PER_MIN;
        }  // 开始时间，用来计算团队应当拥有的科技点
    }
}