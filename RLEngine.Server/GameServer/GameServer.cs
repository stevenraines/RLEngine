using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RLEngine.Core;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Generators;
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
        private IGameBoard GameBoard { get; set; }
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
                                 .Include(x => x.GameLoop)
                                 .FirstOrDefaultAsync();

            if (GameBoard == null)
            {
                GameLoop = new GameLoop(GameLoopType.Timed); ;
                GameBoard = new GameBoard(GameLoop);
                var roomSize = 20;
                // Create a player
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

            player = new GameObject(GameBoard, GameObjectType.Player);
            GameBoard.AddGameObject(player, 0, 0, 0);
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

        public void IssuePlayerCommand(IGameObject player, string command)
        {

            (int x, int y, int z) moveDirection = (0, 0, 0);

            if (command == "ArrowRight") moveDirection = (1, 0, 0);
            if (command == "ArrowLeft") moveDirection = (-1, 0, 0);
            if (command == "ArrowDown") moveDirection = (0, 1, 0);
            if (command == "ArrowUp") moveDirection = (0, -1, 0);

            if (moveDirection != (0, 0, 0))
            {
                var scheduledAction = new ScheduledAction(player.Id, new MoveAction(player,
                                                                                moveDirection.x,
                                                                                moveDirection.y,
                                                                                moveDirection.z));

                GameBoard.GameLoop.ScheduleAction(scheduledAction);
            }

        }



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


    }
}