using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace RLEngine.Core.Components
{
    public class InventoryComponent : GameComponent
    {

        [NotMapped]
        public IList<IGameObject> Items { get; set; } = new List<IGameObject>();

        public InventoryComponent()
        {

        }

    }
}