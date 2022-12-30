namespace RLEngine.Core
{

    public interface IGameComponent
    {
        public Guid Id { get; set; }

        public IGameObject GameObject { get; set; }

    }

}