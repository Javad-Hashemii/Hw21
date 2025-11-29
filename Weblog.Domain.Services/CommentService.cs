using System;
using System.Collections.Generic;
using System.Linq;
using Weblog.Domain.Core.PostAgg.Contracts.Repository;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Services
{
    public class CommentService(ICommentRepository _repository, IBlogRepository _postRepository) : ICommentService
    {
        public int AddComment(AddCommentDto dto, string? userId, string? userName)
        {
            var post = _postRepository.GetById(dto.BlogPostId);
            if (post == null)
                throw new KeyNotFoundException("Cannot add comment: Post not found.");

            if (string.IsNullOrWhiteSpace(dto.Text))
                throw new ArgumentException("Comment text is required.");

            if (dto.Rating is < 1 or > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var comment = new Comment
            {
                BlogPostId = dto.BlogPostId,
                Text = dto.Text,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow,
                Status = CommentStatus.Pending,
                UserId = userId,
                Name = ResolveAuthorName(dto, userId, userName),
                Email = ResolveEmail(dto, userId)
            };

            return _repository.Add(comment);
        }

        public void Approve(int commentId, string authorId) =>
            UpdateStatus(commentId, authorId, CommentStatus.Approved);

        public void Reject(int commentId, string authorId) =>
            UpdateStatus(commentId, authorId, CommentStatus.Rejected);

        public List<ShowCommentDto> GetApprovedByPostId(int postId)
        {
            return _repository.GetByPostId(postId)
                .Where(c => c.Status == CommentStatus.Approved)
                .Select(ToShowDto)
                .ToList();
        }

        public List<ManageCommentDto> GetAuthorComments(string authorId, CommentStatus? status = null)
        {
            var comments = _repository.GetByAuthor(authorId);

            if (status.HasValue)
            {
                comments = comments.Where(c => c.Status == status.Value).ToList();
            }

            return comments.Select(c => new ManageCommentDto
            {
                Id = c.Id,
                PostId = c.BlogPostId,
                PostTitle = c.BlogPost?.Title,
                CommenterName = c.Name,
                CommenterEmail = c.Email,
                Text = c.Text,
                Rating = c.Rating,
                CreatedAt = c.CreatedAt,
                Status = c.Status
            }).ToList();
        }

        private static string ResolveAuthorName(AddCommentDto dto, string? userId, string? userName)
        {
            if (!string.IsNullOrWhiteSpace(userId))
                return !string.IsNullOrWhiteSpace(userName) ? userName : "Registered User";

            if (string.IsNullOrWhiteSpace(dto.GuestName))
                throw new ArgumentException("Guest comments require a name.");

            return dto.GuestName;
        }

        private static string? ResolveEmail(AddCommentDto dto, string? userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return dto.GuestEmail; // optional for logged users (can be null)
            }

            if (string.IsNullOrWhiteSpace(dto.GuestEmail))
                throw new ArgumentException("Guest comments require an email.");

            return dto.GuestEmail;
        }

        private void UpdateStatus(int commentId, string authorId, CommentStatus newStatus)
        {
            if (string.IsNullOrWhiteSpace(authorId))
                throw new UnauthorizedAccessException("Author id is required.");

            var comment = _repository.GetById(commentId);
            if (comment == null)
                throw new KeyNotFoundException("Comment not found.");

            var post = comment.BlogPost ?? _postRepository.GetById(comment.BlogPostId);
            if (post == null || post.AuthorId != authorId)
                throw new UnauthorizedAccessException("You cannot moderate this comment.");

            comment.Status = newStatus;
            _repository.Update(comment);
        }

        private static ShowCommentDto ToShowDto(Comment comment) => new ShowCommentDto
        {
            Id = comment.Id,
            Text = comment.Text,
            AuthorName = comment.Name,
            CreatedDate = comment.CreatedAt,
            Rating = comment.Rating,
            Status = comment.Status
        };
    }
}
