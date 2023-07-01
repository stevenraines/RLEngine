using RLEngine.Core;

namespace RLEngine.Core.Events
{

    public class GameTickProcessedEventArgs : System.EventArgs
    {
        public int EventsProcessed { get; set; }
        public long GameTick { get; set; }
    }
}


