using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class InventoryDialogBase : ComponentBase
    {
        [Parameter]
        public InventoryDialogMode Mode { get; set; } = InventoryDialogMode.Player;

        [Parameter]
        public bool Open { get; set; } = false;

        [Parameter]
        public IGameObject Player { get; set; } = null;

        [Parameter]
        public IList<IGameObject> Items { get; set; } = null;

        private InventoryComponent Inventory { get; set; }







    }

    public enum InventoryDialogMode
    {
        Player,
        GameBoard,
        Container

    }

}