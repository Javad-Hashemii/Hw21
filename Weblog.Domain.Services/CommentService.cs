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
            ValidatePostExists(dto.BlogPostId);
            ValidateCommentText(dto.Text);
            ValidateRating(dto.Rating);

            var comment = new Comment
            {
                BlogPostId = dto.BlogPostId,
                Text = dto.Text,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow,
                Status = CommentStatus.Pending,
                UserId = userId,
                Name = ResolveAuthorName(dto, userId, userName),
                Email = ResolveEmail(dto, userId, dto.GuestEmail)
            };

            return _repository.Add(comment);
        }

        public void Approve(int commentId, string authorId)
        {
            UpdateStatus(commentId, authorId, CommentStatus.Approved);
        }

        public void Reject(int commentId, string authorId)
        {
            UpdateStatus(commentId, authorId, CommentStatus.Rejected);
        }

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

            return comments.Select(MapToManageDto).ToList();
        }

        private void ValidatePostExists(int postId)
        {
            var post = _postRepository.GetById(postId);
            if (post == null)
            {
                throw new Exception("پست پیدا نشد");
            }
        }

        private void ValidateCommentText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new Exception("متن نظر الزامی است");
            }
        }

        private void ValidateRating(int rating)
        {
            if (rating < 1 || rating > 5)
            {
                throw new Exception("Rating must be between 1 and 5.");
            }
        }

        private static string ResolveAuthorName(AddCommentDto dto, string? userId, string? userName)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return !string.IsNullOrWhiteSpace(userName) ? userName : "Registered User";
            }

            if (string.IsNullOrWhiteSpace(dto.GuestName))
            {
                throw new Exception("اسم برای مهمان ها اجباری است");
            }

            return dto.GuestName;
        }

        private static string ResolveEmail(AddCommentDto dto, string? userId, string? guestEmail)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return guestEmail;
            }

            if (string.IsNullOrWhiteSpace(guestEmail))
            {
                throw new Exception("ایمیل برای مهمان ها اجباری است");
            }

            return guestEmail;
        }

        private int UpdateStatus(int commentId, string authorId, CommentStatus newStatus)
        {
            if (string.IsNullOrWhiteSpace(authorId))
            {
                throw new Exception("آیدی کاربر پیدا نشد");
            }

            var comment = _repository.GetById(commentId);
            if (comment == null)
            {
                throw new Exception("نظر پیدا نشد");
            }

            var post = comment.BlogPost ?? _postRepository.GetById(comment.BlogPostId);
            if (post == null || post.AuthorId != authorId)
            {
                throw new Exception("خطای دسترسی");
            }

            comment.Status = newStatus;
            int rowsAffected = _repository.Update(comment);

            if (rowsAffected == 0)
            {
                throw new Exception("خطا در به‌روزرسانی نظر");
            }

            return rowsAffected;
        }


        private static ShowCommentDto ToShowDto(Comment comment)
        {
            return new ShowCommentDto
            {
                Id = comment.Id,
                Text = comment.Text,
                AuthorName = comment.Name,
                CreatedDate = comment.CreatedAt,
                Rating = comment.Rating,
                Status = comment.Status
            };
        }

        private static ManageCommentDto MapToManageDto(Comment c)
        {
            return new ManageCommentDto
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
            };
        }
    }
}
