using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Competicoes.Models
{
    [Table("Estados")]
    public class Estado : IEntity
    {
        [Key]
        public Guid EstadoId { get; set; }

        [Required]
        public Guid PaisId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Estado")]
        public string Nome { get; set; }

        [Required]
        [StringLength(2)]
        public string Sigla { get; set; }

        public virtual Pais Pais { get; set; }  
        
        [InverseProperty(nameof(Cidade.Estado))]
        public ICollection<Cidade> Cidades { get; set; }
    }
}
