using System.Collections.Generic;

namespace RLEngine.Core.Components
{
    public class EquipableComponent : GameComponent
    {

        public ISet<IEquipmentSlot> AcceptableSlots { get; set; } = new HashSet<IEquipmentSlot>();

        public EquipableComponent() { }

        public EquipableComponent(ISet<IEquipmentSlot> acceptableSlots) : base()
        {
            AcceptableSlots = acceptableSlots;
        }

    }
}