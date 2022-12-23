using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public static class Events {
    private const int COUNT_OF_EVENTS = 21;
    
    public const byte F_GAME_START = 0;
    public const byte M_CREATE_ROBOT = 1;
    public const byte F_ROBOT_CREATED = 2;
    public const byte F_COMPONENT_DESTROYED = 3;
    public const byte F_COMPONENT_DAMAGE = 4;  // FIXME: Why
    public const byte F_COMPONENT_HEALTH_CHANGED = 4;
    public const byte F_BODY_DESTROYED = 5;
    public const byte F_BODY_HEALTH_CHANGED = 6;
    public const byte M_ROBOT_FIRE = 7;
    public const byte F_ROBOT_FIRED = 8;
    public const byte M_ROBOT_MOTIVATION_CHANGE = 9;
    public const byte F_ROBOT_MOTIVATION_CHANGE = 10;
    public const byte F_ROBOT_SEIZE_ENEMY = 11;
    public const byte F_ROBOT_LOST_SEIZE_ENEMY = 12;
    public const byte F_TOWER_HEALTH_CHANGED = 13;
    public const byte F_TOWER_DESTROYED = 14;
    public const byte M_CAPTURE_BASE = 15;
    public const byte F_BASE_DESTROYED = 16;  // 参数：基地id(int)
    public const byte M_BASE_CHANGE_HEALTH = 17;  // 参数：基地id(int), 血量变化量(int)
    public const byte F_BASE_HEALTH_CHANGED = 18;  // 参数：基地id(int), 当前血量(int)
    public const byte F_TEAM_ACQUIRE_COMPONENT = 19;
    public const byte F_TEAM_ACQUIRE_COINS = 20;
    
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

    public static bool Invoke(byte eventId, object[] args)
    {
        if (eventId >= COUNT_OF_EVENTS) return false;
        PhotonNetwork.RaiseEvent(eventId, args, RaiseEventOptions.Default, SendOptions.SendReliable);
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