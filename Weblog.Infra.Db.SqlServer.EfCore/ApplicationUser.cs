using Microsoft.AspNetCore.Identity;

namespace Weblog.Infra.Db.SqlServer.EfCore
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
