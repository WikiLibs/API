using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SymbolManager.Controllers
{
    [Authorize]
    public class SymbolController : Controller
    {
        private API.Modules.ISymbolManager _symmgr;
        private API.Modules.IUserManager _umgr;

        public SymbolController(API.IModuleManager mgr)
        {
            _symmgr = mgr.GetModule<API.Modules.ISymbolManager>();
            _umgr = mgr.GetModule<API.Modules.IUserManager>();
        }

        public class SymbolUpdate
        {
            public class Prototype
            {
                public class Parameter
                {
                    public string prototype { get; set; }
                    public string description { get; set; }
                    public string path { get; set; }
                }

                public string prototype { get; set; }
                public string description { get; set; }
                public Parameter[] parameters { get; set; }
            }

            public string lang { get; set; }
            public string type { get; set; }
            public Prototype[] prototypes { get; set; }
            public string[] symbols { get; set; }
            public string userId { get; set; }
            public string date { get; set; }
            public string path { get; set; }
        }

        API.Entities.Symbol Convert(string path, SymbolUpdate s)
        {
            var sym = new API.Entities.Symbol
            {
                Path = path,
                UserID = User.FindFirst(ClaimTypes.Name).Value,
                Lang = s.lang,
                Type = s.type,
                Prototypes = new API.Entities.Symbol.Prototype[s.prototypes.Length],
                Symbols = s.symbols
            };

            for (int i = 0; i < s.prototypes.Length; ++i)
            {
                sym.Prototypes[i] = new API.Entities.Symbol.Prototype
                {
                    Proto = s.prototypes[i].prototype,
                    Description = s.prototypes[i].description,
                    Parameters = new API.Entities.Symbol.Prototype.Param[s.prototypes[i].parameters.Length]
                };
                for (int j = 0; j < s.prototypes[i].parameters.Length; ++j)
                {
                    sym.Prototypes[i].Parameters[j] = new API.Entities.Symbol.Prototype.Param
                    {
                        Proto = s.prototypes[i].parameters[j].prototype,
                        Description = s.prototypes[i].parameters[j].description,
                        Path = s.prototypes[i].parameters[j].path
                    };
                }
            }
            return (sym);
        }

        private SymbolUpdate Convert(API.Entities.Symbol sym)
        {
            SymbolUpdate res = new SymbolUpdate
            {
                userId = sym.UserID,
                date = sym.Date.ToString(),
                lang = sym.Lang,
                type = sym.Type,
                path = sym.Path,
                prototypes = new SymbolUpdate.Prototype[sym.Prototypes.Length],
                symbols = sym.Symbols
            };

            for (int i = 0; i < sym.Prototypes.Length; ++i)
            {
                res.prototypes[i] = new SymbolUpdate.Prototype
                {
                    prototype = sym.Prototypes[i].Proto,
                    description = sym.Prototypes[i].Description,
                    parameters = new SymbolUpdate.Prototype.Parameter[sym.Prototypes[i].Parameters.Length]
                };
                for (int j = 0; j < sym.Prototypes[i].Parameters.Length; ++j)
                {
                    res.prototypes[i].parameters[j] = new SymbolUpdate.Prototype.Parameter
                    {
                        prototype = sym.Prototypes[i].Parameters[j].Proto,
                        description = sym.Prototypes[i].Parameters[j].Description,
                        path = sym.Prototypes[i].Parameters[j].Path
                    };
                }
            }
            return (res);
        }

        [Route("/symbol/{*path}")]
        [HttpGet]
        public IActionResult GetSymbol(string path)
        {
            if (path == null)
                return (NotFound());
            var sym = _symmgr.GetSymbol(path);
            if (sym == null)
                return (NotFound());
            return (Json(Convert(sym)));
        }

        [Route("/symbol")]
        [HttpPost]
        public IActionResult NewSymbol([FromHeader] string path, [FromBody] SymbolUpdate sym)
        {
            API.Entities.Symbol symbol = Convert(path, sym);
            var usr = _umgr.GetUser(User.FindFirst(ClaimTypes.Name).Value);

            if (!usr.HasPermission(API.Permissions.CREATE_SYMBOL))
                return (Unauthorized());
            if (_symmgr.GetSymbol(path) != null)
                return (Conflict());
            return (StatusCode(_symmgr.SetSymbol(symbol)));
        }

        [Route("/symbol")]
        [HttpPatch]
        public IActionResult UpdateSymbol([FromHeader] string path, [FromBody] SymbolUpdate sym)
        {
            API.Entities.Symbol symbol = _symmgr.GetSymbol(path);
            var usr = _umgr.GetUser(User.FindFirst(ClaimTypes.Name).Value);

            if (!usr.HasPermission(API.Permissions.UPDATE_SYMBOL))
                return (Unauthorized());
            if (symbol == null)
                return (NotFound());
            var converted = Convert(path, sym);
            symbol.Prototypes = converted.Prototypes;
            symbol.Symbols = converted.Symbols;
            symbol.Type = converted.Type;
            return (StatusCode(_symmgr.SetSymbol(symbol)));
        }

        [Route("/symbol")]
        [HttpDelete]
        public IActionResult DeleteSymbol([FromHeader] string path)
        {
            API.Entities.Symbol symbol = _symmgr.GetSymbol(path);
            var usr = _umgr.GetUser(User.FindFirst(ClaimTypes.Name).Value);

            if (!usr.HasPermission(API.Permissions.DELETE_SYMBOL))
                return (Unauthorized());
            if (symbol == null)
                return (NotFound());
            return (StatusCode(_symmgr.DeleteSymbol(symbol)));
        }
    }
}
