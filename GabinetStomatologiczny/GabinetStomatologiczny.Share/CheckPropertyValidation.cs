using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Share
{
    public class CheckPropertyValidation
    {
        public IList<ValidationResult> myValidation(object model)
        {
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, result);

            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);

            return result;



        }
    }
}
