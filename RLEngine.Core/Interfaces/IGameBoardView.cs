using System;
using System.Collections.Generic;
using RLEngine.Core.Enumerations;


namespace RLEngine.Core
{

    public interface IGameBoardView
    {

        IList<IGameBoardPosition> Positions { get; }


    }

}