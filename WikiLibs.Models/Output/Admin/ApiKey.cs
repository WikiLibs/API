using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Output.Admin
{
    public class ApiKey : GetModel<ApiKey, Data.Models.ApiKey>
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Origin { get; set; }
        public int UseNum { get; set; }
        public int Flags { get; set; }
        public DateTime ExpirationDate { get; set; }

        public override void Map(in Data.Models.ApiKey model)
        {
            Id = model.Id;
            Description = model.Description;
            UseNum = model.UseNum;
            Flags = model.Flags;
            ExpirationDate = model.ExpirationDate;
            Origin = model.Origin;
        }
    }
}
