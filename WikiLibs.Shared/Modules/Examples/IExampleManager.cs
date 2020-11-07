using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Service;

namespace WikiLibs.Shared.Modules.Examples
{
    public interface IExampleManager : ICRUDOperations<Data.Models.Examples.Example>
    {
        IQueryable<Data.Models.Examples.Example> GetForSymbol(long symbol);
        Task UpVote(IUser user, long exampleId);
        Task DownVote(IUser user, long exampleId);
        bool HasAlreadyVoted(IUser user, long exampleId);
    }
}
