using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.File;
using WikiLibs.Shared.Modules.Symbols;

namespace WikiLibs.Symbols
{
    public class LangManager : BaseCRUDOperations<Context, Lang>, ILangManager
    {
        public LangManager(Context ctx) : base(ctx)
        {
        }

        public override async Task<Lang> PatchAsync(long key, Lang mdl)
        {
            var m = await GetAsync(key);

            if (Set.Any(e => e.Id != key && e.Name == mdl.Name))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = key.ToString(),
                    ResourceName = m.Name,
                    ResourceType = typeof(Lang)
                };
            m.Name = mdl.Name;
            m.DisplayName = mdl.DisplayName;
            await SaveChanges();
            return (m);
        }

        public override Task<Lang> PostAsync(Lang mdl)
        {
            if (Set.Any(e => e.Name == mdl.Name))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = mdl.Name,
                    ResourceName = mdl.Name,
                    ResourceType = typeof(Lang)
                };
            return (base.PostAsync(mdl));
        }

        public IEnumerable<Lang> GetAllLangs()
        {
            return (Set.OrderBy(e => e.Name));
        }

        public PageResult<LibListItem> GetFirstLibs(long lang, PageOptions options)
        {
            var lng = Context.SymbolLangs.Find(new object[] { lang });
            var lngName = lng.Name + "/";
            return (base.ToPageResult<LibListItem, Lib>(options,
                Context.SymbolLibs.Where(e => e.Name.StartsWith(lngName))
                                  .OrderBy(e => e.Name)));
        }

        public async Task PostFileAsync(Lang data, ImageFile fle)
        {
            var res = ImageUtils.ResizeImage(fle.OpenReadStream(), new System.Drawing.Size(128, 128), System.Drawing.Imaging.ImageFormat.Jpeg);
            data.Icon = res;
            await SaveChanges();
        }

        class LangIcon : ImageFile
        {
            public override string ContentType => "image/jpeg";

            public override long Length => _data.Length;

            public override string Name => "Image";

            private byte[] _data;

            public LangIcon(byte[] data)
            {
                _data = data;
            }

            public override Stream OpenReadStream()
            {
                return (new MemoryStream(_data));
            }
        }

        public ImageFile GetFile(Lang data)
        {
            if (data.Icon == null || data.Icon.Length <= 0)
                throw new Shared.Exceptions.ResourceNotFound()
                {
                    ResourceId = data.Id.ToString(),
                    ResourceName = "Icon",
                    ResourceType = typeof(Lang)
                };
            return (new LangIcon(data.Icon));
        }
    }
}
