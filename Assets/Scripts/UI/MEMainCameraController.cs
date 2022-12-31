using System;
using UnityEngine;

namespace UI {
    public class MEMainCameraController : MonoBehaviour {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector3 mainCameraInitPosition;
        [SerializeField] private Vector3 mainCameraInitRotation;

        private void Start() {
            mainCamera.transform.position = mainCameraInitPosition;
            mainCamera.transform.eulerAngles = mainCameraInitRotation;
        }

        private void Update() {
            
        }
    }
}