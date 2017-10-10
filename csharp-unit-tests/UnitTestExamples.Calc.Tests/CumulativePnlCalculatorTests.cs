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
    public class CumulativePnlCalculatorTests
    {
        [TestMethod]
        public async Task GetCumulativePnl_NonExistentRegion_ReturnsError()
        {
            IList<ProfitAndLoss> emptyRepositoryResults = new List<ProfitAndLoss>();
            var mockRepository = new Mock<IQueryRepository>();
            mockRepository.Setup(x => x.GetProfitAndLossForRegion(It.IsAny<string>(), null)).Returns(Task.FromResult(emptyRepositoryResults));

            var calculator = new CumulativePnlCalculator(mockRepository.Object);

            var result = await calculator.GetCumulativePnl("NonExistentRegion");

            Assert.IsNull(result.Results);
            Assert.IsTrue(result.ErrorMessage == "No profit & loss data available for region NonExistentRegion");
        }

        [TestMethod]
        public async Task GetCumulativePnl_RegionWithTwoStrategies_ReturnsCorrectResults()
        {
            var strategy1 = new Strategy { Name = "TestStrategy1" };
            var strategy2 = new Strategy { Name = "TestStrategy2" };

            IList<ProfitAndLoss> repositoryResults = new List<ProfitAndLoss>
            {
                new ProfitAndLoss { Date = new DateTime(2016, 1, 5), Value = 100, Strategy = strategy1 },
                new ProfitAndLoss { Date = new DateTime(2016, 2, 21), Value = 200, Strategy = strategy1 },
                new ProfitAndLoss { Date = new DateTime(2016, 2, 22), Value = -50, Strategy = strategy1 },
                new ProfitAndLoss { Date = new DateTime(2016, 4, 7), Value = 300, Strategy = strategy1 },
                new ProfitAndLoss { Date = new DateTime(2016, 1, 16), Value = 200, Strategy = strategy2 },
                new ProfitAndLoss { Date = new DateTime(2016, 3, 6), Value = -20, Strategy = strategy2 },
                new ProfitAndLoss { Date = new DateTime(2016, 4, 7), Value = 100, Strategy = strategy2 },
                new ProfitAndLoss { Date = new DateTime(2016, 4, 29), Value = -90, Strategy = strategy2 }
            }.OrderBy(x => x.Date).ToList();

            var mockRepository = new Mock<IQueryRepository>();
            mockRepository.Setup(x => x.GetProfitAndLossForRegion(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(repositoryResults));

            var calculator = new CumulativePnlCalculator(mockRepository.Object);

            var result = await calculator.GetCumulativePnl("TestRegion", new DateTime(2016, 1, 3));

            Assert.IsNull(result.ErrorMessage);
            Assert.IsNotNull(result.Results);
            var results = result.Results.ToList();
            Assert.IsTrue(results.Select(x => x.Date).ToList().IsDailyTimeSeries());
            Assert.IsTrue(results.First().Date == new DateTime(2016, 1, 3));
            Assert.IsTrue(results.Last().Date == new DateTime(2016, 4, 29));
            Assert.IsTrue(results.FirstOrDefault(x => x.Date == new DateTime(2016, 2, 21)).CumulativePnl == 500);
            Assert.IsTrue(results.FirstOrDefault(x => x.Date == new DateTime(2016, 2, 22)).CumulativePnl == 450);
            Assert.IsTrue(results.FirstOrDefault(x => x.Date == new DateTime(2016, 2, 23)).CumulativePnl == 450);
            Assert.IsTrue(results.FirstOrDefault(x => x.Date == new DateTime(2016, 4, 29)).CumulativePnl == 740);
        }
    }
}
