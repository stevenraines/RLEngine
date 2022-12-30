using System.Text.Json;
using System.Text.Json.Serialization;
namespace RLEngine.Core
{

    public class GameComponent : IGameComponent
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string ComponentTypeName { get; set; }

        public Guid GameObjectId { get; set; }

        public IGameObject GameObject { get; set; }

        public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        public string Name
        {
            get
            {
                return (string)Data["name"];
            }
        }

        public GameComponent(IGameComponent gameComponent)
        {
            Id = gameComponent.Id;
            ComponentTypeName = gameComponent.ComponentTypeName;
            GameObjectId = gameComponent.GameObjectId;
            GameObject = gameComponent.GameObject;
            Data = gameComponent.Data;
        }

        public GameComponent()
        {
            ComponentTypeName = nameof(GameComponent);
        }

        public GameComponent(string name, object properties)
        {
            ComponentTypeName = nameof(GameComponent);
            Data["name"] = name;
            Data["properties"] = properties;
        }

        public string Serialized
        {
            get { return JsonSerializer.Serialize(Data); }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                var metaData = JsonSerializer.Deserialize<Dictionary<string, object>>(value);
                Data = metaData ?? new Dictionary<string, object>();
            }
        }
    }

}