using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.ImportMySQLDB
{
    class Program
    {
        public static string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
                return;
            string oldDbConnect = args[0];
            string newDbConnect = args[1];
            var newctx = new Data.Context(new DbContextOptionsBuilder().UseLazyLoadingProxies().UseSqlServer(newDbConnect).Options);
            var oldctx = new DB.Context(new DbContextOptionsBuilder<DB.Context>().UseMySql(oldDbConnect).Options);
            foreach (var grp in oldctx.Groups)
            {
                var newGrp = new Data.Models.Group()
                {
                    Name = grp.Name
                };
                foreach (var perm in oldctx.Permissions.Where(o => o.Group == grp.Name))
                {
                    var newPerm = new Data.Models.Permission()
                    {
                        Group = newGrp,
                        Perm = perm.Perm
                    };
                    newGrp.Permissions.Add(newPerm);
                }
                newctx.Groups.Add(newGrp);
            }
            newctx.SaveChanges();
            foreach (var usr in oldctx.Users)
            {
                var newUsr = new Data.Models.User()
                {
                    Id = usr.UUID,
                    FirstName = usr.FirstName,
                    LastName = usr.LastName,
                    Icon = usr.Icon,
                    EMail = usr.EMail,
                    Private = usr.ShowEmail,
                    ProfileMsg = usr.ProfileMsg,
                    Points = usr.Points,
                    Pseudo = usr.Pseudo,
                    Group = newctx.Groups.Where(g => g.Name == usr.Group).First(),
                    Pass = SHA512(usr.Pass),
                    RegistrationDate = usr.Date
                };
                newctx.Users.Add(newUsr);
            }
            newctx.SaveChanges();
            foreach (var sym in oldctx.Symbols)
            {
                var newSym = new Data.Models.Symbols.Symbol()
                {
                    Path = sym.Path,
                    CreationDate = sym.Date,
                    LastModificationDate = DateTime.UtcNow,
                    Type = sym.Type,
                    User = newctx.Users.Where(u => u.Id == sym.UserID).FirstOrDefault()
                };
                foreach (var proto in JsonConvert.DeserializeObject<API.Entities.Symbol.Prototype[]>(sym.Prototypes))
                {
                    var newProto = new Data.Models.Symbols.Prototype()
                    {
                        Description = proto.Description,
                        Symbol = newSym,
                        Data = proto.Proto
                    };
                    foreach (var param in proto.Parameters)
                    {
                        var newParam = new Data.Models.Symbols.PrototypeParam()
                        {
                            Description = param.Description,
                            Data = param.Proto,
                            SymbolRef = param.Path != null ? new PrototypeParamSymbolRef() { RefPath = param.Path } : null,
                            Prototype = newProto
                        };
                        newProto.Parameters.Add(newParam);
                    }
                    newSym.Prototypes.Add(newProto);
                }
                foreach (var symref in JsonConvert.DeserializeObject<string[]>(sym.Symbols))
                {
                    var newSymRef = new Data.Models.Symbols.SymbolRef()
                    {
                        Path = symref,
                        Symbol = newSym
                    };
                    newSym.Symbols.Add(newSymRef);
                }
                newctx.Symbols.Add(newSym);
            }
            newctx.SaveChanges();
            foreach (var info in oldctx.InfoTable)
            {
                var newInfo = new Data.Models.Symbols.Info()
                {
                    Data = info.Data,
                    Type = (Data.Models.Symbols.EInfoType)info.Type
                };
                newctx.InfoTable.Add(newInfo);
            }
            newctx.SaveChanges();
        }
    }
}
