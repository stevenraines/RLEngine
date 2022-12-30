using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
namespace RLEngine.Core
{

    public class GameComponent : IGameComponent
    {

        [JsonInclude]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonIgnore]
        public IGameObject GameObject { get; set; }

        public GameComponent()
        { }


    }

}