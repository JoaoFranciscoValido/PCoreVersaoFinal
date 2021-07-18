using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PCore.Models {
    public class Carrinho {

        /// <summary>
        /// Identificador da lista de favoritos
        /// </summary>
        [Key]
        public int IdCarrinho { get; set; }

        //*****************************
        /// <summary>
        /// Fk para o Utilizador
        /// </summary>
        [ForeignKey(nameof(Utilizador))]
        public int UtilizadoresFK { get; set; }
        public Utilizadores Utilizador { get; set; }


        /// <summary>
        /// FK para o Componente
        /// </summary>
        [ForeignKey(nameof(Componente))]
        public int ComponentesFK { get; set; }
        public Componentes Componente { get; set; }



    }
}
