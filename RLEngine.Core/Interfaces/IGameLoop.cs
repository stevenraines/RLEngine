using RLEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using RLEngine.Core.Enumerations;

namespace RLEngine.Core
{

    public interface IGameLoop
    {
        GameLoopType Type { get; }
        long GameTick { get; set; }
        IList<IScheduledAction> ScheduledActions { get; }
        Guid GameBoardId { get; set; }
        IGameBoard GameBoard { get; set; }
        void ScheduleAction(IScheduledAction scheduledAction);
        long ExecuteActions();
    }

}