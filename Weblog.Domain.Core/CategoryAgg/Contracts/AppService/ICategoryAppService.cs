using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.CategoryAgg.Dtos;

namespace Weblog.Domain.Core.CategoryAgg.Contracts.AppService
{
    public interface ICategoryAppService
    {
        List<CategoryDto> GetAll();
        List<CategoryDto> GetByUser(string userId);
        CategoryDto GetById(int id);
        int Create(CreateCategoryDto dto);
        void Update(CreateCategoryDto dto);
        void Delete(int postId, string userId);
    }
}
