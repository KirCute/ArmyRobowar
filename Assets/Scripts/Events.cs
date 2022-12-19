using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public static class Events {
    public const byte F_BASE_HEALTH_CHANGED = 0;
    public const byte F_BASE_DESTROYED = 1;
    private const int COUNT_OF_EVENTS = 20;
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