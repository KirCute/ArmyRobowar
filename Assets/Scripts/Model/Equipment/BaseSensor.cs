using Model.Equipment.Template;
using UnityEngine;

namespace Model.Equipment {
    public abstract class BaseSensor {
        public const byte STATUS_REST = 0;
        public const byte STATUS_EQUIPPED = 1;
        public const byte STATUS_DROPPED = 2;
        public const byte STATUS_PICKED = 3;

        public readonly SensorTemplate template;
        public byte status { get; set; }
        public readonly int id;
        
        public BaseSensor(int id, SensorTemplate template) {
            this.id = id;
            this.template = template;
            this.status = STATUS_REST;
        }

        public abstract void EquipOn(int robotId);

        public abstract void UnloadFrom(int robotId);
    }
}