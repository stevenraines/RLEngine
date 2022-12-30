namespace RLEngine.Core
{

    public interface IGameComponent
    {
        public Guid Id { get; set; }
        public Guid GameObjectId { get; set; }
        public IGameObject GameObject { get; set; }
        public string ComponentTypeName { get; set; }
        IDictionary<string, object> Data { get; set; }

    }

}