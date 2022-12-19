using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

namespace Model.Equipment {
    public class Team {
        private const int TEAM_MAP_WIDTH = 0; // TODO
        private const int TEAM_MAP_HEIGHT = 0; // TODO
        private const int TECHNOLOGY_ITEM_COUNT = 0; // TODO
        private const int INIT_COUNT_OF_COINS = 0;
        private const double INIT_RESEARCH_POINT = 13.0;
        private const double RESEARCH_POINT_INCREMENT_PER_MIN = 2.5;

        public Team(int teamColor, ISet<int> members, double startTime, int initBase) {
            this.teamColor = teamColor;
            this.members = members;
            this.robots = new HashSet<int>();
            this.components = new HashSet<int>();
            this.bases = new HashSet<int> {initBase};
            this.towers = new HashSet<int>();
            this.coins = INIT_COUNT_OF_COINS;
            this.teamMap = new BitArray(TEAM_MAP_WIDTH * TEAM_MAP_HEIGHT, false);
            this.technology = new BitArray(TECHNOLOGY_ITEM_COUNT, false);
            this.usedResearchTime = startTime - INIT_RESEARCH_POINT * 60.0 / RESEARCH_POINT_INCREMENT_PER_MIN;
        }

        public int teamColor { get; }
        public ISet<int> members { get; }
        public ISet<int> robots { get; }
        public ISet<int> components { get; }
        public ISet<int> bases { get; }
        public ISet<int> towers { get; }
        public int coins { get; set; }
        public BitArray teamMap { get; }
        public BitArray technology { get; }
        private double usedResearchTime;

        public double researchPoint {
            get => (PhotonNetwork.Time - usedResearchTime) * RESEARCH_POINT_INCREMENT_PER_MIN / 60.0;
            protected set => usedResearchTime = PhotonNetwork.Time - value * 60.0 / RESEARCH_POINT_INCREMENT_PER_MIN;
        }
    }
}