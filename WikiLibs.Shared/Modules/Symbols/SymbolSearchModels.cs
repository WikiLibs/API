using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules.Symbols
{
    public class SymbolListItem : IPageResultModel<SymbolListItem, Symbol>
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }

        public SymbolListItem Map(Symbol model)
        {
            Id = model.Id;
            Path = model.Path;
            Type = model.Type.Name;
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

    public class LangListItem : IPageResultModel<LangListItem, Lang>
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public LangListItem Map(Lang model)
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

        public SymbolReference Map(Symbol model)
        {
            Id = model.Id;
            Path = model.Path;
            return (this);
        }
    }
}
