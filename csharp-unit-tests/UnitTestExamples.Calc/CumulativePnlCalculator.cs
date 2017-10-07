using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTestExamples.Extensions;
using UnitTestExamples.Repository;
using UnitTestExamples.Repository.Entities;

namespace UnitTestExamples.Calc
{
    public class CumulativePnlCalculator
    {
        private readonly IQueryRepository _repository;

        public CumulativePnlCalculator(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CalculationResult<CumulativeProfitAndLoss>> GetCumulativePnl(string region, DateTime? startDate = null)
        {
            var pnls = await _repository.GetProfitAndLossForRegion(region, startDate);

            if (pnls == null || !pnls.Any())
            {
                var errorMessage = $"No profit & loss data available for region {region}";
                if (startDate != null)
                {
                    errorMessage += $" using start date {startDate.Value.ToFormattedString()}";
                }

                return new CalculationResult<CumulativeProfitAndLoss> { ErrorMessage = errorMessage };
            }

            return new CalculationResult<CumulativeProfitAndLoss> { Results = GetResults(pnls, region, startDate) };
        }

        private IEnumerable<CumulativeProfitAndLoss> GetResults(IList<ProfitAndLoss> pnls, string region, DateTime? startDate = null)
        {
            var pnlsFromStart = startDate != null && pnls.First().Date > startDate 
                ? (new List<ProfitAndLoss> { new ProfitAndLoss { Date = startDate.Value, Value = 0 } }).Concat(pnls).ToList()
                : pnls; 

            var cumulativePnl = 0.0;
            DateTime? currentDate = null;

            Func<CumulativeProfitAndLoss> newResult = () => new CumulativeProfitAndLoss { Region = region, Date = currentDate.Value, CumulativePnl = (int)Math.Round(cumulativePnl, 0) };

            foreach (var pnl in pnlsFromStart)
            {
                if (currentDate == null)
                {
                    // for first loop only
                    currentDate = pnl.Date;
                }
                else if (currentDate != pnl.Date)
                {
                    // new date found - return result for current date
                    yield return newResult();

                    var daysGap = (pnl.Date - currentDate).Value.Days - 1;
                    for (var i = 0; i < daysGap; i++)
                    {
                        // fill in any missing days between current date and next date
                        currentDate = currentDate.Value.AddDays(1);
                        yield return newResult();
                    }

                    currentDate = pnl.Date;
                }

                // increase cumulative pnl using current date's value
                cumulativePnl += pnl.Value;
            }

            yield return newResult();
        }
    }
}
