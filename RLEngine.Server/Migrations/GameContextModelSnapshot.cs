﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RLEngine.Server.Infrastructure;

#nullable disable

namespace RLEngine.Server.Migrations
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("RLEngine.Core.GameBoard", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("Seed")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GameBoards");
                });

            modelBuilder.Entity("RLEngine.Core.GameLoop", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GameBoardId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("GameLoopRunning")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GameTick")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameBoardId")
                        .IsUnique();

                    b.ToTable("GameLoop");
                });

            modelBuilder.Entity("RLEngine.Core.GameMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GameObjectId")
                        .HasColumnType("TEXT");

                    b.Property<long>("GameTick")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameObjectId");

                    b.ToTable("GameMessage");
                });

            modelBuilder.Entity("RLEngine.Core.GameObject", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContainerGameObjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GameBoardId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Layer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("SerializedComponents")
                        .HasColumnType("TEXT")
                        .HasColumnName("Components");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Z")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameBoardId");

                    b.ToTable("GameObject");
                });

            modelBuilder.Entity("RLEngine.Core.GameLoop", b =>
                {
                    b.HasOne("RLEngine.Core.GameBoard", "GameBoard")
                        .WithOne("GameLoop")
                        .HasForeignKey("RLEngine.Core.GameLoop", "GameBoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameBoard");
                });

            modelBuilder.Entity("RLEngine.Core.GameMessage", b =>
                {
                    b.HasOne("RLEngine.Core.GameObject", "GameObject")
                        .WithMany("Messages")
                        .HasForeignKey("GameObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameObject");
                });

            modelBuilder.Entity("RLEngine.Core.GameObject", b =>
                {
                    b.HasOne("RLEngine.Core.GameBoard", "GameBoard")
                        .WithMany("GameObjects")
                        .HasForeignKey("GameBoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameBoard");
                });

            modelBuilder.Entity("RLEngine.Core.GameBoard", b =>
                {
                    b.Navigation("GameLoop");

                    b.Navigation("GameObjects");
                });

            modelBuilder.Entity("RLEngine.Core.GameObject", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
