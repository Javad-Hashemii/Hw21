using Weblog.Domain.Core.CategoryAgg.Contracts.Repository;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Domain.Core.CategoryAgg.Entities;

namespace Weblog.Domain.Services
{
    public class CategoryService(ICategoryRepository _repository) : ICategoryService
    {
        public int CreateCategory(CreateCategoryDto dto)
        {
            EnsureNameIsUnique(dto.UserId, dto.Name);

            var category = new Category
            {
                Name = dto.Name,
                OwnerId = dto.UserId
            };

            return _repository.Add(category);
        }

        public bool Update(CreateCategoryDto dto)
        {
            var category = GetAndValidate(dto.Id, dto.UserId);

            EnsureNameIsUnique(dto.UserId, dto.Name);

            category.Name = dto.Name;

            var result = _repository.Update(category);
            return result > 0;
        }


        public bool Delete(int id, string userId)
        {
            var category = GetAndValidate(id, userId);

            var result = _repository.Delete(id);
            if (!result)
            {
                throw new Exception("خطا در حذف دسته بندی");
            }

            return result;
        }

        public CategoryDto GetCategoryById(int id)
        {
            var category = _repository.GetById(id);
            return category == null ? null : MapToDto(category);
        }

        public List<CategoryDto> GetAllCategories()
        {
            return _repository.GetAllCategories()
                              .Select(MapToDto)
                              .ToList();
        }

        public List<CategoryDto> GetCategoryByUserId(string userId)
        {
            return _repository.GetByUserId(userId)
                              .Select(MapToDto)
                              .ToList();
        }

        public bool IsCategoryNameUnique(string userId, string categoryName)
        {
            return _repository.IsNameExist(userId, categoryName);
        }

        private Category GetAndValidate(int id, string userId)
        {
            var category = _repository.GetById(id);

            if (category == null)
            {
                throw new Exception("دسته بندی پیدا نشد");
            }

            if (category.OwnerId != userId)
            {
                throw new Exception("خطای دسترسی");
            }

            return category;
        }

        private void EnsureNameIsUnique(string userId, string name)
        {
            if (_repository.IsNameExist(userId, name))
            {
                throw new Exception("دسته بندی با این اسم از قبل وجود دارد");
            }
        }

        private CategoryDto MapToDto(Category c)
        {
            return new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                OwnerId = c.OwnerId
            };
        }
    }
}
