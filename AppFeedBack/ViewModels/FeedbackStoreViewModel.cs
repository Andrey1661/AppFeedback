﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.Validation;

namespace AppFeedBack.ViewModels
{
    /// <summary>
    /// Модель представления, хранящая данные отзыва пользователя
    /// </summary>
    public class FeedbackStoreViewModel
    {
        private readonly ICategoryListCreator _categoryCreator;
        private readonly IFeedbackRepository _repository;


        public Guid? Id { get; set; }

        /// <summary>
        /// Указывает, что редактируется существующий отзыв
        /// </summary>
        public bool EditMode { get; set; }

        /// <summary>
        /// Текст отзыва
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Необходимо заполнить поле")]
        [MaxLength(4000, ErrorMessage = "Максимальная длина сообщения - 4000 символов")]
        public string Text { get; set; }

        /// <summary>
        /// Категория, к которой относится отзыв
        /// </summary>
        [NotEmpty(ErrorMessage = "Категория не выбрана")]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Имя автора
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Прикрепленные пользователем файлы
        /// </summary>
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        /// <summary>
        /// Список категорий для представления
        /// </summary>
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public FeedbackStoreViewModel() { }

        public FeedbackStoreViewModel(IFeedbackRepository repository, ICategoryListCreator categoryCreator)
        {
            _categoryCreator = categoryCreator;
            _repository = repository;
        }

        /// <summary>
        /// Инициализирует модель для создания отзыва. 
        /// По умолчанию загружает в модель список категорий из базы
        /// </summary>
        /// <param name="loadCategories">Если true - загружает в модель спискок существующих категорий</param>
        /// <returns></returns>
        public async Task Initialize(bool loadCategories = true)
        {
            await Initialize(Guid.Empty, loadCategories);
        }

        /// <summary>
        /// Инициализирует модель для редактирования отзыва на основе существующего. 
        /// По умолчанию загружает в модель список категорий из базы. 
        /// </summary>
        /// <param name="id">id существующего отзыва</param>
        /// <param name="loadCategories">Если true - загружает в модель спискок существующих категорий</param>
        /// <returns></returns>
        public async Task Initialize(Guid id, bool loadCategories = true)
        {
            if (_repository == null || _categoryCreator == null) return;

            if (loadCategories)
            {
                Categories = await _categoryCreator.GetCategories("Не выбрана");
            }

            if (id == Guid.Empty) return;

            var feedback = await _repository.Get(id);

            if (feedback != null)
            {
                EditMode = true;
                Id = feedback.Id;
                Text = feedback.Text;
                CategoryId = feedback.CategoryId;
            }
        } 
    }
}