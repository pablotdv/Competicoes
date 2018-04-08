using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Competicoes.Models
{
    [Table("Cidades")]
    public class Cidade : IEntity
    {
        [Key]
        public Guid CidadeId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public Guid EstadoId { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Cidade")]
        public string Nome { get; set; }

        [Required]
        public int Cep { get; set; }

        [ForeignKey(nameof(EstadoId))]
        public virtual Estado Estado { get; set; }
    }
}
