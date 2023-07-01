
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Events;
using static RLEngine.Core.IGameLoop;

namespace RLEngine.Core
{

    public class GameLoop : IGameLoop
    { 

        public event GameTickProcessedHandler GameTickProcessed;

        public Guid Id { get; set; } = Guid.NewGuid();
        public GameLoopType Type { get; }
        public Guid GameBoardId { get; set; }
        public IGameBoard GameBoard { get; set; }
        public bool GameLoopRunning { get; set; } = false;
        public long GameTick { get; set; } = 0;

        public IList<IScheduledAction> ScheduledActions { get; } = new List<IScheduledAction>();

        public GameLoop() { }

        public GameLoop(GameLoopType type)
        {
            Type = type;
        }

        public void ScheduleAction(IScheduledAction scheduledAction)
        {
            scheduledAction.ExecuteAt = GameTick;
            if (!ScheduledActions.Any(a => a.OwnerId == scheduledAction.OwnerId))
                ScheduledActions.Add(scheduledAction);
        }

        public long ExecuteActions()
        {
            if (GameLoopRunning) return GameTick;

            GameLoopRunning = true;
            try
            {

                var executeStartTime = DateTime.UtcNow;

                var actionsToExecute = ScheduledActions.Where(x => x.ExecuteAt <= GameTick).ToList();

                var uniqueGameObjects = actionsToExecute.Select(a => a.OwnerId).Distinct();
                
                foreach (var gameObjectId in uniqueGameObjects)
                {
                    IScheduledAction scheduledAction = actionsToExecute.Where(a => a.OwnerId == gameObjectId).FirstOrDefault();
                    scheduledAction.Action.Execute();
                    ScheduledActions.Remove(scheduledAction);
                }

                var args = new GameTickProcessedEventArgs() {
                    EventsProcessed = actionsToExecute.Count,
                    GameTick = GameTick
                };


                GameTickProcessed.Invoke(this, args );
                GameTick += 1;

            }
            finally
            {
                GameLoopRunning = false;
            }

            return GameTick;

        }
    }

}