using RLEngine.Core;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Components;
using RLEngine.Core.Components.Scores;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace RLEngine.Core.Factories
{
    public static class ItemFactory
    {

        public static IGameObject CreateItem(IGameBoard gameBoard,
                                                string name,
                                                ItemComponentConfiguration configuration,
                                                int x,
                                                int y,
                                                int z)
        {
            var item = new GameObject(gameBoard, GameObjectType.Item, name, x, y, z, 1);

            item.AddComponent(new ItemComponent(configuration.Value));

            // add any score bonsuses
            if (configuration.ScoreModifiers != null)
            {
                foreach (var modifier in configuration.ScoreModifiers)
                {
                    item.AddComponent(modifier);
                }
            }
            // add equipment slots
            if (configuration.EquipmentSlots != null)
            {
                item.AddComponent(new EquipableComponent(configuration.EquipmentSlots));
            }

            return item;
        }

        public static IGameObject CreateMeleeWeapon(IGameBoard gameBoard,
                                               string name,
                                               ItemComponentConfiguration configuration,
                                               int x,
                                               int y,
                                               int z)
        {



            var availableSlots = new HashSet<EquipmentSlot>();
            availableSlots.Add(new EquipmentSlot("Melee Weapon"));
            availableSlots.Add(new EquipmentSlot("Alternate Melee Weapon"));
            configuration.EquipmentSlots = availableSlots;

            var item = ItemFactory.CreateItem(gameBoard, name, configuration, 1, 1, 0);



            return item;
        }
        public static IGameObject CreateArmor(IGameBoard gameBoard,
                                              string name,
                                              ItemComponentConfiguration configuration,
                                              int x,
                                              int y,
                                              int z)
        {



            var availableSlots = new HashSet<EquipmentSlot>();
            availableSlots.Add(new EquipmentSlot("Melee Weapon"));
            availableSlots.Add(new EquipmentSlot("Alternate Melee Weapon"));
            configuration.EquipmentSlots = availableSlots;

            var item = ItemFactory.CreateItem(gameBoard, name, configuration, 1, 1, 0);



            return item;
        }


        public static IGameObject CreateWeapon(IGameBoard gameBoard,
                                                string name,
                                                dynamic properties,
                                                int x,
                                                int y,
                                                int z)
        {
            var item = new GameObject(gameBoard, GameObjectType.Item, name, x, y, z, 1);
            item.AddComponent(new ItemComponent(100));
            var validSlots = new HashSet<EquipmentSlot>();
            validSlots.Add(new EquipmentSlot("Melee Weapon"));
            validSlots.Add(new EquipmentSlot("Alternate Melee Weapon"));
            item.AddComponent(new EquipableComponent(validSlots));
            return item;
        }

        public static IGameObject CreateHealthRing(IGameBoard gameBoard,
                                              string name,
                                              int value,
                                              int x,
                                              int y,
                                              int z)
        {
            var item = new GameObject(gameBoard, GameObjectType.Item, name, x, y, z, 1);
            item.AddComponent(new ItemComponent(100));
            var validSlots = new HashSet<EquipmentSlot>();
            validSlots.Add(new EquipmentSlot("Left Hand"));
            validSlots.Add(new EquipmentSlot("Right Hand"));
            item.AddComponent(new EquipableComponent(validSlots));
            item.AddComponent(new ScoreModifierComponent(ScoreType.HealthScore, 5));
            return item;
        }

    }

}