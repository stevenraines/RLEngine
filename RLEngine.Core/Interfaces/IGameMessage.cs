namespace RLEngine.Core
{

    public interface IGameMessage
    {

        public Guid Id { get; set; }
        public Guid GameObjectId { get; set; }
        public IGameObject GameObject { get; set; }
        string Message { get; set; }
        long GameTick { get; set; }

    }

}