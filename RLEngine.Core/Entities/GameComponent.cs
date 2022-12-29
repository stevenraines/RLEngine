using System.Text.Json;
using System.Text.Json.Serialization;
namespace RLEngine.Core
{

    public class GameComponent : IGameComponent
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid GameObjectId { get; set; }

        public IGameObject GameObject { get; set; }

        public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();


        public GameComponent() { }
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