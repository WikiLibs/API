using System;
using System.Linq;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules.Symbols
{
    public class SymbolListItem : IPageResultModel<SymbolListItem, Symbol>
    {
        public class LangObject
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
        }

        public class LibObject
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public long Id { get; set; }
        public string Path { get; set; }
        public string TypeName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime LastModificationDate { get; set; }
        public LangObject Lang { get; set; }
        public LibObject Lib { get; set; }
        public long Views { get; set; }

        public SymbolListItem Map(Symbol model)
        {
            Id = model.Id;
            Path = model.Path;
            TypeName = model.Type.DisplayName;
            Lib = new LibObject()
            {
                Name = model.Lib.Name,
                Id = model.LibId
            };
            UserId = model.UserId;
            UserName = model.User != null ? model.User.Pseudo : null;
            LastModificationDate = model.LastModificationDate;
            Views = model.Views;
            Lang = new LangObject()
            {
                Id = model.LangId,
                Name = model.Lang.Name,
                DisplayName = model.Lang.DisplayName
            };
            return (this);
        }
    }

    public class LibListItem : IPageResultModel<LibListItem, Lib>
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public LibListItem Map(Lib model)
        {
            Id = model.Id;
            Name = model.Name;
            return (this);
        }
    }

    public class SymbolReference : IPageResultModel<SymbolReference, Symbol>
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string FirstPrototype { get; set; }

        public SymbolReference Map(Symbol model)
        {
            Id = model.Id;
            Path = model.Path;
            Type = model.Type.Name;
            FirstPrototype = model.Prototypes.FirstOrDefault() != null ? model.Prototypes.FirstOrDefault().Data : null;
            return (this);
        }
    }
}
