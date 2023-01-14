using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
namespace RLEngine.Core.Components
{

    public class InventoryComponent : GameComponent
    {

        public InventoryComponent()
        {
        }

        [JsonIgnore]
        public IList<IGameObject> Items
        {
            get
            {
                return GameObject.GameBoard.GameObjects.Where(x => x.ContainerGameObjectId == Id).ToList();
            }
        }

        public IGameObject PickupItem(IGameObject item)
        {

            if (!item.HasComponent<ItemComponent>()) return null;
            item.ContainerGameObjectId = Id;
            return item;

        }

        public IGameObject DropItem(IGameObject item, int x, int y, int z)
        {

            if (item.ContainerGameObjectId != Id) return null;
            if (!item.HasComponent<ItemComponent>()) return null;
            item.ContainerGameObjectId = Guid.Empty;
            item.X = x;
            item.Y = y;
            item.Z = z;
            return item;

        }


        public IList<IGameObject> GetEquipableItems(IEquipmentSlot slot)
        {

            if (slot == null) return null;

            return Items.Where(x => x.GetComponent<EquipableComponent>() != null
                        && x.GetComponent<EquipableComponent>().AcceptableSlots.Any(x => x.Name == slot.Name)).ToList();

        }


    }
}