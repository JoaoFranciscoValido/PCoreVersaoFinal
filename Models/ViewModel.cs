using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCore.Models
{
    public class ComponentesAPIViewModel
    {
        /// <summary>
        /// Identificador do componente
        /// </summary>
        public int IdComponentes { get; set; }

        /// <summary>
        /// Título do componente
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Foto do Componente
        /// </summary>
        public string Foto { get; set; }

        /// <summary>
        /// Breve descrição sobre o componente
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Preço do Componente
        /// </summary>
        public int Preco { get; set; }

        /// <summary>
        /// Stock do Componente
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Pontuação do componente
        /// </summary>
        public double Pontuacao { get; set; }

    }
}
