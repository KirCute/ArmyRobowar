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

        public Team(int teamColor, IReadOnlyList<Player> members, double startTime) {
            this.teamColor = teamColor;
            this.members = members;
            this.robots = new Dictionary<int, Robot>();
            this.components = new List<Sensor>();
            this.bases = new Dictionary<int, Basement>();
            this.towers = new Dictionary<int, Tower>();
            this.availableRobotTemplates = new HashSet<string> {"iRobot"};
            this.availableSensorTemplates = new HashSet<string> {"BaseCamera", "BaseGun"};
            this.achievedTechnics = new HashSet<string> {"iRobot", "BaseCamera", "BaseGun", "BaseTower"};
            this.achievedTower = new HashSet<string> {"BaseTower"};
            this.coins = INIT_COUNT_OF_COINS;
            this.teamMap = new BitArray(TEAM_MAP_WIDTH * TEAM_MAP_HEIGHT, false);
            this.usedResearchTime = startTime - INIT_RESEARCH_POINT * 60.0 / RESEARCH_POINT_INCREMENT_PER_MIN;
        }

        public readonly int teamColor;
        public IReadOnlyList<Player> members { get; }
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
    }
}