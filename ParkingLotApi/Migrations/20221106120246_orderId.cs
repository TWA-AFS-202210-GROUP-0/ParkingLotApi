using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingLotApi.Migrations
{
    public partial class orderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingLots_Order_OrderId",
                table: "ParkingLots");

            migrationBuilder.DropIndex(
                name: "IX_ParkingLots_OrderId",
                table: "ParkingLots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLots_OrderId",
                table: "ParkingLots",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingLots_Order_OrderId",
                table: "ParkingLots",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
