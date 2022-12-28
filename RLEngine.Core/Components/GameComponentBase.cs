namespace RLEngine.Core.Components
{
    public class GameComponentBase : IGameComponent
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid GameObjectId { get; set; }
        public IGameObject GameObject { get; set; }

        public GameComponentBase() { }

        public GameComponentBase(IGameObject gameObject)
        {
            GameObject = gameObject;
            GameObjectId = gameObject.Id;

        }
    }
}