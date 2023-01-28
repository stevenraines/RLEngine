using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RLEngine.Core;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Generators;
using RLEngine.Core.Components;
using RLEngine.Core.Components.Scores;
using RLEngine.Core.Factories;
using RLEngine.Server.Infrastructure;
namespace RLEngine.Server
{
    public class GameServer
    {

        public const int TileSize = 16;
        public const int DrawDistance = 12;
        public const int GameSpeed = 250; // ms per tick
        public const int TicksBetweenSaves = 4;

        private readonly GameContext GameContext;
        private DbContextOptions<GameContext> DbOptions;
        public System.Timers.Timer GameTimer { get; private set; }
        public GameBoard GameBoard { get; set; }
        private IGameLoop GameLoop { get; set; }
        private IList<IGameObject> Players { get; set; } = new List<IGameObject>();

        public long GameTick { get { return GameBoard?.GameLoop?.GameTick ?? -1; } }

        private bool Saving { get; set; } = false;

        public GameServer(IServiceProvider services, DbContextOptions<GameContext> dbOptions, Guid? Id = null)
        {
            GameContext = new GameContext(dbOptions);
            DbOptions = dbOptions;

        }

        public async Task InitializeGameBoard()
        {

            Saving = false;
            await LoadGameBoard();

            GameTimer = new System.Timers.Timer(GameSpeed);
            GameTimer.Elapsed += async (sender, e) => await GameLoopTick(GameBoard);
            GameTimer.Start();

        }

        public async Task<bool> SaveGameBoard()
        {
            if (Saving) return false;
            Saving = true;
            var gameContext = GameContext;
            await gameContext.SaveChangesAsync();
            Saving = false;
            return true;

        }

        public IGameBoard ConfigureDefaultGameBoard()
        {
            GameLoop = new GameLoop(GameLoopType.Timed); ;

            var gameboard = new GameBoard(GameLoop);
            gameboard.AddGameObject(ItemFactory.CreateItem(gameboard, "Gem", new ItemComponentConfiguration() { Value = 100 }, 1, 1, 0));
            gameboard.AddGameObject(ItemFactory.CreateMeleeWeapon(gameboard, "Sword", new ItemComponentConfiguration() { Value = 10, ScoreModifiers = new List<ScoreModifierComponent>() { new ScoreModifierComponent(ScoreType.AttackScore, 2) } }, 2, 2, 0));
            gameboard.AddGameObject(ItemFactory.CreateItem(gameboard, "Torch", new ItemComponentConfiguration() { Value = 1 }, 2, 2, 0));
            gameboard.AddGameObject(ItemFactory.CreateItem(gameboard, "Helm", new ItemComponentConfiguration()
            {
                Value = 50,
                EquipmentSlots = new HashSet<EquipmentSlot>() { new EquipmentSlot("Head") },
                ScoreModifiers = new List<ScoreModifierComponent>() { new ScoreModifierComponent(ScoreType.DefenseScore, 2) }
            }, 4, -1, 0));
            gameboard.AddGameObject(ItemFactory.CreateHealthRing(gameboard, "Ring of Health", 5, 3, 1, 0));

            /*
                        gameboard.AddGameObjects(-roomSize / 2, -roomSize / 2, RectangleRoomGenerator.Generate(roomSize, roomSize));
            */
            gameboard.AddGameObject(GameObjectType.Wall, 1, 3, 0);
            gameboard.AddGameObject(GameObjectType.Wall, 3, 2, 0);

            gameboard.AddGameObject(GameObjectType.Monster, -2, -2, 0);

            return gameboard;
        }
        public async Task<IGameBoard> LoadGameBoard()
        {

            var gameContext = GameContext;

            GameBoard = await gameContext.GameBoards
                                  .Include(x => x.GameObjects)
                                    .ThenInclude(x => x.Messages)
                                 .Include(x => x.GameLoop)
                                 .AsSplitQuery()
                                 .OrderBy(x => x.Id)
                                 .FirstOrDefaultAsync();

            if (GameBoard == null)
            {
                await CreateGameBoard();
            }

            GameLoop = GameBoard.GameLoop;


            return GameBoard;
        }

        public async Task<bool> CreateGameBoard()
        {
            var gameContext = new GameContext(DbOptions);
            GameBoard = (GameBoard)ConfigureDefaultGameBoard();
            gameContext.GameBoards.Add((GameBoard)GameBoard);
            await gameContext.SaveChangesAsync();
            return true;
        }

        public async Task<IGameObject> RegisterPlayer(Guid Id)
        {

            var player = GameBoard.GetGameObject(Id);
            if (player != null) return player;

            player = GameBoard.AddGameObject(PlayerFactory.CreatePlayer(GameBoard), 0, 0, 0);
            await SaveGameBoard();
            Players.Add(player);
            return player;
        }

        public async Task<bool> GameLoopTick(IGameBoard gameBoard)
        {
            gameBoard.GameLoop.ExecuteActions();
            if (GameBoard.GameLoop.GameTick > 0 && GameBoard.GameLoop.GameTick % TicksBetweenSaves == 0)
                await SaveGameBoard();
            return true;
        }


        public bool IssuePlayerMoveCommand(IGameObject player, string command)
        {
            Direction moveDirection = Direction.None;
            if (command == "ArrowRight") moveDirection = Direction.East;
            if (command == "ArrowLeft") moveDirection = Direction.West;
            if (command == "ArrowDown") moveDirection = Direction.South;
            if (command == "ArrowUp") moveDirection = Direction.North;

            if (moveDirection != Direction.None)
            {
                var scheduledAction = new ScheduledAction(player.Id, new MoveAction(player, moveDirection));
                GameBoard.GameLoop.ScheduleAction(scheduledAction);

            }

            return true;

        }

        public IList<IGameObject> GetGameBoardObjectsToRender(int x, int y, int z)
        {
            var gameObjects = new List<IGameObject>();
            new GameBoardView(GameBoard, x - DrawDistance, y - DrawDistance, DrawDistance * 2, DrawDistance * 2, 0);
            var gameBoardView = new GameBoardView(GameBoard, x - DrawDistance, y - DrawDistance, DrawDistance * 2, DrawDistance * 2, 0);

            var allXs = gameBoardView.Positions.Select(p => p.X).Distinct().OrderBy(o => o).ToList();
            var allYs = gameBoardView.Positions.Select(p => p.Y).Distinct().OrderBy(o => o).ToList();

            foreach (var currentY in allYs)
            {
                foreach (var currentX in allXs)
                {

                    var pos = gameBoardView.Positions.Where(p => p.X == currentX && p.Y == currentY).First();
                    var posEntity = pos.GameObjects.OrderByDescending(x => x.Layer).FirstOrDefault() ?? new GameObject(GameBoard, GameObjectType.None, currentX, currentY, z, 0);
                    gameObjects.Add(posEntity);
                }
            }
            return gameObjects?.OrderBy(g => g.Y).ThenBy(g => g.X).ToList() ?? new List<IGameObject>();
        }


        public IList<IGameObject> GetItemsAtPosition(int x, int y, int z)
        {
            var position = GameBoard.GetGameBoardPosition(x, y, z);
            var itemObjects = position.GetGameObjectsWithComponent<ItemComponent>();

            return itemObjects;
        }

    }



}