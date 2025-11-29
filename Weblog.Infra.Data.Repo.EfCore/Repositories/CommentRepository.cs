using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weblog.Domain.Core.PostAgg.Contracts.Repository;
using Weblog.Domain.Core.PostAgg.Entities;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Infra.Data.Repo.EfCore.Repositories
{
    public class CommentRepository(AppDbContext dbContext) : ICommentRepository
    {
        public int Add(Comment comment)
        {
            dbContext.Comments.Add(comment);
            dbContext.SaveChanges();
            return comment.Id;
        }

        public bool Delete(int id)
        {
            var comment = dbContext.Comments.Find(id);
            if (comment == null) return false;

            dbContext.Comments.Remove(comment);
            return dbContext.SaveChanges() > 0;
        }

        public Comment GetById(int id)
        {
            return dbContext.Comments
                .Include(c => c.BlogPost)
                .FirstOrDefault(c => c.Id == id);
        }

        public List<Comment> GetByPostId(int postId)
        {
            return dbContext.Comments
                .Where(c=>c.BlogPostId == postId)
                .OrderByDescending(c=>c.CreatedAt)
                .ToList();
        }

        public List<Comment> GetByAuthor(string authorId)
        {
            return dbContext.Comments
                .Include(c => c.BlogPost)
                .Where(c => c.BlogPost.AuthorId == authorId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

        public void Update(Comment comment)
        {
            dbContext.Comments.Update(comment);
            dbContext.SaveChanges();
        }
    }
}
