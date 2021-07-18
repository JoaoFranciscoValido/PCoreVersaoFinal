using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PCore.Models
{
    /// <summary>
    /// Descrição de um componente
    /// </summary>
    public class Componentes
    {

        /// <summary>
        /// Construtores
        /// </summary>
        public Componentes()
        {
            ListaDeReviews = new HashSet<Reviews>();
            ListaDeCategorias = new HashSet<Categorias>();
            ListaDeCarrinho = new HashSet<Carrinho>();
        }

        /// <summary>
        /// Identificador do componente
        /// </summary>
        [Key]
        public int IdComponentes { get; set; }

        /// <summary>
        /// Título do componente
        /// </summary>
        [Required(ErrorMessage = "O Nome é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
        public string Nome { get; set; }

        /// <summary>
        /// Foto do Componente
        /// </summary>
        public string Foto { get; set; }

        /// <summary>
        /// Breve descrição sobre o componente
        /// </summary>
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        /// <summary>
        /// Preço do Componente
        /// </summary>
        [Required]
        [Display(Name = "Preço(€)")]
        public int Preco { get; set; }

        /// <summary>
        /// Stock do Componente
        /// </summary>
        [Required]
        public int Stock { get; set; }

        /// <summary>
        /// Pontuação do componente
        /// </summary>
        [Required]
        [Display(Name = "Pontuação")]
        public double Pontuacao { get; set; }

        /// <summary>
        /// Lista das reviews dos componentes
        /// </summary>
        public ICollection<Reviews> ListaDeReviews { get; set; }

        /// <summary>
        /// Lista de categorias dos componentes
        /// </summary>
        public ICollection<Categorias> ListaDeCategorias { get; set; }

        /// <summary>
        /// carrinho de componentes
        /// </summary>
        public ICollection<Carrinho> ListaDeCarrinho { get; set; }
    }
}
