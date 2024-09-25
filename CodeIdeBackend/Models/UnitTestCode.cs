using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeIdeBackend.Models
{
    public class UnitTestCode
    {
        public const string TestCode = @"
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearningCSharpConcept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LearningCSharpConcept.Tests
{
    [TestClass()]
    public class FizzBuzzTests
    {
        [TestMethod()]
        public void FizzBuzz_GetOutput_Method_Returns_Fizz_When_Number_Is_Divisible_By_Only_3()
        {
            var expected = ""Fizz"";

            var actual = FizzBuzz.GetOutput(3);
            Assert.AreEqual(actual, expected);

            var actual1 = FizzBuzz.GetOutput(6);
            Assert.AreEqual(actual1, expected);

            var actual2 = FizzBuzz.GetOutput(9);
            Assert.AreEqual(actual2, expected);
        }

        [TestMethod()]
        public void FizzBuzz_GetOutput_Method_Returns_Buzz_When_Number_Is_Divisible_By_Only_5()
        {
            var expected = ""Buzz"";

            var actual = FizzBuzz.GetOutput(5);
            Assert.AreEqual(actual, expected);

            var actual1 = FizzBuzz.GetOutput(10);
            Assert.AreEqual(actual1, expected);

            var actual2 = FizzBuzz.GetOutput(20);
            Assert.AreEqual(actual2, expected);
        }

        [TestMethod()]
        public void FizzBuzz_GetOutput_Method_Returns_FizzBuzz_When_Number_Is_Divisible_By_3_And_5()
        {
            var expected = ""FizzBuzz"";

            var actual = FizzBuzz.GetOutput(15);
            Assert.AreEqual(actual, expected);

            var actual1 = FizzBuzz.GetOutput(30);
            Assert.AreEqual(actual1, expected);

            var actual2 = FizzBuzz.GetOutput(45);
            Assert.AreEqual(actual2, expected);
        }

        [TestMethod()]
        public void FizzBuzz_GetOutput_Method_Returns_Number_When_Number_Is_Not_Divisible_By_3_Or_5()
        {
            var expected = ""7"";
            var actual = FizzBuzz.GetOutput(7);
            Assert.AreEqual(actual, expected);

            var expected1 = ""17"";
            var actual2 = FizzBuzz.GetOutput(17);
            Assert.AreEqual(actual2, expected1);


            var expected2 = ""1"";
            var actual3 = FizzBuzz.GetOutput(1);
            Assert.AreEqual(actual3, expected2);
        }
    }
}
";
    }
}