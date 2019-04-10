﻿using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.DTO.Input
{
    public class UserUpdate : IPatchDTO<Data.Models.User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Icon { get; set; }
        public string Email { get; set; }
        public bool Private { get; set; }
        public string ProfileMsg { get; set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }

        public User CreatePatch(in User current)
        {
            throw new NotImplementedException();
        }
    }
}