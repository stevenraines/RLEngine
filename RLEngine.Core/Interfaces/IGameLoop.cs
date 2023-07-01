using RLEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Events;

namespace RLEngine.Core
{

    public interface IGameLoop
    {
        public event GameTickProcessedHandler GameTickProcessed;
        public delegate void GameTickProcessedHandler(object? sender, GameTickProcessedEventArgs e);

        GameLoopType Type { get; }
        long GameTick { get; set; }
        IList<IScheduledAction> ScheduledActions { get; }
        Guid GameBoardId { get; set; }
        IGameBoard GameBoard { get; set; }
        void ScheduleAction(IScheduledAction scheduledAction);
        long ExecuteActions();
    }

}