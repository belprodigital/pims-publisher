using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PimsPublisher.Infrastructure.IntegrationMessages.Migrations
{
    /// <inheritdoc />
    public partial class initdbwithmessagelog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegrationMessageLog",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageTypeName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MessageTypeShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    TimesSent = table.Column<int>(type: "int", nullable: false),
                    OccurredTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayLoad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationMessageLog", x => x.MessageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationMessageLog");
        }
    }
}
