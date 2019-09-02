using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WikiLibs.Shared.Modules.File;

namespace WikiLibs.API
{
    public class FileController : Controller
    {
        public class FormFile
        {
            public IFormFile File { get; set; }
        }

        class PrivateFile : IFile
        {
            public string ContentType => _file.ContentType;

            public long Length => _file.Length;

            public string Name => _file.FileName;

            public Stream OpenReadStream()
            {
                var mem = new MemoryStream();
                _stream.Position = 0;
                _stream.CopyTo(mem);
                return (mem);
            }

            private IFormFile _file;

            private Stream _stream;

            public PrivateFile(IFormFile fle)
            {
                _file = fle;
                _stream = fle.OpenReadStream();
            }
        }

        class PrivateImageFile : ImageFile
        {
            public override string ContentType => _file.ContentType;

            public override long Length => _file.Length;

            public override string Name => _file.FileName;

            public override Stream OpenReadStream()
            {
                var mem = new MemoryStream();
                _stream.Position = 0;
                _stream.CopyTo(mem);
                return (mem);
            }

            private IFormFile _file;

            private Stream _stream;

            public PrivateImageFile(IFormFile fle)
            {
                _file = fle;
                _stream = fle.OpenReadStream();
            }
        }

        [NonAction]
        public IFile FileFromForm(FormFile file)
        {
            return (new PrivateFile(file.File));
        }

        [NonAction]
        public ImageFile ImageFileFromForm(FormFile file)
        {
            return (new PrivateImageFile(file.File));
        }
    }
}
