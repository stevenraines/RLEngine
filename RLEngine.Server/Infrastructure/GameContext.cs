using Microsoft.EntityFrameworkCore;
using RLEngine.Core;
using RLEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLEngine.Server.Infrastructure
{
    public class GameContext : DbContext
    {
        public DbSet<GameBoard> GameBoards { get; set; }

        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
            Database.OpenConnection();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<GameBoard>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<GameLoop>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<GameObject>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<GameMessage>().Property(x => x.Id).ValueGeneratedNever();

            modelBuilder.Entity<GameBoard>()
                .HasMany(h => (IList<GameObject>)h.GameObjects)
                .WithOne(h => (GameBoard)h.GameBoard)
                    .HasForeignKey(p => p.GameBoardId);

            modelBuilder.Entity<GameBoard>()
                .HasOne(h => (GameLoop)h.GameLoop)
                .WithOne(h => (GameBoard)h.GameBoard);

            modelBuilder.Entity<GameObject>()
                .HasMany(h => (IList<GameMessage>)h.Messages)
                .WithOne(h => (GameObject)h.GameObject)
                    .HasForeignKey(p => p.GameObjectId);

            modelBuilder.Entity<GameObject>().Ignore(p => p.Components);

            modelBuilder.Entity<GameObject>()
                         .Property(p => p.SerializedComponents)
                         .HasColumnName("Components");


            base.OnModelCreating(modelBuilder);
        }

        public override void Dispose()
        {
            Database.CloseConnection();
            base.Dispose();
        }
    }
}
