using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Contracts.AppService;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Appservices
{
    public class CommentAppService(ICommentService _commentService):ICommentAppService
    {
        public int AddComment(AddCommentDto dto, string? userId, string? userName)
        {
            return _commentService.AddComment(dto, userId, userName);
        }

        public void Approve(int commentId, string authorId)
        {
            _commentService.Approve(commentId, authorId);
        }

        public void Reject(int commentId, string authorId)
        {
            _commentService.Reject(commentId, authorId);
        }

        public List<ShowCommentDto> GetApprovedByPostId(int postId)
        {
            return _commentService.GetApprovedByPostId(postId);
        }

        public List<ManageCommentDto> GetAuthorComments(string authorId, CommentStatus? status = null)
        {
            return _commentService.GetAuthorComments(authorId, status);
        }
    }
}
