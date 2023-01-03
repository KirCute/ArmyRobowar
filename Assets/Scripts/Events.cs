using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public static class Events {
    private const int COUNT_OF_EVENTS = 54;
    
    public const byte F_GAME_START = 0;  // 参数：游戏开始时间(double), 0队初始基地(int), 1队初始基地(int), 0队玩家个数n(int), n*0队玩家名单(Player), 1队玩家个数m(int), m*1队玩家名单(Player)
    public const byte M_CREATE_ROBOT = 1;  // 参数：基地号(int), 底盘的科技树编号(string), 机器人的名称(string)
    public const byte F_ROBOT_CREATED = 2;  // 参数：队伍号(int), 机器人id(int)
    public const byte F_COMPONENT_DESTROYED = 3;  // 参数：机器人id(int), 配件所在格子号(int)
    public const byte M_COMPONENT_DAMAGE = 4;  // 参数：机器人id(int), 配件所在格子号(int), 血量变化量(int)
    public const byte F_COMPONENT_HEALTH_CHANGED = 5;  // 参数：机器人id(int), 配件所在格子号(int)，配件当前血量(int)
    public const byte F_BODY_DESTROYED = 6;  // 参数：机器人id(int)
    public const byte M_BODY_DAMAGE = 7;  // 参数：机器人id(int), 血量变化量(int)
    public const byte F_BODY_HEALTH_CHANGED = 8;  // 参数：机器人id(int), 底盘当前血量(int)
    public const byte M_ROBOT_FIRE = 9;  // 参数：机器人id(int)
    public const byte F_ROBOT_FIRED = 10;  // 参数：机器人id(int), 机器人炮口位置(Vector3), 机器人击中点(Vector3)
    public const byte M_ROBOT_MOTIVATION_CHANGE = 11;  // 参数：机器人id(int), 控制模式(int), [前后方向动机, 左右方向动机（当控制模式为0时） | 目标点（当控制模式为1时）](Vector2)
    public const byte F_ROBOT_MOTIVATION_CHANGED = 12;  // 参数：机器人id(int), 控制模式(int), [前后方向动机, 左右方向动机（当控制模式为0时） | 目标点（当控制模式为1时）](Vector2)
    public const byte M_ROBOT_TOWARDS_CHANGE = 13;  // 参数：机器人id(int), 模式(锁定为0,自由移动为1)(int), 炮口和摄像头朝向改变量(仅模式1)(Vector2)
    public const byte F_ROBOT_SEIZE_ENEMY = 14;  // 参数：己方队伍号(int), 被索敌敌方机器人id(int)
    public const byte F_ROBOT_LOST_SEIZE_ENEMY = 15;  // 参数：己方队伍号(int), 被索敌敌方机器人id(int)
    public const byte M_ROBOT_CHANGE_CONNECTION = 16;  // 参数：机器人id(int), 信号强度变化量(int)
    public const byte F_ROBOT_WEAK_CONNECTION = 17;  // 参数：机器人id(int)
    public const byte F_ROBOT_LOST_CONNECTION = 18;  // 参数：机器人id(int)
    public const byte F_ROBOT_STRONG_CONNECTION = 19;  // 参数：机器人id(int)
    public const byte M_ROBOT_MONITOR = 20;  // 参数：机器人id(int), 申请查看摄像头玩家(Player), 查看(true)或退出查看(false)(bool)
    public const byte M_ROBOT_CONTROL = 21;  // 参数：机器人id(int), 机器人当前控制者(Player, 若无为null)
    public const byte M_TOWER_DAMAGE = 22;  // 参数：信号塔id(int), 血量变化量(int)
    public const byte F_TOWER_HEALTH_CHANGED = 23;  // 参数：信号塔id(int), 信号塔当前血量(int)
    public const byte F_TOWER_DESTROYED = 24;  // 参数：信号塔id(int)
    public const byte M_CAPTURE_BASE = 25;  // 参数：基地id(int), 队伍号(int)
    public const byte M_BASE_DAMAGE = 26;  // 参数：基地id(int), 血量变化量(int)
    public const byte F_BASE_HEALTH_CHANGED = 27;  // 参数：基地id(int), 当前血量(int)
    public const byte F_BASE_DESTROYED = 28;  // 参数：基地id(int), 队伍号(int)
    public const byte F_TEAM_ACQUIRE_COMPONENT = 29;  // 参数：队伍号(int), 部件的科技树编号(string), 部件的血量(int)
    public const byte F_TEAM_ACQUIRE_COINS = 30;  // 参数：队伍号(int), 资源数量(int)
    public const byte M_TEAM_BUY_COMPONENT = 31;  // 参数：队伍号(int), 部件的科技树编号(string)
    public const byte M_ROBOT_NAVIGATION = 32;  // 参数：机器人id(int), 导航点个数n(int), n*导航点(Vector2)
    public const byte F_ROBOT_ACQUIRE_COMPONENT = 33;  // 参数：队伍号(int), 机器人id(int), 部件的科技树编号(string), 部件的血量(int)
    public const byte F_ROBOT_ACQUIRE_COINS = 34;  // 参数：队伍号(int), 机器人id(int), 资源数量(int)
    public const byte M_ROBOT_RELEASE_INVENTORY = 35;  // 参数：队伍号(int), 机器人id(int)
    public const byte M_ROBOT_INSTALL_COMPONENT = 36;  // 参数：队伍号(int), 机器人id(int), 安装位置(int), 要安装的传感器在仓库中的索引(int)
    public const byte M_ROBOT_UNINSTALL_COMPONENT = 37;  // 参数：队伍号(int), 机器人id(int), 安装位置(int)
    public const byte M_CREATE_PICKABLE_COMPONENT = 38;  // 参数：部件的科技树编号(string), 部件的血量(int), 位置(Vector3)
    public const byte M_CREATE_PICKABLE_COINS = 39;  // 参数：生成金币的预制体名称(string), 位置(Vector3)
    public const byte F_ROBOT_FOUND_PICKABLE = 40;  // 参数：机器人id(int), 掉落物名称(string)
    public const byte F_ROBOT_LOST_FOUND_PICKABLE = 41;  // 参数：机器人id(int)
    public const byte M_ROBOT_PICK = 42;  // 参数：机器人id(int)
    public const byte F_PICKABLE_PICKED = 43;  // 参数：掉落物id(int)
    public const byte M_CREATE_TOWER = 44;  // 参数：队伍号(int), 信号塔的科技树编号(string), 位置(Vector2)
    public const byte F_ROBOT_LIDAR_SYNC = 45;  // 参数：队伍号(int), 同步点数n(int), n*同步点(int)
    public const byte M_TECHNOLOGY_RESEARCH = 46;  // 参数：队伍号(int), 科技点编号(string)
    public const byte M_PLAYER_ATTEND = 47;  // 参数：玩家(Player)
    public const byte M_PLAYER_READY = 48;  // 参数：玩家(Player), 准备状态(取消准备时为false)(bool)
    public const byte M_CHANGE_TEAM = 49;  // 参数：玩家(Player), 新的队号(int)
    public const byte M_LEAVE_MATCHING = 50;  // 参数：玩家(Player)
    public const byte F_PLAYER_LIST_UPDATED = 51;  // 参数：0队玩家数n(int), n*(0队第i个玩家(Player), 0队第i个玩家是否准备(bool)), 1队玩家数m(int), m*(1队第i个玩家(Player), 1队第i个玩家是否准备(bool))
    public const byte F_GAME_OVER = 52;  // 参数：胜利的队伍号(int)
    public const byte F_RESTART = 53;  // 无参数

    public delegate void GameEvent(object[] args);
    private static readonly GameEvent[] EVENTS = new GameEvent[COUNT_OF_EVENTS];

    static Events()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkEventCallback;
    }

    private static void NetworkEventCallback(EventData data)
    {
        if (data.Code >= COUNT_OF_EVENTS) return;
        if (data.CustomData is object[] args) {
			EVENTS[data.Code]?.Invoke(args);
            //Debug.Log($"Remote Event Invoke: id={data.Code}, args={args.ToStringFull()}");
		}
    }

    public static bool Invoke(byte eventId, object[] args, bool reliable = true)
    {
        if (eventId >= COUNT_OF_EVENTS) return false;
        //Debug.Log($"Local Event Invoke: id={eventId}, args={args.ToStringFull()}");
        PhotonNetwork.RaiseEvent(eventId, args, RaiseEventOptions.Default,
            reliable ? SendOptions.SendReliable : SendOptions.SendUnreliable);
        EVENTS[eventId]?.Invoke(args);
        return true;
    }

    public static bool AddListener(byte eventId, GameEvent callback)
    {
        if (eventId >= COUNT_OF_EVENTS) return false;
        EVENTS[eventId] += callback;
        return true;
    }

    public static bool RemoveListener(byte eventId, GameEvent callback)
    {
        if (eventId >= COUNT_OF_EVENTS) return false;
        EVENTS[eventId] -= callback;
        return true;
    }
}