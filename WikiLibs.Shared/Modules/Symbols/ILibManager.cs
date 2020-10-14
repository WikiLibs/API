using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.File;

namespace WikiLibs.Shared.Modules.Lib
{
    public interface ILibManager : IModule, ICRUDOperations<Data.Models.Symbols.Lib>,
                                   IFileManager<Data.Models.Symbols.Lib, ImageFile>
    {
        ILangManager LangManager { get; }
        ICRUDOperations<Data.Models.Symbols.Type> TypeManager { get; }

        Task<Data.Models.Symbols.Lib> getAsync(string path);

        Task OptimizeAsync();
    }
}