using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PimsPublisher.Infrastructure.PublisherDb.Migrations
{
    /// <inheritdoc />
    public partial class initialpublisherdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Synchronizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ModelCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TotalItems = table.Column<int>(type: "int", nullable: false),
                    BatchSize = table.Column<int>(type: "int", nullable: false),
                    TotalBatch = table.Column<int>(type: "int", nullable: false),
                    TotalSubmitted = table.Column<int>(type: "int", nullable: false),
                    Progress = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synchronizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SynchronizationBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyncId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BatchNo = table.Column<int>(type: "int", nullable: false),
                    Offset = table.Column<int>(type: "int", nullable: false),
                    BatchTotal = table.Column<int>(type: "int", nullable: false),
                    SyncTotal = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SynchronizationBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SynchronizationBatches_Synchronizations_SyncId",
                        column: x => x.SyncId,
                        principalTable: "Synchronizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SynchronizationItems",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyncId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchNo = table.Column<int>(type: "int", nullable: false),
                    Keys = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SynchronizationItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_SynchronizationItems_SynchronizationBatches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "SynchronizationBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SynchronizationBatches_SyncId",
                table: "SynchronizationBatches",
                column: "SyncId");

            migrationBuilder.CreateIndex(
                name: "IX_SynchronizationItems_BatchId",
                table: "SynchronizationItems",
                column: "BatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SynchronizationItems");

            migrationBuilder.DropTable(
                name: "SynchronizationBatches");

            migrationBuilder.DropTable(
                name: "Synchronizations");
        }
    }
}
