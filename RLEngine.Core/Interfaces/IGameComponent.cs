namespace RLEngine.Core
{

    public interface IGameComponent
    {

        Guid GameObjectId { get; set; }
        public IGameObject GameObject { get; set; }

    }

}