using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PCore.Data.Migrations
{
    public partial class Inicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Componentes",
                columns: table => new
                {
                    IdComponentes = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preco = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Pontuacao = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Componentes", x => x.IdComponentes);
                });

            migrationBuilder.CreateTable(
                name: "ListaDeCategorias",
                columns: table => new
                {
                    IdCategorias = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaDeCategorias", x => x.IdCategorias);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    IdUtilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserNameId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ControlarReview = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.IdUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasComponentes",
                columns: table => new
                {
                    ListaDeCategoriasIdCategorias = table.Column<int>(type: "int", nullable: false),
                    ListaDeComponentesIdComponentes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasComponentes", x => new { x.ListaDeCategoriasIdCategorias, x.ListaDeComponentesIdComponentes });
                    table.ForeignKey(
                        name: "FK_CategoriasComponentes_Componentes_ListaDeComponentesIdComponentes",
                        column: x => x.ListaDeComponentesIdComponentes,
                        principalTable: "Componentes",
                        principalColumn: "IdComponentes",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriasComponentes_ListaDeCategorias_ListaDeCategoriasIdCategorias",
                        column: x => x.ListaDeCategoriasIdCategorias,
                        principalTable: "ListaDeCategorias",
                        principalColumn: "IdCategorias",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carrinho",
                columns: table => new
                {
                    IdCarrinho = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtilizadoresFK = table.Column<int>(type: "int", nullable: false),
                    ComponentesFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrinho", x => x.IdCarrinho);
                    table.ForeignKey(
                        name: "FK_Carrinho_Componentes_ComponentesFK",
                        column: x => x.ComponentesFK,
                        principalTable: "Componentes",
                        principalColumn: "IdComponentes",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carrinho_Utilizadores_UtilizadoresFK",
                        column: x => x.UtilizadoresFK,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    IdReview = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pontuacao = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Visibilidade = table.Column<bool>(type: "bit", nullable: false),
                    UtilizadoresFK = table.Column<int>(type: "int", nullable: false),
                    ComponentesFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.IdReview);
                    table.ForeignKey(
                        name: "FK_Reviews_Componentes_ComponentesFK",
                        column: x => x.ComponentesFK,
                        principalTable: "Componentes",
                        principalColumn: "IdComponentes",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Utilizadores_UtilizadoresFK",
                        column: x => x.UtilizadoresFK,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c", "c19ad920-d7ca-4324-b7bf-7d5df2ae0f84", "Cliente", "CLIENTE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "g", "b0ff6d95-1570-431a-ae8c-e8f9a9d387d7", "Gestor", "GESTOR" });

            migrationBuilder.CreateIndex(
                name: "IX_Carrinho_ComponentesFK",
                table: "Carrinho",
                column: "ComponentesFK");

            migrationBuilder.CreateIndex(
                name: "IX_Carrinho_UtilizadoresFK",
                table: "Carrinho",
                column: "UtilizadoresFK");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasComponentes_ListaDeComponentesIdComponentes",
                table: "CategoriasComponentes",
                column: "ListaDeComponentesIdComponentes");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ComponentesFK",
                table: "Reviews",
                column: "ComponentesFK");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UtilizadoresFK",
                table: "Reviews",
                column: "UtilizadoresFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carrinho");

            migrationBuilder.DropTable(
                name: "CategoriasComponentes");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ListaDeCategorias");

            migrationBuilder.DropTable(
                name: "Componentes");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "g");
        }
    }
}
