using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

namespace Equipment.Sensor.Gun {
    public class MEFire : MonoBehaviourPun {
        private const float MAX_SHOOT_DISTANCE = 20f;
        
        private MEComponentIdentifier identity;
        private double lastShoot;
        [SerializeField] private int damage = 3;
        [SerializeField] private double loadingTime = 2.0;

        private void Awake() {
            identity = GetComponent<MEComponentIdentifier>();
        }

        private void OnEnable() {
            Events.AddListener(Events.M_ROBOT_FIRE, OnFire);
        }

        private void OnDisable() {
            Events.RemoveListener(Events.M_ROBOT_FIRE, OnFire);
        }

        private void OnFire(object[] args) {
            if (identity.robotId == (int) args[0] && PhotonNetwork.Time - lastShoot > loadingTime && photonView.IsMine) {
                if (Physics.Raycast(transform.position, transform.forward, out var hit, MAX_SHOOT_DISTANCE)) {
                    var hurt = hit.collider.GetComponent<AbstractMTHurt>();
                    if (hurt != null) hurt.Hurt(damage, identity.team);
                }

                Events.Invoke(Events.F_ROBOT_FIRED, new object[] {identity.robotId, transform.position, hit.point});
                lastShoot = PhotonNetwork.Time;
            }
        }
    }
}