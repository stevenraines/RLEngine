namespace RLEngine.Core.Components
{
    public class ItemComponent : GameComponent
    {

        public ItemComponent(string name, object properties) : base(name, properties)
        {
            ComponentTypeName = nameof(ItemComponent);
        }

    }
}