using System;
using System.Collections.Generic;
using RLEngine.Core.Enumerations;


namespace RLEngine.Core
{

    public interface IGameBoardPosition
    {
        int X { get; }
        int Y { get; }
        int Z { get; }

        IList<IGameObject> GameObjects { get; }
        bool IsNavigable();
        IList<IGameObject> GetGameObjectsWithComponent<T>();
    }

}