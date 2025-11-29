using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weblog.Domain.Core.FileAgg.Contracts
{
    public interface IFileService
    {
        string Upload(IFormFile file, string folder);
    }
}
