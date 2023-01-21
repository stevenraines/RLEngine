using System.Collections.Generic;

namespace RLEngine.Core.Components
{
    public class EquipableComponent : GameComponent
    {

        public ISet<EquipmentSlot> AcceptableSlots { get; set; } = new HashSet<EquipmentSlot>();

        public EquipableComponent() { }

        public EquipableComponent(HashSet<EquipmentSlot> acceptableSlots) : base()
        {
            AcceptableSlots = acceptableSlots;
        }

    }
}