using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class HistoryTerminalBase : ComponentBase
    {


        [Parameter] public EventCallback<string> OnCommandChange { get; set; }

        [Parameter] public EventCallback OnClick { get; set; }

        protected ElementReference inputRef;

        [Parameter]
        public IGameObject Player { get; set; } = null;


        protected string _CommandText;

        [Parameter]
        public string CommandText
        {
            get { return _CommandText; }
            set
            {
                if (_CommandText == value)
                    return;

                _CommandText = value.Trim();
                if (!String.IsNullOrWhiteSpace(_CommandText))
                {

                    OnCommandChange.InvokeAsync(_CommandText);
                    _CommandText = "";
                }


            }
        }

        private string localCommandText { get; set; }

        public string Messages { get; private set; } = "";


        protected override void OnInitialized()
        {

        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (Player != null)
                Messages = String.Join("\n",  Player.Messages.Where(x => !String.IsNullOrWhiteSpace(x.Message))
                                                            .OrderByDescending(x => x.GameTick)
                                                            .Take(10)
                                                            .OrderBy(x => x.GameTick)
                                                            .Select(x => x.Message)
                                                            .ToList());



        }

        public async void Focus()
        {
            await inputRef.FocusAsync();
        }


    }

}