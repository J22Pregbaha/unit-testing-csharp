using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class FizzBuzzTests
    {
        [Test]
        public void GetOutput_NumberIsDivisibleByThreeAndFive_ReturnFizzBuzz()
        {
            var result = FizzBuzz.GetOutput(15);
            
            Assert.That(result == "FizzBuzz");
        }
        
        [Test]
        public void GetOutput_NumberIsDivisibleByOnlyThree_ReturnFizz()
        {
            var result = FizzBuzz.GetOutput(3);
            
            Assert.That(result == "Fizz");
        }
        
        [Test]
        public void GetOutput_NumberIsDivisibleByOnlyFive_ReturnBuzz()
        {
            var result = FizzBuzz.GetOutput(5);
            
            Assert.That(result == "Buzz");
        }
        
        [Test]
        public void GetOutput_NumberIsNotDivisibleByThreeOrFive_ReturnNumber()
        {
            var result = FizzBuzz.GetOutput(7);
            
            Assert.That(result == "7");
        }
    }
}