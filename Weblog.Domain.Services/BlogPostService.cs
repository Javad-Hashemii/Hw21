using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.FileAgg.Contracts;
using Weblog.Domain.Core.PostAgg.Contracts.Repository;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Services
{
    public class BlogPostService(IBlogRepository _repository, ICategoryService _categoryService, IFileService _fileService) : IBlogPostService
    {

        public int Create(CreatePostDto dto)
        {
            ValidateTitle(dto.Title);
            ValidateUser(dto.AuthorId);
            EnsureUserOwnsCategory(dto.AuthorId, dto.CategoryId);

            string imagePath = _fileService.Upload(dto.ImageFile, "BlogImages");

            var post = new BlogPost
            {
                Title = dto.Title,
                Text = dto.Text,
                CategoryId = dto.CategoryId,
                AuthorId = dto.AuthorId,
                PublishedDate = DateTime.UtcNow,
                ImageUrl = imagePath
            };

            return _repository.Add(post);
        }

        public int Update(UpdatePostDto dto)
        {
            ValidateTitle(dto.Title);
            ValidateUser(dto.AuthorId);

            var existing = GetPostOrThrow(dto.Id);
            EnsurePostOwner(existing, dto.AuthorId);
            EnsureUserOwnsCategory(dto.AuthorId, dto.CategoryId);

            existing.Title = dto.Title;
            existing.Text = dto.Text;
            existing.CategoryId = dto.CategoryId;

            if (dto.PublishedDate.HasValue)
            {
                existing.PublishedDate = dto.PublishedDate.Value;
            }

            if (dto.ImageFile != null)
            {
                existing.ImageUrl = _fileService.Upload(dto.ImageFile, "BlogImages");
            }

            int rowsAffected = _repository.Update(existing);

            if (rowsAffected == 0)
            {
                throw new Exception("خطا در به‌روزرسانی پست");
            }

            return rowsAffected;
        }
        public int Delete(int id, string userId)
        {
            var existing = GetPostOrThrow(id);
            EnsurePostOwner(existing, userId);

            int rowsAffected = _repository.Delete(id);

            if (rowsAffected == 0)
            {
                throw new Exception("خطا در حذف پست");
            }

            return rowsAffected;
        }

        public ShowPostDto GetById(int id)
        {
            var post = GetPostOrThrow(id);
            return MapToDto(post);
        }

        public List<ShowPostDto> GetRecents(int count)
        {
            return _repository.GetRecents(count).Select(MapToDto).ToList();
        }

        public List<ShowPostDto> GetByCategory(int categoryId)
        {
            return _repository.GetByCategoryId(categoryId).Select(MapToDto).ToList();
        }

        public List<ShowPostDto> GetUserPosts(string userId)
        {
           return _repository.GetByUserId(userId).Select(MapToDto).ToList();
        }

        private void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exception("عنوان الزامی است");
            }
        }

        private void ValidateUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("آیدی کاربر پیدا نشد");
        }

        private BlogPost GetPostOrThrow(int id)
        {
            var post = _repository.GetById(id);
            if (post == null)
            {
                throw new Exception("پست پیدا نشد");
            }

            return post;
        }

        private void EnsurePostOwner(BlogPost post, string userId)
        {
            if (post.AuthorId != userId)
            {
                throw new Exception("خطای دسترسی");
            }
        }

        private void EnsureUserOwnsCategory(string authorId, int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);

            if (category == null)
            {
                throw new Exception("دسته بندی پیدا نشد");
            }

            if (category.OwnerId != authorId)
            {
                throw new Exception("خطای دسترسی");
            }
        }

        private static ShowPostDto MapToDto(BlogPost p)
        {
            return new ShowPostDto
            {
                Id = p.Id,
                Title = p.Title,
                Text = p.Text,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                AuthorId = p.AuthorId,
                PublishedDate = p.PublishedDate,
                ImageUrl = p.ImageUrl
            };
        }
    }
}
