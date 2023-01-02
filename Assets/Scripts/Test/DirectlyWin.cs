using System;
using UnityEngine;

namespace Test {
    public class DirectlyWin : MonoBehaviour {
        public void Update() {
            if (Input.GetKeyDown(KeyCode.RightShift)) {
                Events.Invoke(Events.F_GAME_OVER, new object[] {Summary.team.teamColor });
            }
        }
    }
}