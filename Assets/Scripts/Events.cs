using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public static class Events {
    private const int COUNT_OF_EVENTS = 37;

    public static byte F_COMPONENT_DAMAGE = 1;
    public static byte F_COMPONENT_DESTROYED = 2;
    public static byte F_COMPONENT_HEALTH_CHANGED = 3;
    public static byte F_BODY_DESTROYED = 4;
    public static byte F_BODY_HEALTH_CHANGED = 5;
    public static byte M_ROBOT_FIRE = 8;
    public static byte F_ROBOT_FIRED = 9;
    public static byte F_TOWER_DESTROYED = 32;
    public static byte F_TOWER_HEALTH_CHANGED = 33;
    public static byte F_BASE_DESTROYED = 36;
    public static byte F_BASE_HEALTH_CHANGED = 37;
    
    public delegate void GameEvent(object[] args);
    private static readonly GameEvent[] EVENTS = new GameEvent[COUNT_OF_EVENTS];

    static Events() {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkEventCallback;
    }

    private static void NetworkEventCallback(EventData data) {
        if (data.Code >= COUNT_OF_EVENTS) return;
        if (data.CustomData is object[] args) EVENTS[data.Code]?.Invoke(args);
    }

    public static bool Invoke(byte eventId, object[] args) {
        if (eventId >= COUNT_OF_EVENTS) return false;
        PhotonNetwork.RaiseEvent(eventId, args, RaiseEventOptions.Default, SendOptions.SendReliable);
        EVENTS[eventId]?.Invoke(args);
        return true;
    }

    public static bool AddListener(byte eventId, GameEvent callback) {
        if (eventId >= COUNT_OF_EVENTS) return false;
        EVENTS[eventId] += callback;
        return true;
    }

    public static bool RemoveListener(byte eventId, GameEvent callback) {
        if (eventId >= COUNT_OF_EVENTS) return false;
        EVENTS[eventId] -= callback;
        return true;
    }
}