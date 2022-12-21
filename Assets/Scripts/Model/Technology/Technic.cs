using Model.Equipment;
using UnityEngine;

namespace Model.Technology {
    public class Technic {
        public delegate void UnlockDelegate(Team team);
        
        public readonly string name;
        public readonly string description;
        public readonly UnlockDelegate Unlock;

        public Technic(string name, string description, UnlockDelegate unlockFunc) {
            this.name = name;
            this.description = description;
            this.Unlock = unlockFunc;
        }
    }
}