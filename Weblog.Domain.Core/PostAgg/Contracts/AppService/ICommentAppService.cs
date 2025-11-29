using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Core.PostAgg.Contracts.AppService
{
    public interface ICommentAppService
    {
        int AddComment(AddCommentDto dto, string? userId, string? userName);
        void Approve(int commentId, string authorId);
        void Reject(int commentId, string authorId);
        List<ShowCommentDto> GetApprovedByPostId(int postId);
        List<ManageCommentDto> GetAuthorComments(string authorId, CommentStatus? status = null);
    }
}
