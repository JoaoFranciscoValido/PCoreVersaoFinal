using Microsoft.EntityFrameworkCore.Migrations;

namespace PCore.Data.Migrations
{
    public partial class componentes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Componentes",
                columns: new[] { "IdComponentes", "Descricao", "Foto", "Nome", "Pontuacao", "Preco", "Stock" },
                values: new object[,]
                {
                    { 1, "Processador Intel Core i5-10400F 6-Core 2.9GHz c/ Turbo 4.3GHz 12MB Skt1200", "cpu.jpg", "Intel Core I5", 0.0, 350, 20 },
                    { 2, "Processador Intel Core i7-10700K 8-Core 3.8GHz c/ Turbo 5.1GHz 16MB Skt1200", "cpu1.jpg", "Intel Core I7", 0.0, 400, 5 },
                    { 3, "Ventoinha 240mm  1200RPM ML120 PRO LED Branco 4 Pinos PWM", "fan.jpg", "Ventoinha 240", 0.0, 120, 50 },
                    { 4, "Ventoinha 120mm  2400RPM ML120 PRO LED Branco 4 Pinos PWM", "fan2.jpg", "Ventoinha 120", 0.0, 100, 25 },
                    { 5, "Placa Gráfica Gaming GeForce GTX 1650 OC 4GB", "gpu.jpg", "NVIDIA GTX1650", 0.0, 700, 2 },
                    { 6, "Placa Gráfica PNY NVIDIA Quadro RTX 5000", "gpu1.jpg", "NVIDIA Quadro", 0.0, 400, 5 },
                    { 7, "HDD Western Digital 1TB Caviar Blue 7200rpm 64MB SATA III 3.5", "hdd.jpg", "1TB HDD", 0.0, 45, 1 },
                    { 8, "Disco Interno SSD A400 - 512GB", "hdd1.jpg", "512GB HDD", 0.0, 20, 60 },
                    { 9, "Disco Interno SSD 860 EVO - 2TB", "ssd.jpg", "2TB SSD", 0.0, 120, 15 },
                    { 10, "Disco Interno SSD  A400 - 512GB", "ssd1.jpg", "512GB SSD", 0.0, 450, 70 },
                    { 11, "MotherBoard Gaming", "mother.jpg", "MotherBoard", 0.0, 80, 0 },
                    { 12, "Caixa Gamer Storm", "box.jpg", "Caixa Gaming", 0.0, 80, 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Componentes",
                keyColumn: "IdComponentes",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c",
                column: "ConcurrencyStamp",
                value: "c19ad920-d7ca-4324-b7bf-7d5df2ae0f84");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "g",
                column: "ConcurrencyStamp",
                value: "b0ff6d95-1570-431a-ae8c-e8f9a9d387d7");
        }
    }
}
