using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input
{
    public class ErrorCreate : PostModel<ErrorCreate, Error>
    {
        [Required]
        public string ErrorMessage { get; set; }

        [Required]
        public string ErrorData { get; set; }

        public override Error CreateModel()
        {
            return new Error()
            {
                ErrorData = ErrorData,
                ErrorMessage = ErrorMessage
            };
        }
    }
}
