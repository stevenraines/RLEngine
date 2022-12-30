namespace RLEngine.Core.Components
{
    public class ItemComponent : GameComponent
    {

        public decimal Value { get; set; }

        public ItemComponent() { }
        public ItemComponent(decimal value) : base()
        {
            Value = value;
        }

    }
}