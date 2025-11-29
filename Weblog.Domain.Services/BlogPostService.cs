using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Contracts.Repository;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Services
{
    public class BlogPostService(IBlogRepository _repository, IBlogPostImageService _imageService, ICategoryService _categoryService) : IBlogPostService
    {

        public int Create(CreatePostDto dto)
        {
            // 1. Validation
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(dto.AuthorId))
                throw new UnauthorizedAccessException("Author ID is missing.");

            // 2. Business Rule: Check Category Ownership
            var category = _categoryService.GetById(dto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            if (category.OwnerId != dto.AuthorId)
                throw new UnauthorizedAccessException("You cannot post in a category you do not own.");

            // 3. Map DTO -> Entity 
            var post = new BlogPost
            {
                Title = dto.Title,
                Text = dto.Text,
                CategoryId = dto.CategoryId,
                AuthorId = dto.AuthorId,
                PublishedDate = DateTime.UtcNow
            };

            // 4. Persist Post to get the ID
            int newPostId = _repository.Add(post);

            // 5. Delegate Image Creation to ImageService
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var filesToUpload = new List<IFormFile> { dto.ImageFile };
                var storedPaths = _imageService.Create(newPostId, filesToUpload);
                if (storedPaths.Any())
                {
                    post.ImageUrl = storedPaths.First(); //fuck
                    post.Id = newPostId;
                    _repository.Update(post);
                }
            }

            return newPostId;
        }

        public void Update(UpdatePostDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(dto.AuthorId))
                throw new UnauthorizedAccessException("Author ID is missing.");

            var existing = _repository.GetById(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Post not found.");

            if (existing.AuthorId != dto.AuthorId)
                throw new UnauthorizedAccessException("You do not have permission to edit this post.");

            var category = _categoryService.GetById(dto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            if (category.OwnerId != dto.AuthorId)
                throw new UnauthorizedAccessException("You cannot move the post to a category you do not own.");

            existing.Title = dto.Title;
            existing.Text = dto.Text;
            existing.CategoryId = dto.CategoryId;
            if (dto.PublishedDate.HasValue)
                existing.PublishedDate = dto.PublishedDate.Value;

            _repository.Update(existing);

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var filesToUpload = new List<IFormFile> { dto.ImageFile };
                var storedPaths = _imageService.Create(existing.Id, filesToUpload);
                if (storedPaths.Any())
                {
                    existing.ImageUrl = storedPaths.First(); //fuck
                    _repository.Update(existing);
                }
            }
        }

        public void Delete(int id, string userId)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException("Post not found.");

            if (existing.AuthorId != userId)
                throw new UnauthorizedAccessException("You do not have permission to delete this post.");

            if (!_repository.Delete(id))
                throw new InvalidOperationException("Failed to delete post.");
        }

        // --- Reads now return ShowPostDto ---

        public ShowPostDto GetById(int id)
        {
            var post = _repository.GetById(id);
            if (post == null) return null;

            return MapToDto(post);
        }

        public List<ShowPostDto> GetRecents(int count)
        {
            var posts = _repository.GetRecents(count);
            return posts.Select(MapToDto).ToList();
        }

        public List<ShowPostDto> GetByCategory(int categoryId)
        {
            var posts = _repository.GetByCategoryId(categoryId);
            return posts.Select(MapToDto).ToList();
        }

        public List<ShowPostDto> GetUserPosts(string userId)
        {
            var posts = _repository.GetByUserId(userId);
            return posts.Select(MapToDto).ToList();
        }

        // Helper for mapping Entity -> ShowPostDto
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
                ImageUrls = p.Images?.Select(i => i.ImagePath).ToList() ?? new List<string>(),
                CoverImageUrl = !string.IsNullOrWhiteSpace(p.ImageUrl) //fuck
                    ? p.ImageUrl //fuck
                    : p.Images?.Select(i => i.ImagePath).FirstOrDefault()
            };
        }
    }
}
