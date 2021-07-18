using Microsoft.EntityFrameworkCore.Migrations;

namespace PCore.Data.Migrations
{
    public partial class categorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c",
                column: "ConcurrencyStamp",
                value: "0fa230b0-0531-4751-a2f9-6fd7717a99d3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "g",
                column: "ConcurrencyStamp",
                value: "ac76a700-3541-4ab8-ad3e-3a46c0b3419a");

            migrationBuilder.InsertData(
                table: "ListaDeCategorias",
                columns: new[] { "IdCategorias", "Nome" },
                values: new object[,]
                {
                    { 1, "Processadores" },
                    { 2, "Placas Gráficas" },
                    { 3, "Ventoinhas" },
                    { 4, "Armazenamento HDD" },
                    { 5, "Armazenamento SSD" },
                    { 6, "Motherboards" },
                    { 7, "Caixas" },
                    { 8, "Fontes de Alimentação" },
                    { 9, "Cooling" },
                    { 10, "Periféricos" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ListaDeCategorias",
                keyColumn: "IdCategorias",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c",
                column: "ConcurrencyStamp",
                value: "53a7efe3-3ede-44f8-9f34-d307be3023f1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "g",
                column: "ConcurrencyStamp",
                value: "780e0728-fc92-4647-970d-165f30e178b1");
        }
    }
}
