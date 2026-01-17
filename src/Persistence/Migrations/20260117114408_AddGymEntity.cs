using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGymEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailabilityItem",
                table: "AvailabilityItem");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AvailabilityItem",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()");

            migrationBuilder.AddColumn<Guid>(
                name: "GymId",
                table: "AvailabilityItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailabilityItem",
                table: "AvailabilityItem",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Gym",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ApiPrefix = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gym", x => x.Id);
                });

            var defaultGymId = Guid.CreateVersion7();
            migrationBuilder.Sql($@"
                INSERT INTO ""Gym"" (""Id"", ""Name"", ""Username"", ""Password"", ""ApiPrefix"")
                VALUES ('{defaultGymId}', 'HutFit', 'user', 'pwd', 'Wic_Hutfit');
                
                UPDATE ""AvailabilityItem""
                SET ""GymId"" = '{defaultGymId}';
            ");
            
            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityItem_GymId",
                table: "AvailabilityItem",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilityItem_Gym_GymId",
                table: "AvailabilityItem",
                column: "GymId",
                principalTable: "Gym",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilityItem_Gym_GymId",
                table: "AvailabilityItem");

            migrationBuilder.DropTable(
                name: "Gym");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailabilityItem",
                table: "AvailabilityItem");

            migrationBuilder.DropIndex(
                name: "IX_AvailabilityItem_GymId",
                table: "AvailabilityItem");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AvailabilityItem");

            migrationBuilder.DropColumn(
                name: "GymId",
                table: "AvailabilityItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailabilityItem",
                table: "AvailabilityItem",
                column: "Time");
        }
    }
}
