using System.Linq;
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
        [Ignore("This test works. I'm just ignoring it because I can")]
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

        [Test]
        public void GetOddNumbers_LimitIsGreaterThanZero_ReturnOddNumbersUpToLimit()
        {
            var result = _math.GetOddNumbers(5);
            
            Assert.That(result, Is.Not.Empty); // Checks if the array is not empty. Too General
            
            Assert.That(result.Count(), Is.EqualTo(3)); // Specific
            
            /*Assert.That(result, Does.Contain(1));
            Assert.That(result, Does.Contain(3));
            Assert.That(result, Does.Contain(5));*/
            
            Assert.That(result, Is.EquivalentTo(new [] {1, 3, 5})); // This line is equal to the line above.
                                                                    // It doesn't check for the order of the array.
            // Check if the array is ordered 
            Assert.That(result, Is.Ordered);
            
            // Check if every element in the array is unique
            Assert.That(result, Is.Unique);
        }
    }
}