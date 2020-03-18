using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules.Symbols
{
    public class SearchQuery
    {
        [Required]
        public string Path { get; set; }
        public long? LangId { get; set; }
        public long? LibId { get; set; }
        public string Type { get; set; }
        public PageOptions PageOptions { get; set; }
    }
}
