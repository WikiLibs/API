using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Admin
{
    public class ApiKeyUpdate : PatchModel<ApiKeyUpdate, ApiKey>
    {
        public string Description { get; set; }
        public int? Flags { get; set; }
        public int? UseNum { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public override ApiKey CreatePatch(in ApiKey current)
        {
            return (new ApiKey()
            {
                Id = current.Id,
                Description = Description != null ? Description : current.Description,
                Flags = Flags != null ? Flags.Value : current.Flags,
                UseNum = UseNum != null ? UseNum.Value : current.UseNum,
                ExpirationDate = ExpirationDate != null ? ExpirationDate.Value : current.ExpirationDate
            });
        }
    }
}
