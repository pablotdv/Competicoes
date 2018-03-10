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

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid PaisId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} deve ser uma string com um comprimento máximo de {1}")]
        [Display(Name = "Estado")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(2, ErrorMessage = "O campo {0} deve ser uma string com um comprimento máximo de {1}")]
        public string Sigla { get; set; }

        public virtual Pais Pais { get; set; }        
    }
}
