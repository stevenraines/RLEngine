namespace RLEngine.Core
{
    public class GameMessage : IGameMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid GameObjectId { get; set; }
        public IGameObject GameObject { get; set; }
        public string Message { get; set; }
        public long GameTick { get; set; }

        public GameMessage() { }
        public GameMessage(long gameTick, string message)
        {
            GameTick = gameTick;
            Message = message;
        }
    }
}