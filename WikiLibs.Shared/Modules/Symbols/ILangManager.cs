﻿using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.File;

namespace WikiLibs.Shared.Modules.Symbols
{
    public interface ILangManager : ICRUDOperations<Data.Models.Symbols.Lang>,
                                    IFileManager<Data.Models.Symbols.Lang, ImageFile>
    {
        IEnumerable<Data.Models.Symbols.Lang> GetAllLangs();
        PageResult<LibListItem> GetFirstLibs(long lang, PageOptions options);
    }
}
