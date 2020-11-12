using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;

namespace WikiLibs.Error
{
    [Module(Interface = typeof(IErrorManager))]
    public class ErrorManager : IErrorManager
    {
        private readonly Data.Context _context;

        public ErrorManager(Data.Context ctx)
        {
            _context = ctx;
        }

        public async Task CleanupAsync()
        {
            _context.Errors.RemoveRange(_context.Errors);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long key)
        {
            _context.RemoveRange(_context.Errors.Where(e => e.Id == key));
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Data.Models.Error> GetLatestErrors()
        {
            return _context.Errors.OrderByDescending(e => e.ErrorDate).Take(5000); //Max out at 5k errors to avoid bandwidth overload
        }

        public async Task PostAsync(Data.Models.Error error)
        {
            error.ErrorDate = DateTime.UtcNow;
            await _context.Errors.AddAsync(error);
            await _context.SaveChangesAsync();
        }
    }
}
