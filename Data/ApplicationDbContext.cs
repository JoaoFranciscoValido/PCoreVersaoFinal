using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PCore.Data
{


    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "c", Name = "Cliente", NormalizedName = "CLIENTE" },
            new IdentityRole { Id = "g", Name = "Gestor", NormalizedName = "GESTOR" }
         );


            // insert DB seed
            modelBuilder.Entity<Componentes>().HasData(
               new Componentes { IdComponentes = 1, Nome = "Intel Core I5", Foto = "cpu.jpg", Descricao = "Processador Intel Core i5-10400F 6-Core 2.9GHz c/ Turbo 4.3GHz 12MB Skt1200", Stock = 20, Preco = 350 },
               new Componentes { IdComponentes = 2, Nome = "Intel Core I7",  Foto = "cpu1.jpg", Descricao = "Processador Intel Core i7-10700K 8-Core 3.8GHz c/ Turbo 5.1GHz 16MB Skt1200", Stock = 5, Preco = 400 },
               new Componentes { IdComponentes = 3, Nome = "Ventoinha 240",  Foto = "fan.jpg", Descricao = "Ventoinha 240mm  1200RPM ML120 PRO LED Branco 4 Pinos PWM", Stock = 50, Preco = 120 },
               new Componentes { IdComponentes = 4, Nome = "Ventoinha 120",  Foto = "fan2.jpg", Descricao = "Ventoinha 120mm  2400RPM ML120 PRO LED Branco 4 Pinos PWM", Stock = 25, Preco = 100 },
               new Componentes { IdComponentes = 5, Nome = "NVIDIA GTX1650",  Foto = "gpu.jpg", Descricao = "Placa Gráfica Gaming GeForce GTX 1650 OC 4GB", Stock = 2, Preco = 700 },
               new Componentes { IdComponentes = 6, Nome = "NVIDIA Quadro",  Foto = "gpu1.jpg" , Descricao = "Placa Gráfica PNY NVIDIA Quadro RTX 5000", Stock = 5, Preco = 400},
               new Componentes { IdComponentes = 7, Nome = "1TB HDD",  Foto = "hdd.jpg" , Descricao = "HDD Western Digital 1TB Caviar Blue 7200rpm 64MB SATA III 3.5", Stock = 1, Preco = 45},
               new Componentes { IdComponentes = 8, Nome = "512GB HDD",  Foto = "hdd1.jpg", Descricao = "Disco Interno SSD A400 - 512GB", Stock = 60, Preco = 20 },
               new Componentes { IdComponentes = 9, Nome = "2TB SSD",  Foto = "ssd.jpg" , Descricao = "Disco Interno SSD 860 EVO - 2TB", Stock = 15, Preco = 120},
               new Componentes { IdComponentes = 10, Nome = "512GB SSD",  Foto = "ssd1.jpg", Descricao = "Disco Interno SSD  A400 - 512GB", Stock = 70, Preco = 450 },
               new Componentes { IdComponentes = 11, Nome = "MotherBoard", Foto = "mother.jpg", Descricao = "MotherBoard Gaming", Stock = 0, Preco = 80 },
               new Componentes { IdComponentes = 12, Nome = "Caixa Gaming", Foto = "box.jpg", Descricao = "Caixa Gamer Storm", Preco = 80, Stock = 0 }
           );


            modelBuilder.Entity<Categorias>().HasData(
               new Categorias { IdCategorias = 1, Nome = "Processadores" },
               new Categorias { IdCategorias = 2, Nome = "Placas Gráficas" },
               new Categorias { IdCategorias = 3, Nome = "Ventoinhas" },
               new Categorias { IdCategorias = 4, Nome = "Armazenamento HDD" },
               new Categorias { IdCategorias = 5, Nome = "Armazenamento SSD" },
               new Categorias { IdCategorias = 6, Nome = "Motherboards" },
               new Categorias { IdCategorias = 7, Nome = "Caixas" },
               new Categorias { IdCategorias = 8, Nome = "Fontes de Alimentação" },
               new Categorias { IdCategorias = 9, Nome = "Cooling" },
               new Categorias { IdCategorias = 10, Nome = "Periféricos" }
           );




        }

        /// <summary>
        /// Representar a Tabela Componente da BD
        /// </summary>
        public DbSet<Componentes> Componentes { get; set; }
        public DbSet<Categorias> ListaDeCategorias { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Carrinho> Carrinho { get; set; }
    }
}