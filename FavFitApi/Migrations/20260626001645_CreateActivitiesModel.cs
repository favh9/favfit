using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FavFitApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateActivitiesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    elapsed_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    distance = table.Column<double>(type: "double precision", nullable: false),
                    average_speed = table.Column<double>(type: "double precision", nullable: false),
                    average_cadence = table.Column<int>(type: "integer", nullable: false),
                    average_pace = table.Column<TimeSpan>(type: "interval", nullable: false),
                    average_heart_rate = table.Column<int>(type: "integer", nullable: false),
                    elevation_gain = table.Column<double>(type: "double precision", nullable: false),
                    calories = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activities", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activities");
        }
    }
}
