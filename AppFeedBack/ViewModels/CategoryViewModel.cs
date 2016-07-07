using System;

namespace AppFeedBack.ViewModels
{
    /// <summary>
    /// Модель представления для категорий
    /// </summary>
    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public CategoryViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}