namespace RLEngine.Core
{

    public class GameComponent : IGameComponent
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid GameObjectId { get; set; }

        public IGameObject GameObject { get; set; }

        public GameComponent() { }

    }

}