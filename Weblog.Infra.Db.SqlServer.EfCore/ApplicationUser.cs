using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weblog.Infra.Db.SqlServer.EfCore
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
