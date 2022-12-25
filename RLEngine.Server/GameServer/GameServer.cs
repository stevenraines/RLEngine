using System;
using System.Timers;
using RLEngine.Core;
using RLEngine.Core.Enumerations;

namespace RLEngine.Server
{
    public class GameServer
    {

        private System.Timers.Timer GameTimer { get; set; }
        private GameBoard GameBoard { get; set; }
        private GameLoop GameLoop { get; set; }
        public long GameTick { get { return GameBoard?.GameLoop?.GameTick ?? -1; } }
        public GameServer()
        {

            GameLoop = new GameLoop(GameLoopType.Timed);
            GameBoard = new GameBoard(GameLoop);
            GameTimer = new System.Timers.Timer(1000);
            GameTimer.Elapsed += (sender, e) => GameLoopTick(GameBoard);
            GameTimer.Start();
        }

        public static void GameLoopTick(GameBoard gameBoard)
        {
            gameBoard.GameLoop.ExecuteActions();



        }

    }
}