using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTestExamples.Extensions;
using UnitTestExamples.Repository;

namespace UnitTestExamples.Calc
{
    public class CompoundDailyReturnsCalculator
    {
        private readonly IQueryRepository _repository;

        public CompoundDailyReturnsCalculator(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CalculationResult<CompoundReturns>> GetCompoundReturns(string strategy)
        {
            var capitals = (await _repository.GetCapitalsForStrategy(strategy)).OrderBy(x => x.Date).ToList();
            if (capitals == null || !capitals.Any())
            {
                return new CalculationResult<CompoundReturns> { ErrorMessage = $"No capitals data available for strategy {strategy}" };
            }

            var pnls = (await _repository.GetProfitAndLossForStrategy(strategy)).OrderBy(x => x.Date).ToList();
            if (pnls == null || !pnls.Any())
            {
                return new CalculationResult<CompoundReturns> { ErrorMessage = $"No profit & loss data available for strategy {strategy}" };
            }

            var firstCapital = capitals.First();
            var firstPnl = pnls.First();
            if (firstPnl.Date < firstCapital.Date)
            {
                return new CalculationResult<CompoundReturns> { ErrorMessage = $"Invalid data: first profit & loss at {firstPnl.Date.ToFormattedString()} when no capital data available until {firstCapital.Date.ToFormattedString()}" };
            }

            var dailyReturns = new Dictionary<DateTime, double>();

            if (firstPnl.Date > firstCapital.Date)
            {
                // "Returns a time series of compound daily returns since inception" -> inception == first available capital
                dailyReturns[firstCapital.Date] = 0;
            }

            foreach (var pnl in pnls)
            {
                var capital = capitals.FirstOrDefault(x => x.Date.Month == pnl.Date.Month && x.Date.Year == pnl.Date.Year);
                if (capital == null)
                {
                    return new CalculationResult<CompoundReturns> { ErrorMessage = $"Invalid data: no monthly capital available for date {pnl.Date.ToFormattedString()}" };
                }

                var dailyReturn = GetDailyReturn(pnl.Value, capital.Value);
                if (dailyReturn == null)
                {
                    return new CalculationResult<CompoundReturns> { ErrorMessage = $"Invalid data: non-zero pnl value of {pnl.Value} at {pnl.Date.ToFormattedString()} when capital value is 0" };
                }

                if (dailyReturns.ContainsKey(pnl.Date))
                {
                    return new CalculationResult<CompoundReturns> { ErrorMessage = $"Invalid data: duplicate pnl values provided for date {pnl.Date.ToFormattedString()}" };
                }

                dailyReturns[pnl.Date] = dailyReturn.Value;
            }

            return new CalculationResult<CompoundReturns> { Results = GetCompoundReturns(dailyReturns, strategy) };
        }

        private double? GetDailyReturn(double dailyPnl, double monthlyCapital)
        {
            if (monthlyCapital == 0)
            {
                if (dailyPnl == 0)
                {
                    return 0;
                }
                else
                {
                    // How can a strategy make any profit or loss on an investment of 0? Looks like invalid data.
                    // Don't attempt to guess at what capital.Value should be (e.g. by using previous month's value)
                    // since this could be an invalid assumption. Instead, return error.
                    return null;
                }
            }
            else
            {
                return dailyPnl / monthlyCapital;
            }
        }

        private IEnumerable<CompoundReturns> GetCompoundReturns(IDictionary<DateTime, double> dailyReturns, string strategy)
        {
            var compoundReturn = 0.0;
            for (var date = dailyReturns.First().Key; date <= dailyReturns.Last().Key; date = date.AddDays(1))
            {
                var dailyReturn = dailyReturns.ContainsKey(date) ? dailyReturns[date] : 0;
                compoundReturn = (1 + compoundReturn) * (1 + dailyReturn) - 1;

                yield return new CompoundReturns
                {
                    Strategy = strategy,
                    Date = date,
                    CompoundReturn = Math.Round(compoundReturn, 5)
                };
            }    
        }
    }
}
