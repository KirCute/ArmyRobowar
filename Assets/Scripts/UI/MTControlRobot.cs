using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace UI {
    public class MTControlRobot : MonoBehaviour {
        private const float SENSITIVITY = 10f;
        private int controllingRobot { get; set; } = -1;
        private Vector2 lastMotivation = Vector2.zero;
        
        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_CONTROL, OnControlled);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_CONTROL, OnControlled);
        }

        private void OnControlled(object[] args) {
            if (controllingRobot == (int) args[0] && args[1] == null) {
                Cursor.lockState = CursorLockMode.None;
                controllingRobot = -1;
            } else if (PhotonNetwork.LocalPlayer.Equals((Player) args[1])) {
                Cursor.lockState = CursorLockMode.Locked;
                controllingRobot = (int) args[0];
            }
        }
        
        private void Update() {
            if (controllingRobot == -1) return;
            var mouseMove = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * SENSITIVITY;
            if (mouseMove.sqrMagnitude > 0.0001f) {
                Events.Invoke(Events.M_ROBOT_TOWARDS_CHANGE, new object[] {controllingRobot, mouseMove});
            }
            if (Input.GetMouseButtonDown(0)) {
                Events.Invoke(Events.M_ROBOT_FIRE, new object[] {controllingRobot});
            }

            if (Input.GetMouseButtonDown(1)) {
                Events.Invoke(Events.M_ROBOT_PICK, new object[] {controllingRobot});
            }

            var motivation = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if ((motivation - lastMotivation).sqrMagnitude > 0.0001f) {
                Events.Invoke(Events.M_ROBOT_MOTIVATION_CHANGE, new object[] {controllingRobot, 0, motivation});
                lastMotivation = motivation;
            }
        }
    }
}