using RLEngine.Core.Components.Scores;

namespace RLEngine.Core.Components
{
    public class ItemComponentConfiguration : GameComponent
    {

        public decimal Value { get; set; } = 0.00m;
        public List<ScoreModifierComponent> ScoreModifiers { get; set; } = new List<ScoreModifierComponent>();
        public HashSet<EquipmentSlot> EquipmentSlots = new HashSet<EquipmentSlot>();
    }
}