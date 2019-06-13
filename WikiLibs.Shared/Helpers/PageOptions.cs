using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Helpers
{
    public class PageOptions
    {
        public int? Page { get; set; }
        public int? Count { get; set; }

        public void EnsureValid(Type errType, string errName, int maxResults)
        {
            Count = Count == null || Count == 0 ? 10 : Count; //By default expose 10 results
            Count = Count > maxResults ? maxResults : Count; //Ensure a maximum of results
            Page = Page != null ? Page.Value : 1;
            if (Page == 0)
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "PageNum",
                    ResourceName = errName,
                    ResourceType = errType
                };
        }
    }
}
