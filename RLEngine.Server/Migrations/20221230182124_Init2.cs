using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RLEngine.Server.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameBoards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Seed = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameLoop",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameBoardId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameLoopRunning = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameTick = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLoop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameLoop_GameBoards_GameBoardId",
                        column: x => x.GameBoardId,
                        principalTable: "GameBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameObject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Z = table.Column<int>(type: "INTEGER", nullable: false),
                    Layer = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    GameBoardId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Components = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameObject_GameBoards_GameBoardId",
                        column: x => x.GameBoardId,
                        principalTable: "GameBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameObjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    GameTick = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameMessage_GameObject_GameObjectId",
                        column: x => x.GameObjectId,
                        principalTable: "GameObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameLoop_GameBoardId",
                table: "GameLoop",
                column: "GameBoardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameMessage_GameObjectId",
                table: "GameMessage",
                column: "GameObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GameObject_GameBoardId",
                table: "GameObject",
                column: "GameBoardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameLoop");

            migrationBuilder.DropTable(
                name: "GameMessage");

            migrationBuilder.DropTable(
                name: "GameObject");

            migrationBuilder.DropTable(
                name: "GameBoards");
        }
    }
}
