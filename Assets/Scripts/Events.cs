using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public static class Events {

    /*private const int COUNT_OF_EVENTS = 37;
    
    public const byte F_GAME_START = 0;
    public const byte F_COMPONENT_DAMAGE = 1;
    public const byte F_COMPONENT_DESTROYED = 2;
    public const byte F_COMPONENT_HEALTH_CHANGED = 3;
    public const byte F_BODY_DESTROYED = 4;
    public const byte F_BODY_HEALTH_CHANGED = 5;
    public const byte M_ROBOT_FIRE = 8;
    public const byte F_ROBOT_FIRED = 9;
    public const byte F_TOWER_DESTROYED = 32;
    public const byte F_TOWER_HEALTH_CHANGED = 33;
    public const byte M_CREATE_ROBOT = 34;
    public const byte F_ROBOT_CREATED = 18;
    public const byte F_BASE_DESTROYED = 35;
    public const byte F_BASE_HEALTH_CHANGED = 36;
    public const byte M_ROBOT_MOTIVATION_CHANGE = 14;
    public const byte F_ROBOT_SEIZE_ENEMY = 6;
    public const byte F_ROBOT_LOST_SEIZE_ENEMY = 7;
    public const byte F_ROBOT_MOTIVATION_CHANGE = 21;
    public const byte F_ROBOT_ACQUIRED_CONNECTION = 16;
    public const byte F_ROBOT_WEAK_CONNECTION = 23;
    public const byte F_ROBOT_LOST_CONNECTION = 20;
    public const byte F_ROBOT_CONNECTION = 37;
    public const byte F_TEAM_ACQUIRE_COMPONENT = 18;
    public const byte F_TEAM_ACQUIRE_COINS = 19;*/


    private const int COUNT_OF_EVENTS = 33;
    
    public const byte F_GAME_START = 0;
    public const byte M_CREATE_ROBOT = 1;
    public const byte F_ROBOT_CREATED = 2;
    public const byte F_COMPONENT_DESTROYED = 3;
    public const byte M_COMPONENT_DAMAGE = 4;
    public const byte F_COMPONENT_HEALTH_CHANGED = 5;
    public const byte F_BODY_DESTROYED = 6;
    public const byte M_BODY_DAMAGE = 7;
    public const byte F_BODY_HEALTH_CHANGED = 8;
    public const byte M_ROBOT_FIRE = 9;
    public const byte F_ROBOT_FIRED = 10;
    public const byte M_ROBOT_MOTIVATION_CHANGE = 11;
    public const byte F_ROBOT_MOTIVATION_CHANGED = 12;
    public const byte M_ROBOT_TOWARDS_CHANGE = 13;
    public const byte F_ROBOT_TOWARDS_CHANGED = 14;
    public const byte F_ROBOT_SEIZE_ENEMY = 15;
    public const byte F_ROBOT_LOST_SEIZE_ENEMY = 16;
    public const byte M_ROBOT_CHANGE_CONNECTION = 17;
    public const byte F_ROBOT_WEAK_CONNECTION = 18;
    public const byte F_ROBOT_LOST_CONNECTION = 19;
    public const byte F_ROBOT_STRONG_CONNECTION = 20;
    public const byte M_ROBOT_MONITOR = 21;
    public const byte M_ROBOT_CONTROL = 22;
    public const byte M_TOWER_DAMAGE = 23;
    public const byte F_TOWER_HEALTH_CHANGED = 242;
    public const byte F_TOWER_DESTROYED = 25;
    public const byte M_CAPTURE_BASE = 26;
    public const byte M_BASE_DAMAGE = 27;  // 参数：基地id(int), 血量变化量(int)
    public const byte F_BASE_HEALTH_CHANGED = 28;  // 参数：基地id(int), 当前血量(int)
    public const byte F_BASE_DESTROYED = 29;  // 参数：基地id(int)
    public const byte F_TEAM_ACQUIRE_COMPONENT = 30;
    public const byte F_TEAM_ACQUIRE_COINS = 31;
    public const byte F_MAP_ROBOT_ENTER_AREA = 32;
    public const byte F_COMPONENT_DAMAGE = 33;
    
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