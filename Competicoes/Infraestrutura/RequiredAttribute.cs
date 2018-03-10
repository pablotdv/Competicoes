using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Competicoes
{
    public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public RequiredAttribute()
        {
            ErrorMessage = "O campo {0} é obrigatório";
        }
    }
}
