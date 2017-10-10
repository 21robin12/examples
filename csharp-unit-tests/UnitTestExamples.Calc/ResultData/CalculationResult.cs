using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestExamples.Calc
{
    public class CalculationResult<T>
    {
        public IEnumerable<T> Results { get; set; }
        public string ErrorMessage { get; set; }
    }
}
