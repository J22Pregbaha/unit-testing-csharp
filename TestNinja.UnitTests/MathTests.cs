using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class MathTests
    {
        [Test]
        public void Add_WhenCalled_ReturnsSumOfArguments()
        {
            // Arrange
            var math = new Math();

            // Act
            var result = math.Add(1, 2);

            // Assert
            Assert.That(result == 3);
        }

        [Test]
        public void Max_FirstArgumentIsGreater_ReturnFirstArgument()
        {
            var math = new Math();

            var result = math.Max(2, 1);
            
            Assert.That(result == 2);
        }

        [Test]
        public void Max_SecondArgumentIsGreater_ReturnSecondArgument()
        {
            var math = new Math();

            var result = math.Max(1, 2);
            
            Assert.That(result == 2);
        }

        [Test]
        public void Max_ArgumentsAreEqual_ReturnSameArgument()
        {
            var math = new Math();

            var result = math.Max(2, 2);
            
            Assert.That(result == 2);
        }
    }
}