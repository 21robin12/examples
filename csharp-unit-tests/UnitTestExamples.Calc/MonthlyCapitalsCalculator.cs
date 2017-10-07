using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTestExamples.Repository;

namespace UnitTestExamples.Calc
{
    public class MonthlyCapitalsCalculator
    {
        private readonly IQueryRepository _repository;

        public MonthlyCapitalsCalculator(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CalculationResult<MonthlyCapital>> GetMonthlyCapitals(IList<string> strategies)
        {
            var capitals = await _repository.GetCapitalsForStrategies(strategies);
            if (capitals == null || !capitals.Any())
            {
                return new CalculationResult<MonthlyCapital> { ErrorMessage = $"No capitals data available for strategies [{string.Join(", ", strategies)}]" };
            }

            var results = capitals
                .OrderBy(x => x.Date)
                .ThenBy(x => x.Strategy.Name)
                .Select(x => new MonthlyCapital { Strategy = x.Strategy.Name, Capital = (int)Math.Round(x.Value, 0), Date = x.Date });

            return new CalculationResult<MonthlyCapital> { Results = results };
        }
    }
}
