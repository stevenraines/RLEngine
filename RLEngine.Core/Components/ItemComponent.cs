namespace RLEngine.Core.Components
{
    public class ItemComponent : GameComponent
    {

        public string Name
        {
            get
            {
                return (string)Data["name"];
            }
        }

        public ItemComponent(string name, object properties)
        {

            Data["name"] = name;
            Data["properties"] = properties;
        }



    }
}