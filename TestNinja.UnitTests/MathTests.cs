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
        [TestCase(2, 1, 2)]
        [TestCase(1, 2, 2)]
        [TestCase(2, 2, 2)]
        public void Max_WhenCalled_ReturnGreaterArgument(int a, int b, int expectedResult)
        {
            var result = _math.Max(a, b);
            
            Assert.That(result == expectedResult);
        }
    }
}