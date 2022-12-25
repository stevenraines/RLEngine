using System;
using RLEngine.Core.Enumerations;

namespace RLEngine.Core
{

    public interface IScheduledAction
    {
        Guid Id { get; }
        long ExecuteAt { get; set; }
        Guid OwnerId { get; }
        IAction Action { get; }

    }

}