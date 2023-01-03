using System.Collections;
using System.Collections.Generic;
using Model.Equipment;
using Photon.Pun;
using Photon.Realtime;

namespace Model {
    public class Team {
        private const int TEAM_MAP_WIDTH = 46;
        private const int TEAM_MAP_HEIGHT = 34;
        private const int INIT_COUNT_OF_COINS = 10000;
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

        public int teamColor { get; set; } = -1;  // peer-to-peer
        public List<Player> members { get; }  // peer-to-peer
        public Dictionary<int, Robot> robots { get; }  // peer-to-peer
        public List<Sensor> components { get; }  // peer-to-peer
        public Dictionary<int, Basement> bases { get; }  // peer-to-peer
        public Dictionary<int, Tower> towers { get; }  // peer-to-peer
        public ISet<string> availableRobotTemplates { get; }  // peer-to-peer
        public ISet<string> availableSensorTemplates { get; }  // peer-to-peer
        public ISet<string> achievedTechnics { get; }  // peer-to-peer
        public ISet<string> achievedTower { get; }  // peer-to-peer
        public int coins { get; set; }  // peer-to-peer
        public BitArray teamMap { get; }  // peer-to-peer
        private double usedResearchTime;
        public double researchPoint {
            get => (PhotonNetwork.Time - usedResearchTime) * RESEARCH_POINT_INCREMENT_PER_MIN / 60.0;
            set => usedResearchTime = PhotonNetwork.Time - value * 60.0 / RESEARCH_POINT_INCREMENT_PER_MIN;
        }  // peer-to-peer

        public double startTime {
            set => usedResearchTime = value - INIT_RESEARCH_POINT * 60.0 / RESEARCH_POINT_INCREMENT_PER_MIN;
        }
    }
}