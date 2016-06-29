using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppFeedBack.Validation
{
    /// <summary>
    /// Атрибут, не допускающий объект типа Guid равным значению Guid.Empty
    /// </summary>
    public class NotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var guid = (Guid) value;
            if (guid != Guid.Empty)
            {
                return true;
            }

            return false;
        }
    }
}