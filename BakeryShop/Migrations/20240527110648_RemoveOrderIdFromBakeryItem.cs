using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeryShop.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrderIdFromBakeryItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {   
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "BakeryItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "BakeryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
