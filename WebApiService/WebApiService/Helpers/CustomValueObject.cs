using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiService
{
    public class CustomValueObject
    {
    }
    public class Fields
    {
        public string Field { get; set; }
        public bool IsRequired { get; set; }
        public int MaxLength { get; set; }
        public string Source { get; set; }
    }

    public class CombinedFileds
    {
        public String EntityName { get; set; }
        public List<Fields> Fields { get; set; }
    }
}