using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public static class Events {
    private const int COUNT_OF_EVENTS = 34;
    
    public const byte F_GAME_START = 0;
    public const byte M_CREATE_ROBOT = 1;  // 参数：队伍号(int), 底盘的科技树编号(string), 机器人的名称(string)
    public const byte F_ROBOT_CREATED = 2;  // 参数：队伍号(int), 机器人id(int)
    public const byte F_COMPONENT_DESTROYED = 3;  // 参数：机器人id(int), 配件所在格子号(int)
    public const byte M_COMPONENT_DAMAGE = 4;  // 参数：机器人id(int), 配件所在格子号(int), 血量变化量(int)
    public const byte F_COMPONENT_HEALTH_CHANGED = 5;  // 参数：机器人id(int), 配件所在格子号(int)，配件当前血量(int)
    public const byte F_BODY_DESTROYED = 6;  // 参数：机器人id(int)
    public const byte M_BODY_DAMAGE = 7;  // 参数：机器人id(int), 血量变化量(int)
    public const byte F_BODY_HEALTH_CHANGED = 8;  // 参数：机器人id(int), 底盘当前血量(int)
    public const byte M_ROBOT_FIRE = 9;  // 参数：机器人id(int)
    public const byte F_ROBOT_FIRED = 10;  // 参数：机器人id(int), 机器人炮口位置(Vector3), 机器人击中点(Vector3)
    public const byte M_ROBOT_MOTIVATION_CHANGE = 11;  // 参数：机器人id(int), 前后键按下|前后键释放，向前|向后，左右键按下|左右键释放，向左|向右（二进制的后四位，前1后0）(int)
    public const byte F_ROBOT_MOTIVATION_CHANGED = 12;  // 参数：机器人id(int), 前后键按下|前后键释放，向前|向后，左右键按下|左右键释放，向左|向右（二进制的后四位，前1后0）(int)
    public const byte M_ROBOT_TOWARDS_CHANGE = 13;  // 参数：机器人id(int), 炮口和摄像头朝向(Vector3)
    public const byte F_ROBOT_TOWARDS_CHANGED = 14;  // 参数：机器人id(int), 炮口和摄像头朝向(Vector3)
    public const byte F_ROBOT_SEIZE_ENEMY = 15;  // (unreliable) 参数：己方队伍号(int), 被索敌敌方机器人id(int), 敌方机器人位置(Vector3), 敌方机器人朝向(Vector3)
    public const byte F_ROBOT_LOST_SEIZE_ENEMY = 16;  // 参数：己方队伍号(int), 被索敌敌方机器人id(int)
    public const byte M_ROBOT_CHANGE_CONNECTION = 17;  // 参数：机器人id(int), 信号强度变化量(int)
    public const byte F_ROBOT_WEAK_CONNECTION = 18;  // 参数：机器人id(int)
    public const byte F_ROBOT_LOST_CONNECTION = 19;  // 参数：机器人id(int)
    public const byte F_ROBOT_STRONG_CONNECTION = 20;  // 参数：机器人id(int)
    public const byte M_ROBOT_MONITOR = 21;  // 参数：机器人id(int), 申请查看摄像头玩家(Player), 查看(true)或退出查看(false)(bool)
    public const byte M_ROBOT_CONTROL = 22;  // 参数：机器人id(int), 机器人当前控制者(Player, 若无为null)
    public const byte M_TOWER_DAMAGE = 23;  // 参数：信号塔id(int), 血量变化量(int)
    public const byte F_TOWER_HEALTH_CHANGED = 24;  // 参数：信号塔id(int), 信号塔当前血量(int)
    public const byte F_TOWER_DESTROYED = 25;  // 参数：信号塔id(int)
    public const byte M_CAPTURE_BASE = 26;  // 参数：基地id(int), 队伍号(int)
    public const byte M_BASE_DAMAGE = 27;  // 参数：基地id(int), 血量变化量(int)
    public const byte F_BASE_HEALTH_CHANGED = 28;  // 参数：基地id(int), 当前血量(int)
    public const byte F_BASE_DESTROYED = 29;  // 参数：基地id(int)
    public const byte F_TEAM_ACQUIRE_COMPONENT = 30;  // 参数：队伍号(int), 部件的科技树编号(string), 部件的血量(int)
    public const byte F_TEAM_ACQUIRE_COINS = 31;  // 参数：队伍号(int), 资源数量(int)
    public const byte M_TEAM_BUY_COMPONENT = 32;  // 参数：队伍号(int), 部件的科技树编号(string)
    public const byte F_MAP_ROBOT_ENTER_AREA = 33;
    
    public delegate void GameEvent(object[] args);
    private static readonly GameEvent[] EVENTS = new GameEvent[COUNT_OF_EVENTS];

    static Events()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkEventCallback;
    }

    private static void NetworkEventCallback(EventData data)
    {
        if (data.Code >= COUNT_OF_EVENTS) return;
        if (data.CustomData is object[] args) EVENTS[data.Code]?.Invoke(args);
    }

    public static bool Invoke(byte eventId, object[] args, bool reliable = true)
    {
        if (eventId >= COUNT_OF_EVENTS) return false;
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