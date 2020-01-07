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

        [AuthorizeApiKey(Flag = AuthorizeApiKey.SelfDestruct)]
        [HttpDelete("symbols")]
        public async Task<IActionResult> SelfDestructSymbols()
        {
            _context.RemoveRange(_context.Symbols);
            _context.RemoveRange(_context.Prototypes);
            _context.RemoveRange(_context.PrototypeParams);
            _context.RemoveRange(_context.PrototypeParamSymbolRefs);
            _context.RemoveRange(_context.SymbolLibs);
            _context.RemoveRange(_context.SymbolImports);
            _context.RemoveRange(_context.SymbolRefs);
            _context.RemoveRange(_context.Examples);
            _context.RemoveRange(_context.ExampleRequests);
            _context.RemoveRange(_context.ExampleCodeLines);
            _context.RemoveRange(_context.ExampleRequests);
            await _context.SaveChangesAsync();
            return (Ok());
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.SelfDestruct)]
        [HttpDelete("all")]
        public async Task<IActionResult> SelfDestructAll()
        {
            _context.RemoveRange(_context.Symbols);
            _context.RemoveRange(_context.Prototypes);
            _context.RemoveRange(_context.PrototypeParams);
            _context.RemoveRange(_context.PrototypeParamSymbolRefs);
            _context.RemoveRange(_context.SymbolLibs);
            _context.RemoveRange(_context.SymbolImports);
            _context.RemoveRange(_context.SymbolRefs);
            _context.RemoveRange(_context.Examples);
            _context.RemoveRange(_context.ExampleRequests);
            _context.RemoveRange(_context.ExampleCodeLines);
            _context.RemoveRange(_context.ExampleRequests);
            _context.RemoveRange(_context.SymbolLangs);
            _context.RemoveRange(_context.Users);
            _context.RemoveRange(_context.Groups);
            _context.RemoveRange(_context.Permissions);
            _context.RemoveRange(_context.ApiKeys);
            await _context.SaveChangesAsync();
            return (Ok());
        }
    }
}