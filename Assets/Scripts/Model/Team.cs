using System.Collections;
using System.Collections.Generic;
using Model.Equipment;
using Photon.Pun;
using Photon.Realtime;

namespace Model {
    public class Team {
        private const int TEAM_MAP_WIDTH = 0; // TODO
        private const int TEAM_MAP_HEIGHT = 0; // TODO
        private const int TECHNOLOGY_ITEM_COUNT = 0; // TODO
        private const int INIT_COUNT_OF_COINS = 10000;
        private const double INIT_RESEARCH_POINT = 13.0;
        private const double RESEARCH_POINT_INCREMENT_PER_MIN = 2.5;

        public Team(int teamColor, IReadOnlyList<Player> members, double startTime) {
            this.teamColor = teamColor;
            this.members = members;
            this.robots = new Dictionary<int, Robot>();
            this.components = new List<Sensor>();
            this.bases = new Dictionary<int, Basement>();  // TODO
            this.towers = new Dictionary<int, Tower>();
            this.availableRobotTemplates = new HashSet<string> {"iRobot"};
            this.availableSensorTemplates = new HashSet<string> {"BaseCamera", "BaseGun"};
            this.achievedTechnics = new HashSet<string>();
            this.achievedTower = new HashSet<string>();
            this.coins = INIT_COUNT_OF_COINS;
            this.teamMap = new BitArray(TEAM_MAP_WIDTH * TEAM_MAP_HEIGHT, false);
            this.usedResearchTime = startTime - INIT_RESEARCH_POINT * 60.0 / RESEARCH_POINT_INCREMENT_PER_MIN;
        }

        public int teamColor { get; }
        public IReadOnlyList<Player> members { get; }
        public Dictionary<int, Robot> robots { get; }
        public List<Sensor> components { get; }
        public Dictionary<int, Basement> bases { get; }
        public Dictionary<int, Tower> towers { get; }
        public ISet<string> availableRobotTemplates { get; }
        public ISet<string> availableSensorTemplates { get; }
        public ISet<string> achievedTechnics { get; }
        public ISet<string> achievedTower { get; }
        public int coins { get; set; }
        public BitArray teamMap { get; }
        private double usedResearchTime;

        public double researchPoint {
            get => (PhotonNetwork.Time - usedResearchTime) * RESEARCH_POINT_INCREMENT_PER_MIN / 60.0;
            protected set => usedResearchTime = PhotonNetwork.Time - value * 60.0 / RESEARCH_POINT_INCREMENT_PER_MIN;
        }
    }
}