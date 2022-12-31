using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot {
    /// <summary>
    /// 仅用于存储车体的id
    /// </summary>
    public class MERobotIdentifier : MonoBehaviourPun, IPunObservable {
        [SerializeField] private List<Color> lightColors;
        [SerializeField] private List<Material> planeMaterials;
        public int id { get; set; }
        public int team { get; set; }

        public void Awake() {
            id = (int) photonView.InstantiationData[0];
            team = (int) photonView.InstantiationData[1];
            gameObject.name = $"Robot_{id}";
            foreach (var lightRenderer in GetComponentsInChildren<Light>()) {
                lightRenderer.color = lightColors[team];
            }

            transform.Find("Body").Find("平面.006").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("平面.007").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("平面.008").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("平面.009").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("平面.010").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("U arm.step").Find("Gun turret.step").Find("平面").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("U arm.step").Find("Gun turret.step").Find("平面.002").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("U arm.step").Find("Gun turret.step").Find("平面.003").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("U arm.step").Find("Gun turret.step").Find("平面.004").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("U arm.step").Find("Gun turret.step").Find("平面.005").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("U arm.step").Find("Gun turret.step").Find("立方体").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("U arm.step").Find("Gun turret.step").Find("button1").GetComponent<Renderer>().material = planeMaterials[team];
            transform.Find("Body").Find("U arm.step").Find("Gun turret.step").Find("button2").GetComponent<Renderer>().material = planeMaterials[team];
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(id);
                stream.SendNext(team);
            } else {
                id = (int) stream.ReceiveNext();
                team = (int) stream.ReceiveNext();
            }
        }
    }
}