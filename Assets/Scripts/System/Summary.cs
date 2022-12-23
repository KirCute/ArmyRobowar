using System.Collections.Generic;
using Model;
using Model.Equipment;

namespace System {
    public static class Summary {
        public static bool isGameStarted;
        public static bool isTeamLeader;
        public static Team team { get; }
        public static int nextRobotId { get; set; }
        public static int nextTowerId { get; set; }

        static Summary() {
            Events.AddListener(Events.F_GAME_START, OnGameStart);
        }

        private static void OnGameStart(object[] args) {
            isGameStarted = true;
            //team = new Team( );
            // TODO
        }
    }
}