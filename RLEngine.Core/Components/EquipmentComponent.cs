using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using RLEngine.Core.Components.Scores;
using System.Collections.Generic;
namespace RLEngine.Core.Components
{

    public class EquipmentComponent : GameComponent
    {

        [JsonInclude]
        public HashSet<EquipmentSlot> Slots = new HashSet<EquipmentSlot>();

        protected EquipmentComponent()
        {
        }


        public EquipmentComponent(HashSet<EquipmentSlot> slots)
        {
            Slots = slots;
        }

        public IGameObject EquipItem(IGameObject item, IEquipmentSlot slot)
        {

            if (!item.HasComponent<EquipableComponent>()) return null;
            var acceptableSlots = item.GetComponent<EquipableComponent>().AcceptableSlots;

            if (acceptableSlots == null) return null;

            if (!acceptableSlots.Any(x => x.Name == slot.Name))
            {
                return null;
            }

            // remove the item if it was in another slot!
            Slots.Where(x => x.ItemId == item.Id).ToList().ForEach(y => y.ItemId = null);
            slot.ItemId = item.Id;
            return item;

        }

        public IEquipmentSlot UnequipItem(IEquipmentSlot slot)
        {
            slot.ItemId = null;
            return slot;
        }

        public List<ScoreModifierComponent> GetScoreModifierComponents<T>()
        {

            var scoreModifierComponentItems = GetEquippedItemsByComponent<ScoreModifierComponent>();
            return scoreModifierComponentItems.Select(x => (ScoreModifierComponent)x.Components.Where(x => x.Value.GetType() == typeof(ScoreModifierComponent))).Where(x => x.ScoreType.GetType() == typeof(T)).ToList();

        }


        public List<IGameObject> GetEquippedItemsByComponent<T>()
        {

            var gameObjectsWithComponent = new List<IGameObject>();

            var equipmentGameObjectIds = GameObject.GetComponent<EquipmentComponent>().Slots.Where(x => x.ItemId != null).Select(x => x.ItemId).ToList();
            var gameObjects = GameObject.GameBoard.GameObjects.Where(x => equipmentGameObjectIds.Contains(x.Id)).ToList();

            foreach (var gameObject in gameObjects)
            {

                foreach (var component in gameObject.Components)
                {
                    var x = component.Value.GetType();
                    var y = typeof(ScoreModifierComponent);
                    var z = (IDictionary<string, dynamic>)component.Value.GameObject.Components;
                    var scoreComponents = gameObject.Components.Where(x => x.Value.GetType() == typeof(ScoreModifierComponent));
                    var aa = scoreComponents.Any(x => x.Value.ScoreType == typeof(T));
                }


                if (gameObject.Components.Any(x => x.Value.GetType() == typeof(ScoreModifierComponent)))
                    gameObjectsWithComponent.Add(gameObject);

            }

            return gameObjectsWithComponent;

        }




    }
}