using System;
using System.Collections.Generic;
using System.Text;

namespace Weblog.Domain.Core.CategoryAgg.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
    }
}
