using Microsoft.AspNetCore.Http;

namespace Weblog.Domain.Core.FileAgg.Contracts
{
    public interface IFileService
    {
        string Upload(IFormFile file, string folder);
    }
}
