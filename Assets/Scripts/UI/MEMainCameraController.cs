using UnityEngine;

namespace UI {
    public class MEMainCameraController : MonoBehaviour {
        private const float SENSITIVITY = 7.5f;
        private const int CLICK_LAYER_MASK = 1 << 5;
        
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector3 mainCameraInitPosition;
        [SerializeField] private Vector3 mainCameraInitRotation;
        private bool _active;

        public bool active {
            get => _active;
            set {
                _active = value;
                Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            }
        }

        private void OnEnable() {
            Events.AddListener(Events.F_GAME_START, OnGameStart);
            Events.AddListener(Events.F_GAME_OVER, OnGameOver);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.F_GAME_START, OnGameStart);
        }

        private void OnGameStart(object[] args) {
            active = true;
            mainCamera.transform.position = mainCameraInitPosition;
            mainCamera.transform.eulerAngles = mainCameraInitRotation;
        }

        private void Update() {
            if (!active) return;
            var cameraRot = mainCamera.transform.localEulerAngles;
            cameraRot.x -= Input.GetAxis("Mouse Y") * SENSITIVITY;
            cameraRot.y += Input.GetAxis("Mouse X") * SENSITIVITY;
            while (cameraRot.x < -180f) cameraRot.x += 360f;
            while (cameraRot.x > 180f) cameraRot.x -= 360f;
            cameraRot.x = Mathf.Clamp(cameraRot.x, -90f, 90f);
            while (cameraRot.y < 0f) cameraRot.y += 360f;
            while (cameraRot.y > 360f) cameraRot.y -= 360f;
            mainCamera.transform.localEulerAngles = cameraRot;

            if (Input.GetMouseButtonDown(1)) {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 10.0f, CLICK_LAYER_MASK)) {
                    var click = hit.collider.GetComponent<MEEnableWhenClick>();
                    if (click != null) {
                        active = false;
                        click.OnClick();
                    }
                }
            }
        }
        private void OnGameOver(object[] args) {
            if (args.Length != 0) {
                this.enabled = false;
            }
        }
        
    }
}