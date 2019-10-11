using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules.File
{
    public interface IFileManager<DataType, FInterface>
        where FInterface : IFile
    {
        Task PostFileAsync(DataType data, FInterface fle);
        FInterface GetFile(DataType data);
    }
}
