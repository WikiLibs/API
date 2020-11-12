using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WikiLibs.Data.Models
{
    public class Error : Model
    {
        //The description of the API key that has been used to post the error
        public string Description { get; set; }

        //The error message (exception.Message in C#)
        public string ErrorMessage { get; set; }

        //The error data (exception.ToString in C#)
        public string ErrorData { get; set; }

        //Date and time of error
        public DateTime ErrorDate { get; set; }
    }
}
