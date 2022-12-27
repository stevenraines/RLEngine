using System;
using System.Collections.Generic;
using System.Linq;
using RLEngine.Core.Enumerations;


namespace RLEngine.Core
{

    public class GameBoardView : IGameBoardView
    {

        public IList<IGameBoardPosition> Positions { get; } = new List<IGameBoardPosition>();


        public GameBoardView(IGameBoard gameBoard, int startX, int startY, int widthX, int widthY, int z)
        {
            for (var xPos = startX; xPos < startX + widthX + 1; xPos++)
            {
                for (var yPos = startY; yPos < startY + widthY + 1; yPos++)
                {
                    Positions.Add(gameBoard.GetGameBoardPosition(xPos, yPos, z));
                }
            }

        }

    }

}