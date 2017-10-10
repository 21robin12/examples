using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnitTestExamples.Repository.Entities;
using Moq;
using UnitTestExamples.Calc.Tests.Extensions;
using UnitTestExamples.Repository;

namespace UnitTestExamples.Calc.Tests
{
    [TestClass]
    public class MonthlyCapitalsCalculatorTests
    {
        [TestMethod]
        public async Task GetMonthlyCapitals_NonExistentStrategy_ReturnsError()
        {
            IList<Capital> emptyRepositoryResults = new List<Capital>();
            var mockRepository = new Mock<IQueryRepository>();
            mockRepository.Setup(x => x.GetCapitalsForStrategies(It.IsAny<IList<string>>())).Returns(Task.FromResult(emptyRepositoryResults));

            var calculator = new MonthlyCapitalsCalculator(mockRepository.Object);

            var result = await calculator.GetMonthlyCapitals(new List<string> { "NonExistentStrategy" });

            Assert.IsNull(result.Results);
            Assert.IsTrue(result.ErrorMessage == "No capitals data available for strategies [NonExistentStrategy]");
        }

        [TestMethod]
        public async Task GetMonthlyCapitals_OneStrategy_ReturnsCorrectResults()
        {
            var strategy1 = new Strategy { Name = "TestStrategy1" };

            IList<Capital> repositoryResults = new List<Capital>
            {
                new Capital { Date = new DateTime(2016, 1, 1), Value = 10000, Strategy = strategy1 },
                new Capital { Date = new DateTime(2016, 2, 1), Value = 20000, Strategy = strategy1 },
                new Capital { Date = new DateTime(2016, 3, 1), Value = 19000, Strategy = strategy1 },
                new Capital { Date = new DateTime(2016, 4, 1), Value = 28000, Strategy = strategy1 },
                new Capital { Date = new DateTime(2016, 5, 1), Value = 40000, Strategy = strategy1 },
            };

            var mockRepository = new Mock<IQueryRepository>();
            mockRepository.Setup(x => x.GetCapitalsForStrategies(It.IsAny<IList<string>>())).Returns(Task.FromResult(repositoryResults));

            var calculator = new MonthlyCapitalsCalculator(mockRepository.Object);

            var result = await calculator.GetMonthlyCapitals(new List<string> { "TestStrategy1" });

            Assert.IsNull(result.ErrorMessage);
            Assert.IsNotNull(result.Results);
            var results = result.Results.ToList();
            Assert.IsTrue(results.Select(x => x.Date).ToList().IsMonthlyTimeSeries());
            Assert.IsTrue(results.First().Date == new DateTime(2016, 1, 1));
            Assert.IsTrue(results.Last().Date == new DateTime(2016, 5, 1));
        }
    }
}
