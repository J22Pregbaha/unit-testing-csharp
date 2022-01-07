using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class MathTests
    {
        private Math _math;

        [SetUp]
        public void Setup()
        {
            _math = new Math();
        }
        
        [Test]
        public void Add_WhenCalled_ReturnsSumOfArguments()
        {
            // Act
            var result = _math.Add(1, 2);

            // Assert
            Assert.That(result == 3);
        }

        [Test]
        public void Max_FirstArgumentIsGreater_ReturnFirstArgument()
        {
            var result = _math.Max(2, 1);
            
            Assert.That(result == 2);
        }

        [Test]
        public void Max_SecondArgumentIsGreater_ReturnSecondArgument()
        {
            var result = _math.Max(1, 2);
            
            Assert.That(result == 2);
        }

        [Test]
        public void Max_ArgumentsAreEqual_ReturnSameArgument()
        {
            var result = _math.Max(2, 2);
            
            Assert.That(result == 2);
        }
    }
}