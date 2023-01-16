using RLEngine.Core;

namespace RLEngine.Core.Events
{

    public class ValueChangedEventArgs : EventArgs
    {
        public int Value { get; set; }
    }
}