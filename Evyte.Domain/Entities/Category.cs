﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string ImageId { get; set; }
        public string ImageUrl { get; set; }
        public int SortingNumber { get; set; }
        public IEnumerable<Design> Designs { get; set; }
    }
}
