using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppFeedBack.Validation
{
    /// <summary>
    /// Атрибут, не допускающий объект типа Guid равным значению Guid.Empty
    /// </summary>
    public class NotEmptyAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly Guid _empty = Guid.Empty;

        public override bool IsValid(object value)
        {
            var guid = (Guid) value;
            if (guid != _empty)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            ModelClientValidationRule notNullRule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "notempty"
            };

            notNullRule.ValidationParameters.Add("empty", _empty);

            yield return notNullRule;
        }
    }
}