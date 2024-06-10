using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryMarket.Migrations
{
    /// <inheritdoc />
    public partial class itemsQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "JewelryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "JewelryItems");
        }
    }
}
