using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.DTO.Input
{
    public class UserUpdateGlobal : IPatchDTO<Data.Models.User>
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string icon { get; set; }
        public string email { get; set; }
        public bool @private { get; set; }
        public string profileMsg { get; set; }
        public string pseudo { get; set; }
        public int points { get; set; }
        public string group { get; set; }

        public User CreatePatch(in User current)
        {
            throw new NotImplementedException();
        }
    }
}
