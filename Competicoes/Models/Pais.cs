﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Competicoes.Models
{
    [Table("Paises")]
    public class Pais
    {
        public Guid PaisId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "País")]
        [StringLength(200, ErrorMessage = "O campo {0} deve ser uma string com um comprimento máximo de {1}")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(3, ErrorMessage = "O campo {0} deve ser uma string com um comprimento máximo de {1}")]
        public string Sigla { get; set; }

        [InverseProperty(nameof(Estado.Pais))]
        public virtual ICollection<Estado> Estados { get; set; }
    }
}
