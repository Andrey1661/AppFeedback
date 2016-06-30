using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppFeedBack.ViewModels
{
    /// <summary>
    /// Модель представления для категорий
    /// </summary>
    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}