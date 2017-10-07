using System;

namespace UnitTestExamples.Repository.Entities
{
    public class Capital
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Strategy Strategy { get; set; }
        public int StrategyId { get; set; }
        public double Value { get; set; }
    }
}
