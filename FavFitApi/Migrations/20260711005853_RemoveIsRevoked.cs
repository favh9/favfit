using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FavFitApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsRevoked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_revoked",
                table: "refresh_tokens");

            migrationBuilder.AlterColumn<DateTime>(
                name: "issued_at",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "timezone('utc', now())");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "issued_at",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "is_revoked",
                table: "refresh_tokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
