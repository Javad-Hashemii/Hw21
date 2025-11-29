using System;
using System.Collections.Generic;
using System.Text;

namespace Weblog.Domain.Core.UserAgg
{
    public interface IUserLookupService
    {
        string? GetUserNameById(string userId);
    }
}
