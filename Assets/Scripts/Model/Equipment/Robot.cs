using System.Collections.Generic;
using Model.Equipment.Template;
using Model.Inventory;
using Photon.Pun;
using Photon.Realtime;

namespace Model.Equipment {
    /// <summary>
    /// 机器人
    /// </summary>
    public class Robot {
        public const byte STATUS_ACTIVE = 0;
        public const byte STATUS_MISSING = 1;
        public const byte STATUS_MANUFACTURING = 2;

        public readonly int id;
        public readonly string name;  // 玩家命名
        public readonly double createTime;  // 生产出来的时间，用来判定该不该生成游戏物体
        public readonly RobotTemplate template;  // 模板
        public readonly Sensor[] equippedComponents;  // peer-to-peer，安装的配件，注意判null
        public readonly List<IItem> inventory;  // peer-to-peer，物品栏
        public bool atHome { get; set; }  // peer-to-peer，是否可改装
        public int atBase { get; set; }  // peer-to-peer，是否准备占领基地
        public int maxHealth { get; set; }  // peer-to-peer，最大血量（安装装甲时会修改）
        public int health { get; set; }  // client-server，当前血量
        public int connection { get; set; }  // client-server，当前信号值
        public bool manufacturing { get; set; }  // peer-to-peer，是否正在生产
        public bool allowBuild { get; set; }  // peer-to-peer，是否允许建造（安装工程配件时会修改）
        public int inventoryCapacity { get; set; }  // peer-to-peer，物品栏容量
        public Player controller { get; set; }  // peer-to-peer，控制的玩家，无玩家控制时为null
        public double lastRecoveryTime { get; set; }  // peer-to-peer，上一次回血时间
        public byte status => manufacturing ? STATUS_MANUFACTURING : (connection > 0 ? STATUS_ACTIVE : STATUS_MISSING);  // 状态

        public Robot(int id, string name, RobotTemplate template) {
            this.id = id;
            this.name = name;
            this.maxHealth = template.maxHealth;
            this.health = maxHealth;
            this.template = template;
            this.equippedComponents = new Sensor[template.capacity];
            this.inventory = new List<IItem>();
            this.inventoryCapacity = 0;
            this.connection = 0;
            this.manufacturing = true;
            this.createTime = PhotonNetwork.Time;
            this.lastRecoveryTime = PhotonNetwork.Time;
            this.controller = null;
        }
    }
}