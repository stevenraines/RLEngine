using Microsoft.EntityFrameworkCore;
using RLEngine.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLEngine.Server.Infrastructure
{
    public class GameContext : DbContext
    {
        public DbSet<GameBoard> GameBoards { get; set; }
        //  public DbSet<GameObject> GameObjects { get; set; }

        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<GameBoard>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<GameLoop>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<GameObject>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<GameComponent>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<GameMessage>().Property(x => x.Id).ValueGeneratedNever();

            modelBuilder.Entity<GameBoard>()
                .HasMany(h => (IList<GameObject>)h.GameObjects)
                .WithOne(h => (GameBoard)h.GameBoard)
                    .HasForeignKey(p => p.GameBoardId);

            modelBuilder.Entity<GameBoard>()
                .HasOne(h => (GameLoop)h.GameLoop)
                .WithOne(h => (GameBoard)h.GameBoard);

            modelBuilder.Entity<GameObject>()
                .HasMany(h => (IList<GameComponent>)h.Components)
                .WithOne(h => (GameObject)h.GameObject)
                    .HasForeignKey(p => p.GameObjectId);

            modelBuilder.Entity<GameObject>()
                .HasMany(h => (IList<GameMessage>)h.Messages)
                .WithOne(h => (GameObject)h.GameObject)
                    .HasForeignKey(p => p.GameObjectId);

            modelBuilder.Entity<GameComponent>()
                         .Property(p => p.Serialized)
                         .HasColumnName("Meta");
            modelBuilder.Entity<GameComponent>().Ignore(p => p.Data);

            base.OnModelCreating(modelBuilder);
        }
    }
}
