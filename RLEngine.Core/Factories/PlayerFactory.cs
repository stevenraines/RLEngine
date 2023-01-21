using RLEngine.Core;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Components;
using RLEngine.Core.Components.Scores;

namespace RLEngine.Core.Factories
{
    public static class PlayerFactory
    {

        public static IGameObject CreatePlayer(IGameBoard gameBoard)
        {
            var player = new GameObject(gameBoard, GameObjectType.Player, "Player1", 0, 0, 0, 10);
            player.AddComponent(new InventoryComponent());
            player.AddComponent(PlayerFactory.CreateStandardCreatureEquipmentComponent());
            player.AddComponent(new ScoreComponent(new HealthScore(), 5));
            player.AddComponent(new ScoreComponent(new AttackScore(), 10));
            player.AddComponent(new ScoreComponent(new DefenseScore(), 10));
            player.AddComponent(new ScoreComponent(new StrengthScore(), 10));
            player.AddComponent(new ScoreComponent(new DexterityScore(), 10));
            player.AddComponent(new ScoreComponent(new WisdomScore(), 10));
            player.AddComponent(new ScoreComponent(new IntelligenceScore(), 10));
            player.AddComponent(new ScoreComponent(new ConstitutionScore(), 10));
            player.AddComponent(new ScoreComponent(new CharismaScore(), 10));

            return player;
        }

        private static EquipmentComponent CreateStandardCreatureEquipmentComponent()
        {

            var equipmentSlots = new HashSet<EquipmentSlot>();
            equipmentSlots.Add(new EquipmentSlot("Melee Weapon"));
            equipmentSlots.Add(new EquipmentSlot("Alternate Melee Weapon"));
            equipmentSlots.Add(new EquipmentSlot("Left Hand"));
            equipmentSlots.Add(new EquipmentSlot("Right Hand"));
            equipmentSlots.Add(new EquipmentSlot("Left Arm"));
            equipmentSlots.Add(new EquipmentSlot("Right Arm"));
            equipmentSlots.Add(new EquipmentSlot("Head"));
            equipmentSlots.Add(new EquipmentSlot("Body"));
            equipmentSlots.Add(new EquipmentSlot("Neck"));
            equipmentSlots.Add(new EquipmentSlot("Legs"));
            equipmentSlots.Add(new EquipmentSlot("Feet"));
            return new EquipmentComponent(equipmentSlots);

        }

        public static IGameObject CreatePlayer(IGameBoard gameBoard, int x, int y, int z)
        {
            var player = CreatePlayer(gameBoard);
            gameBoard.AddGameObject(player, x, y, z);

            return player;
        }




    }
}