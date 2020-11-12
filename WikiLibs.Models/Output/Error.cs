using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Models.Output
{
    public class Error : GetModel<Error, Data.Models.Error>
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string ErrorData { get; set; }
        public string ErrorMessage { get; set; }

        public override void Map(in Data.Models.Error model)
        {
            Id = model.Id;
            Description = model.Description;
            ErrorData = model.ErrorData;
            ErrorMessage = model.ErrorMessage;
        }
    }
}
