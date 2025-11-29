using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Domain.Core.CategoryAgg.Entities;

namespace Weblog.Domain.Core.CategoryAgg.Contracts.Service
{
    public interface ICategoryService
    {
        public bool IsCategoryNameUnique(string userId, string categoryName);
        public List<CategoryDto> GetByUserId(string userId);
        public List<CategoryDto> GetAll();
        public CategoryDto GetById(int id);
        public void Delete(int id, string userId);
        public void Update(CreateCategoryDto dto);
        public int Create(CreateCategoryDto dto);

    }
}
