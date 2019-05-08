using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Admin
{
    public class APIKeyUpdate : PatchModel<APIKeyUpdate, APIKey>
    {
        public string Description { get; set; }
        public int? Flags { get; set; }
        public int? UseNum { get; set; }
        public DateTime ExpirationDate { get; set; }

        public override APIKey CreatePatch(in APIKey current)
        {
            return (new APIKey()
            {
                Id = current.Id,
                Description = Description != null ? Description : current.Description,
                Flags = Flags != null ? Flags.Value : current.Flags,
                UseNum = UseNum != null ? UseNum.Value : current.UseNum,
                ExpirationDate = ExpirationDate != null ? ExpirationDate : current.ExpirationDate
            });
        }
    }
}
