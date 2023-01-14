using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
namespace RLEngine.Core.Components
{

    public class EquipmentComponent : GameComponent
    {

        [JsonInclude]
        public HashSet<EquipmentSlot> Slots = new HashSet<EquipmentSlot>();

        protected EquipmentComponent()
        {
        }


        public EquipmentComponent(HashSet<EquipmentSlot> slots)
        {
            Slots = slots;
        }

        public IGameObject EquipItem(IGameObject item, IEquipmentSlot slot)
        {

            if (!item.HasComponent<EquipableComponent>()) return null;
            var acceptableSlots = item.GetComponent<EquipableComponent>().AcceptableSlots;

            if (acceptableSlots == null) return null;

            if (!acceptableSlots.Any(x => x.Name == slot.Name))
            {
                return null;
            }

            // remove the item if it was in another slot!
            Slots.Where(x => x.ItemId == item.Id).ToList().ForEach(y => y.ItemId = null);
            slot.ItemId = item.Id;
            return item;

        }

        public IEquipmentSlot UnequipItem(IEquipmentSlot slot)
        {
            slot.ItemId = null;
            return slot;
        }

    }
}