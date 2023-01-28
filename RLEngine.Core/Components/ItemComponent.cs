using System.Text.Json.Serialization;
namespace RLEngine.Core.Components
{
    public class ItemComponent : GameComponent
    {

        [JsonInclude]
        public decimal Value { get; set; }

        public ItemComponent() { }

        public ItemComponent(decimal value) : base()
        {
            Value = value;
        }

    }
}