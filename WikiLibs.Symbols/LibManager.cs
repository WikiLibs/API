using Microsoft.EntityFrameworkCore.Internal;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.File;
using WikiLibs.Shared.Modules.Symbols;

namespace WikiLibs.Symbols
{
    public class LibManager : BaseCRUDOperations<Context, Lib>, ILibManager
    {
        public LibManager(Context db) : base(db)
        {

        }

        public override Task<Lib> PostAsync(Lib mdl)
        {
            if (Set.Any(e => e.Name == mdl.Name))
                throw new Shared.Exceptions.ResourceAlreadyExists
                {
                    ResourceId = "0",
                    ResourceName = mdl.Name,
                    ResourceType = typeof(Lib)
                };
            return base.PostAsync(mdl);
        }

        public override async Task<Lib> PatchAsync(long key, Lib mdl)
        {
            if (Set.Any(e => e.Id == mdl.Id || e.DisplayName == mdl.DisplayName))
                throw new Shared.Exceptions.ResourceAlreadyExists
                {
                    ResourceId = "0",
                    ResourceName = mdl.Name,
                    ResourceType = typeof(Lib)
                };
            var model = await GetAsync(key);

            model.Copyright = mdl.Copyright;
            model.Description = mdl.Description;
            model.DisplayName = mdl.DisplayName;
            await SaveChanges();
            return model;
        }

        public async Task PostFileAsync(Lib data, ImageFile fle)
        {
            data.Icon = ImageUtils.ResizeImage(fle.OpenReadStream(), new Size(128, 128), ImageFormat.Png);
            await SaveChanges();
        }

        class LibIcon : ImageFile
        {
            public override string ContentType => "image/png";

            public override long Length => _data.Length;

            public override string Name => "Image";

            private byte[] _data;

            public LibIcon(byte[] data)
            {
                _data = data;
            }

            public override Stream OpenReadStream()
            {
                return (new MemoryStream(_data));
            }
        }

        public ImageFile GetFile(Lib data)
        {
            if (data.Icon == null || data.Icon.Length <= 0)
                throw new Shared.Exceptions.ResourceNotFound()
                {
                    ResourceId = data.Id.ToString(),
                    ResourceName = "Icon",
                    ResourceType = typeof(Lib)
                };
            return new LibIcon(data.Icon);
        }
    }
}