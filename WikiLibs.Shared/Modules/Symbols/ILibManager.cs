using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.File;

namespace WikiLibs.Shared.Modules.Symbols
{
    public interface ILibManager : ICRUDOperations<Data.Models.Symbols.Lib>,
                                   IFileManager<Data.Models.Symbols.Lib, ImageFile>
    {
        
    }
}