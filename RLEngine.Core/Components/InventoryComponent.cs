using System.Collections.Generic;

namespace RLEngine.Core.Components
{
    public class InventoryComponent : GameComponentBase
    {

        public IList<IGameObject> Items { get; set; } = new List<IGameObject>();

        public InventoryComponent()
        {

        }

    }
}