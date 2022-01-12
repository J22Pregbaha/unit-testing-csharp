using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        private Mock<IEmployeeStorage> _employeeStorage;
        private EmployeeController _controller;

        [SetUp]
        public void Setup()
        {
            _employeeStorage = new Mock<IEmployeeStorage>();
            _controller = new EmployeeController(_employeeStorage.Object);
        }
        
        [Test]
        public void DeleteEmployee_WhenCalled_ReturnsRedirectResult()
        {
            var result = _controller.DeleteEmployee(1);
            
            Assert.That(result, Is.TypeOf<RedirectResult>());
        }
        
        [Test]
        public void DeleteEmployee_WhenCalled_RemovesEmployeeFromDb()
        {
            _controller.DeleteEmployee(1);
            
            _employeeStorage.Verify(s => s.RemoveEmployee(1));
        }
    }
}