using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CelebraTix.Promotions.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    VenueId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VenueGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.VenueId);
                    table.UniqueConstraint("AK_Venues_VenueGuid", x => x.VenueGuid);
                });

            migrationBuilder.CreateTable(
                name: "VenueDescription",
                columns: table => new
                {
                    VenueDescriptionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VenueId = table.Column<int>(type: "integer", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueDescription", x => x.VenueDescriptionId);
                    table.UniqueConstraint("AK_VenueDescription_VenueId_ModifiedDate", x => new { x.VenueId, x.ModifiedDate });
                    table.ForeignKey(
                        name: "FK_VenueDescription_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueLocation",
                columns: table => new
                {
                    VenueLocationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VenueId = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueLocation", x => x.VenueLocationId);
                    table.UniqueConstraint("AK_VenueLocation_VenueId_ModifiedDate", x => new { x.VenueId, x.ModifiedDate });
                    table.ForeignKey(
                        name: "FK_VenueLocation_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueRemoved",
                columns: table => new
                {
                    VenueRemovedId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VenueId = table.Column<int>(type: "integer", nullable: false),
                    RemovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueRemoved", x => x.VenueRemovedId);
                    table.ForeignKey(
                        name: "FK_VenueRemoved_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueTimeZone",
                columns: table => new
                {
                    VenueTimeZoneId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VenueId = table.Column<int>(type: "integer", nullable: false),
                    TimeZone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueTimeZone", x => x.VenueTimeZoneId);
                    table.UniqueConstraint("AK_VenueTimeZone_VenueId_ModifiedDate", x => new { x.VenueId, x.ModifiedDate });
                    table.ForeignKey(
                        name: "FK_VenueTimeZone_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VenueRemoved_VenueId",
                table: "VenueRemoved",
                column: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VenueDescription");

            migrationBuilder.DropTable(
                name: "VenueLocation");

            migrationBuilder.DropTable(
                name: "VenueRemoved");

            migrationBuilder.DropTable(
                name: "VenueTimeZone");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
