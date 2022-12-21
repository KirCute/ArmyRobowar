using System.Collections.Generic;
using Model.Equipment;

namespace System {
    public static class Summary {
        public static List<Team> teams { get; }
        public static Dictionary<int, Robot> robots { get; }
        public static int localPlayerTeam { get; }

        static Summary() {
            teams = new List<Team>();
            robots = new Dictionary<int, Robot>();
            localPlayerTeam = -1;
            Events.AddListener(Events.F_GAME_START, OnGameStart);
        }

        private static void OnGameStart(object[] args) {
            // TODO
        }
    }
}