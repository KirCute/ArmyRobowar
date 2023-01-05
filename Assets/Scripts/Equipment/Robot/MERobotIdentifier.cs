using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Equipment.Robot {
    /// <summary>
    /// 用于记录机器人的ID和团队，从而可以从机器人的GameObject反推其ID和所属团队
    /// </summary>
    public class MERobotIdentifier : MonoBehaviourPun, IPunObservable {
        [SerializeField] private List<Color> lightColors;
        [SerializeField] private List<Material> planeMaterials;
        //以上列表索引为队伍号，每个脚本里此条属性均相同
        public int id { get; set; }
        public int team { get; set; }

        public void Awake() {
            id = (int) photonView.InstantiationData[0];
            team = (int) photonView.InstantiationData[1];
            gameObject.name = $"Robot_{id}";  // 按约定改名
            if (Summary.team.teamColor == team) Summary.team.robots[id].manufacturing = false;

            // 修改车身上能够表示身份的标记的颜色，烂代码慎看
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
            // 这些数据理论上没有同步的必要
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