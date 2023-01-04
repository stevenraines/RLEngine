using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
namespace RLEngine.Core.Components
{

    public class EquipmentComponent : GameComponent
    {


        public ISet<IEquipmentSlot> Slots = new HashSet<IEquipmentSlot>();

        protected EquipmentComponent()
        {
        }

        public EquipmentComponent(ISet<IEquipmentSlot> slots)
        {
            Slots = slots;
        }

        public IGameObject EquipItem(IGameObject item, IEquipmentSlot slot)
        {

            if (!item.HasComponent<EquipableComponent>()) return null;
            item.ContainerGameObjectId = Id;
            return item;

        }

        public IGameObject UnequipItem(IGameObject item, IEquipmentSlot slot)
        {

            if (item.ContainerGameObjectId != Id) return null;
            if (!item.HasComponent<ItemComponent>()) return null;
            item.ContainerGameObjectId = Guid.Empty;

            return item;

        }

    }
}