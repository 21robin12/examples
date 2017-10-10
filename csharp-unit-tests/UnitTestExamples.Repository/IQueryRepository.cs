using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTestExamples.Repository.Entities;

namespace UnitTestExamples.Repository
{
    public interface IQueryRepository
    {
        Task<IList<ProfitAndLoss>> GetProfitAndLossForRegion(string region, DateTime? startDate = null);
        Task<IList<ProfitAndLoss>> GetProfitAndLossForStrategy(string strategy);
        Task<IList<Capital>> GetCapitalsForStrategies(IList<string> strategies);
        Task<IList<Capital>> GetCapitalsForStrategy(string strategy);
    }
}
