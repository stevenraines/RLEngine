namespace RLEngine.Core
{

    public interface IGameComponent
    {

        Guid GameObjectId { get; set; }
        IGameObject GameObject { get; set; }
        IDictionary<string, object> Data { get; set; }


    }

}