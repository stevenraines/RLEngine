using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RLEngine.Core;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Generators;
using RLEngine.Core.Components;
using RLEngine.Core.Factories;
using RLEngine.Server.Infrastructure;
namespace RLEngine.Server
{
    public class GameServer
    {

        public const int TileSize = 16;
        public const int DrawDistance = 12;
        public const int GameSpeed = 250;


        private readonly GameContext GameContext;
        public System.Timers.Timer GameTimer { get; private set; }
        public IGameBoard GameBoard { get; set; }
        private IGameLoop GameLoop { get; set; }
        private IList<IGameObject> Players { get; set; } = new List<IGameObject>();

        public long GameTick { get { return GameBoard?.GameLoop?.GameTick ?? -1; } }

        public GameServer(IServiceProvider services, DbContextOptions<GameContext> dbOptions, Guid? Id = null)
        {

            GameContext = new GameContext(dbOptions);


            Task.Run(InitializeGameBoard).Wait();
        }

        public async Task InitializeGameBoard()
        {

            await LoadGameBoard();
            GameTimer = new System.Timers.Timer(GameSpeed);
            GameTimer.Elapsed += async (sender, e) => await GameLoopTick(GameBoard);
            GameTimer.Start();

        }

        public async Task<bool> SaveGameBoard()
        {

            GameContext.GameBoards.Update((GameBoard)GameBoard);
            await GameContext.SaveChangesAsync();

            return true;

        }

        public async Task<IGameBoard> LoadGameBoard()
        {

            GameBoard = await GameContext.GameBoards
                                 .Include(x => x.GameObjects)
                                  //   .ThenInclude(x => x.Components)
                                  .Include(x => x.GameObjects)
                                    .ThenInclude(x => x.Messages)
                                 .Include(x => x.GameLoop)
                                 .AsSplitQuery()
                                 .OrderBy(x => x.Id)
                                 .FirstOrDefaultAsync();

            if (GameBoard == null)
            {
                GameLoop = new GameLoop(GameLoopType.Timed); ;
                GameBoard = new GameBoard(GameLoop);
                var roomSize = 20;

                GameBoard.AddGameObject(ItemFactory.CreateItem(GameBoard, "Gem", new { value = 10 }, 1, 1, 0));
                GameBoard.AddGameObjects(-roomSize / 2, -roomSize / 2, RectangleRoomGenerator.Generate(roomSize, roomSize));
                GameBoard.AddGameObject(GameObjectType.Wall, 1, 3, 0);
                GameBoard.AddGameObject(GameObjectType.Wall, 3, 2, 0);
                GameBoard.AddGameObject(GameObjectType.Monster, -2, -2, 0);

                GameContext.GameBoards.Add((GameBoard)GameBoard);
                await GameContext.SaveChangesAsync();
            }

            GameLoop = GameBoard.GameLoop;


            return GameBoard;
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
            await gameBoard.GameLoop.ExecuteActions();
            if (GameBoard.GameLoop.GameTick % 20 == 0)
                await SaveGameBoard();
            return true;
        }


        public bool IssuePlayerPickupCommand(IGameObject player, string command)
        {






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

        // public string RenderHtmlView(int x, int y, int z)
        // {
        //     var gameBoardView = new GameBoardView(GameBoard, x - DrawDistance, y - DrawDistance, DrawDistance * 2, DrawDistance * 2, 0);
        //     var str = "";

        //     var allXs = gameBoardView.Positions.Select(p => p.X).Distinct().OrderBy(o => o).ToList();
        //     var allYs = gameBoardView.Positions.Select(p => p.Y).Distinct().OrderBy(o => o).ToList();

        //     foreach (var currentY in allYs)
        //     {
        //         foreach (var currentX in allXs)
        //         {
        //             var pos = gameBoardView.Positions.Where(p => p.X == currentX && p.Y == currentY).First();
        //             var posEntity = pos.GameObjects.OrderByDescending(x => x.Layer).FirstOrDefault();
        //             GameObjectType positionType = (posEntity?.Type ?? GameObjectType.None);
        //             str += $"<div data-go-id=\"{posEntity}\" class=\"{Enum.GetName(positionType)}\" data-pos-x=\"{pos.X}\" data-pos-y=\"{pos.Y}\">{(char)(int)positionType}</div>";
        //         }
        //         str += "\n";
        //     }
        //     return $"<div>{str}</div>";
        // }

        public string RenderStringView(int x, int y, int z)
        {
            var gameBoardView = new GameBoardView(GameBoard, x - DrawDistance, y - DrawDistance, DrawDistance * 2, DrawDistance * 2, 0);
            var str = "";

            var allXs = gameBoardView.Positions.Select(p => p.X).Distinct().OrderBy(o => o).ToList();
            var allYs = gameBoardView.Positions.Select(p => p.Y).Distinct().OrderBy(o => o).ToList();

            foreach (var currentY in allYs)
            {
                foreach (var currentX in allXs)
                {
                    var pos = gameBoardView.Positions.Where(p => p.X == currentX && p.Y == currentY).First();
                    var posEntity = pos.GameObjects.OrderByDescending(x => x.Layer).FirstOrDefault();
                    int positionType = (int)(posEntity?.Type ?? GameObjectType.None);

                    str += (char)positionType;

                }
                str += "\n";
            }


            return str;
        }

        public IList<IGameObject> GetItemsAtPosition(int x, int y, int z)
        {
            var position = GameBoard.GetGameBoardPosition(x, y, z);
            var itemObjects = position.GetGameObjectsWithComponent<ItemComponent>();

            return itemObjects;
        }

    }



}