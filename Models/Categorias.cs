using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCore.Models
{
    /// <summary>
    /// Diferentes tipos de categorias de componentes 
    /// </summary>
    public class Categorias
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Categorias()
        {
            ListaDeComponentes = new HashSet<Componentes>();
        }
        /// <summary>
        /// Identificador de categorias
        /// </summary>
        [Key]
        public int IdCategorias { get; set; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Lista de categorias
        /// </summary>
        public ICollection<Componentes> ListaDeComponentes { get; set; }
    }
}
