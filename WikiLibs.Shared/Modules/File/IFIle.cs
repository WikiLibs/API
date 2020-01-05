using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WikiLibs.Shared.Modules.File
{
    public interface IFile
    {
        string ContentType { get; }
        long Length { get; }
        string Name { get; }

        Stream OpenReadStream();
    }
}
