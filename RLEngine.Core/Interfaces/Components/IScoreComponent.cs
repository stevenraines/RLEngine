using System;
using RLEngine.Core.Enumerations;

namespace RLEngine.Core
{


    public interface IScoreComponent
    {

        int Value { get; set; }

        int MaxValue { get; set; }
        int CurrentValue { get; }
        int CurrentMaxValue { get; }

        event EventHandler ValueChanged;

        event EventHandler MaxValueChanged;


    }

}