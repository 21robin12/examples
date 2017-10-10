using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTestExamples.Calc.Tests.Extensions;
using UnitTestExamples.Repository;
using UnitTestExamples.Repository.Entities;

namespace UnitTestExamples.Calc.Tests
{
    [TestClass]
    public class CompoundDailyReturnsCalculatorTests
    {
        [TestMethod]
        public async Task GetCompoundReturns_NoCapitals_ReturnsError()
        {
            IList<Capital> repositoryCapitalResults = new List<Capital>();
            IList<ProfitAndLoss> repositoryPnlResults = new List<ProfitAndLoss> { new ProfitAndLoss() };
            var mockRepository = new Mock<IQueryRepository>();
            mockRepository.Setup(x => x.GetCapitalsForStrategy(It.IsAny<string>())).Returns(Task.FromResult(repositoryCapitalResults));
            mockRepository.Setup(x => x.GetProfitAndLossForStrategy(It.IsAny<string>())).Returns(Task.FromResult(repositoryPnlResults));

            var calculator = new CompoundDailyReturnsCalculator(mockRepository.Object);

            var result = await calculator.GetCompoundReturns("StrategyWithNoCapitals");

            Assert.IsNull(result.Results);
            Assert.IsTrue(result.ErrorMessage == "No capitals data available for strategy StrategyWithNoCapitals");
        }

        [TestMethod]
        public async Task GetCompoundReturns_NoProfitAndLoss_ReturnsError()
        {
            IList<Capital> repositoryCapitalResults = new List<Capital> { new Capital() };
            IList<ProfitAndLoss> repositoryPnlResults = new List<ProfitAndLoss>();
            var mockRepository = new Mock<IQueryRepository>();
            mockRepository.Setup(x => x.GetCapitalsForStrategy(It.IsAny<string>())).Returns(Task.FromResult(repositoryCapitalResults));
            mockRepository.Setup(x => x.GetProfitAndLossForStrategy(It.IsAny<string>())).Returns(Task.FromResult(repositoryPnlResults));

            var calculator = new CompoundDailyReturnsCalculator(mockRepository.Object);

            var result = await calculator.GetCompoundReturns("StrategyWithNoPnl");

            Assert.IsNull(result.Results);
            Assert.IsTrue(result.ErrorMessage == "No profit & loss data available for strategy StrategyWithNoPnl");
        }

        [TestMethod]
        public async Task GetCompoundReturns_OneStrategy_ReturnsCorrectResults()
        {
            var strategy1 = new Strategy { Name = "TestStrategy1" };

            IList<Capital> repositoryCapitalResults = new List<Capital>
            {
                new Capital { Date = new DateTime(2016, 1, 1), Value = 10000, Strategy = strategy1 },
                new Capital { Date = new DateTime(2016, 2, 1), Value = 20000, Strategy = strategy1 },
                new Capital { Date = new DateTime(2016, 3, 1), Value = 19000, Strategy = strategy1 },
                new Capital { Date = new DateTime(2016, 4, 1), Value = 28000, Strategy = strategy1 },
            };

            IList<ProfitAndLoss> repositoryPnlResults = new List<ProfitAndLoss>
            {
                new ProfitAndLoss { Date = new DateTime(2016, 1, 5), Value = 100, Strategy = strategy1 },
                new ProfitAndLoss { Date = new DateTime(2016, 2, 21), Value = 200, Strategy = strategy1 },
                new ProfitAndLoss { Date = new DateTime(2016, 2, 22), Value = -50, Strategy = strategy1 },
                new ProfitAndLoss { Date = new DateTime(2016, 4, 7), Value = 300, Strategy = strategy1 }
            };

            var mockRepository = new Mock<IQueryRepository>();
            mockRepository.Setup(x => x.GetCapitalsForStrategy(It.IsAny<string>())).Returns(Task.FromResult(repositoryCapitalResults));
            mockRepository.Setup(x => x.GetProfitAndLossForStrategy(It.IsAny<string>())).Returns(Task.FromResult(repositoryPnlResults));

            var calculator = new CompoundDailyReturnsCalculator(mockRepository.Object);

            var result = await calculator.GetCompoundReturns("TestStrategy1");

            Assert.IsNull(result.ErrorMessage);
            Assert.IsNotNull(result.Results);
            var results = result.Results.ToList();
            Assert.IsTrue(results.Select(x => x.Date).ToList().IsDailyTimeSeries());
            Assert.IsTrue(results.First().Date == repositoryCapitalResults.First().Date); 
            Assert.IsTrue(results.Last().Date == repositoryPnlResults.Last().Date);

            var expectedResults = ExpectedResults();
            for (var i = 0; i < results.Count; i++)
            {
                var expected = expectedResults[i];
                var actual = results[i].CompoundReturn;
                Assert.IsTrue(expected == actual);
            }
        } 

        // from compound-daily-returns-test.xlsx
        private IList<double> ExpectedResults()
        {
            return new List<double>
            {
                0.00000,
                0.00000,
                0.00000,
                0.00000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.01000,
                0.02010,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.01755,
                0.02845
            };
        }
    }
}
