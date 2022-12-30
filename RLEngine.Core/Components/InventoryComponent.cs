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

        public IGameObject DropItem(IGameObject item)
        {

            if (item.ContainerGameObjectId != Id) return null;
            if (!item.HasComponent<ItemComponent>()) return null;
            item.ContainerGameObjectId = Guid.Empty;
            return item;

        }



    }
}