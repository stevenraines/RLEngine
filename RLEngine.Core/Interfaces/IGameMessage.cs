namespace RLEngine.Core
{

    public interface IGameMessage
    {
        string Message { get; set; }
        long GameTick { get; set; }

    }

}