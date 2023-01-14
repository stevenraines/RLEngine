using System;
using RLEngine.Core.Enumerations;

namespace RLEngine.Core
{

    public interface IEquipmentSlot
    {
        string Name { get; set; }
        Guid? ItemId { get; set; }

    }

}