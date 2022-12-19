using Model.Equipment.Template;

namespace Model.Equipment {
    public class Bag : BaseSensor<BagTemplate> {
        private BagTemplate priTemplate;

        public override BagTemplate template {
            get => priTemplate;
            set => priTemplate = value;
        }


        public Bag(int id, BagTemplate priTemplate) : base(id) {
            this.priTemplate = priTemplate;
        }

        public override void EquipOn(int robotId) {
            // TODO
        }

        public override void UnloadFrom(int robotId) {
            // TODO
        }
    }
}