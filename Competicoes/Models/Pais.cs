﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Competicoes.Models
{
    [Table("Paises")]
    public class Pais : IEntity
    {
        public Guid PaisId { get; set; }

        [Required]
        [Display(Name = "País")]
        [StringLength(200)]
        public string Nome { get; set; }

        [Required]
        [StringLength(3)]
        public string Sigla { get; set; }

        [InverseProperty(nameof(Estado.Pais))]
        public virtual ICollection<Estado> Estados { get; set; }
    }
}
