using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class DemeritPointsCalculatorTests
    {
        private DemeritPointsCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            _calculator = new DemeritPointsCalculator();
        }

        [Test]
        [TestCase(-1)]
        [TestCase(301)]
        public void CalculateDemeritPoints_InvalidSpeed_ThrowArgumentOutOfRangeException(int speed)
        {
            // Assert.Throws<ArgumentOutOfRangeException>(() => _calculator.CalculateDemeritPoints(speed));
            Assert.That(() => _calculator.CalculateDemeritPoints(speed), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(32, 0)]
        [TestCase(65, 0)]
        [TestCase(68, 0)]
        [TestCase(70, 1)]
        [TestCase(75, 2)]
        public void CalculateDemeritPoints_WhenCalled_ReturnDemeritPoints(int speed, int demerit_points)
        {
            var result = _calculator.CalculateDemeritPoints(speed);
            
            Assert.That(result == demerit_points);
        }
    }
}