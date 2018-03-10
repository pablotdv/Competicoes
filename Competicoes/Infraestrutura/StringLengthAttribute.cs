using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Competicoes
{
    public class StringLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute
    {
        public StringLengthAttribute(int maximumLength)
            :base(maximumLength)
        {
            ErrorMessage = "O campo {0} deve ser uma string com um comprimento máximo de {1}";
        }
    }
}
