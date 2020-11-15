using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;

namespace WikiLibs.Core.Controllers
{
    [ApiController]
    [Route("/self-destruct")]
    public class SelfDestructController : Controller
    {
        private Data.Context _context;

        public SelfDestructController(Data.Context ctx)
        {
            _context = ctx;
        }

        [Authorize(AuthenticationSchemes = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.SelfDestruct)]
        [HttpDelete("symbols")]
        public async Task<IActionResult> SelfDestructSymbols()
        {
            _context.RemoveRange(_context.Symbols);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.Prototypes);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.PrototypeParams);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.PrototypeParamSymbolRefs);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.SymbolLibs);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.SymbolImports);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.SymbolRefs);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.Examples);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.ExampleRequests);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.ExampleCodeLines);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.ExampleRequests);
            await _context.SaveChangesAsync();
            return (Ok());
        }

        [Authorize(AuthenticationSchemes = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.SelfDestruct)]
        [HttpDelete("all")]
        public async Task<IActionResult> SelfDestructAll()
        {
            _context.RemoveRange(_context.Symbols);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.Prototypes);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.PrototypeParams);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.PrototypeParamSymbolRefs);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.SymbolLibs);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.SymbolImports);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.SymbolRefs);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.Examples);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.ExampleRequests);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.ExampleCodeLines);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.ExampleRequests);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.SymbolLangs);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.Groups);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.Permissions);
            await _context.SaveChangesAsync();
            _context.RemoveRange(_context.ApiKeys);
            await _context.SaveChangesAsync();
            return (Ok());
        }
    }
}