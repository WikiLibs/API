using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules.File
{
    public abstract class ImageFile : IFile
    {
        public abstract string ContentType { get; }

        public abstract long Length { get; }

        public abstract string Name { get; }

        public abstract Stream OpenReadStream();

        public async Task<string> ToBase64()
        {
            using (var mem = new MemoryStream())
            {
                await OpenReadStream().CopyToAsync(mem);
                return (ContentType + "," + Convert.ToBase64String(mem.ToArray()));
            }
        }
    }
}
