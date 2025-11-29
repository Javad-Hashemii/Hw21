using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.CategoryAgg.Contracts.Repository;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Domain.Core.CategoryAgg.Entities;

namespace Weblog.Domain.Services
{
    public class CategoryService(ICategoryRepository _repository):ICategoryService
    {
        public int Create(CreateCategoryDto dto)
        {
            // 1. Business Rule Check
            // Note: Assuming Repo has the 3-param overload (userId, name, existingId)
            // If your repo uses the 2-param version, pass 'null' or adjust accordingly.
            if (_repository.IsNameExist(dto.UserId, dto.Name))
                throw new Exception("You already have a category with this name.");

            // 2. Map DTO -> Entity
            var category = new Category
            {
                Name = dto.Name,
                OwnerId = dto.UserId
            };

            // 3. Persist
            return _repository.Add(category);
        }

        public void Update(CreateCategoryDto dto)
        {
            // 1. Fetch existing for validation
            // We use GetById from Repo (returns Entity) to check existence/ownership
            var category = _repository.GetById(dto.Id);

            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            if (category.OwnerId != dto.UserId)
                throw new Exception("You do not have permission to edit this category.");

            // 2. Business Rule Check (Name Uniqueness)
            // We pass the current ID to exclude it from the check
            if (_repository.IsNameExist(dto.UserId, dto.Name))
            {
                throw new Exception("You already have a category with this name.");
            }

            // 3. Map Changes
            category.Name = dto.Name;

            // 4. Persist
            _repository.Update(category);
        }

        public void Delete(int id, string userId)
        {
            var category = _repository.GetById(id);
            if (category == null)
                throw new Exception("Category not found.");

            if (category.OwnerId != userId)
                throw new Exception("You do not have permission to delete this category.");

            var deleted = _repository.Delete(id);
            if (!deleted)
                throw new Exception("Could not delete category.");
        }

        public CategoryDto GetById(int id)
        {
            var category = _repository.GetById(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                OwnerId = category.OwnerId
            };
        }

        public List<CategoryDto> GetAll()
        {
            var categories = _repository.GetAll();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                OwnerId = c.OwnerId
            }).ToList();
        }

        public List<CategoryDto> GetByUserId(string userId)
        {
            var categories = _repository.GetByUserId(userId);
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                OwnerId = c.OwnerId
            }).ToList();
        }

        public bool IsCategoryNameUnique(string userId, string categoryName)
        {
            return _repository.IsNameExist(userId, categoryName);
        }
    }
}
