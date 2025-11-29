using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weblog.Domain.Core.PostAgg.Contracts.Service
{
    public interface IBlogPostImageService
    {

        List<string> Create(int blogPostId, List<IFormFile> files);

        List<string> GetAll(int blogPostId);
    }
}
