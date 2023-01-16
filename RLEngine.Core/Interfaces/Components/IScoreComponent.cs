using System;
using RLEngine.Core.Enumerations;

namespace RLEngine.Core
{


    public interface IScoreComponent
    {

        int Value { get; set; }

        int MaxValue { get; set; }

        int ModifiedMaxValue { get; }

        event EventHandler ValueChanged;

        event EventHandler MaxValueChanged;


    }

}