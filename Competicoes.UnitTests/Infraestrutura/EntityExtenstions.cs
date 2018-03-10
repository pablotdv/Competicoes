using Competicoes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Competicoes.UnitTests.Infraestrutura
{
    internal static class EntityExtenstions
    {
        internal static IList<ValidationResult> ValidateModel(this IEntity model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
