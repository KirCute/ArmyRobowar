using Model.Equipment.Template;

namespace Model.Equipment {
    public class Bag : BaseSensor {
        private readonly BagTemplate bagTemplate;
        
        public Bag(int id, BagTemplate template) : base(id, template) {
            this.bagTemplate = template;
        }

        public override void EquipOn(int robotId) {
            // TODO
        }

        public override void UnloadFrom(int robotId) {
            // TODO
        }
    }
}