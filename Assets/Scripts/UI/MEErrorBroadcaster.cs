using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MEErrorBroadcaster : MonoBehaviour {
        private const float SINGLE_BROADCAST_TIME = 1.0F;
        
        private readonly List<string> longTermMessage = new();
        private int broadcastingLongTerm = -1;
        private float lastBroadcastTime;
        private Text text;

        private void Awake() {
            text = GetComponentInChildren<Text>();
        }

        private void Update() {
            if (broadcastingLongTerm == -1) return;
            if (Time.time - lastBroadcastTime >= SINGLE_BROADCAST_TIME) {
                if (longTermMessage.Count == 0) {
                    broadcastingLongTerm = -1;
                    text.text = "";
                } else {
                    broadcastingLongTerm++;
                    if (broadcastingLongTerm >= longTermMessage.Count) broadcastingLongTerm = 0;
                    text.text = longTermMessage[broadcastingLongTerm];
                    lastBroadcastTime = Time.time;
                }
            } else {
                var color = text.color;
                color.a = Mathf.Sin((Time.time - lastBroadcastTime) * Mathf.PI / SINGLE_BROADCAST_TIME);
                text.color = color;
            }
        }

        public void AddLongTermMessage(string message) {
            longTermMessage.Add(message);
            if (broadcastingLongTerm == -1) broadcastingLongTerm = 0;
        }

        public void RemoveLongTermMessage(string message) {
            if (longTermMessage.Contains(message)) longTermMessage.Remove(message);
        }

        public void Broadcast(string message) {
            if (broadcastingLongTerm == -1) broadcastingLongTerm = 0;
            lastBroadcastTime = Time.time;
            text.text = message;
        }
    }
}