﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules.Symbols
{
    public class SymbolListItem : IPageResultModel<SymbolListItem, Symbol>
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public string TypeName { get; set; }
        public long TypeId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModificationDate { get; set; }
        public long LangId { get; set; }
        public string LangName { get; set; }
        public long LibId { get; set; }
        public string LibName { get; set; }
        public long Views { get; set; }

        public SymbolListItem Map(Symbol model)
        {
            Id = model.Id;
            Path = model.Path;
            TypeName = model.Type.DisplayName;
            TypeId = model.Type.Id;
            UserId = model.UserId;
            UserName = model.User != null ? model.User.Pseudo : null;
            LastModificationDate = model.LastModificationDate;
            CreationDate = model.CreationDate;
            LangName = model.Lang.DisplayName;
            LibName = model.Lib.Name;
            Views = model.Views;
            LibId = model.LibId;
            LangId = model.LangId;
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
