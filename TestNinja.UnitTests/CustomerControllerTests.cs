using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class CustomerControllerTests
    {
        [Test]
        public void GetCustomer_IdIsZero_ReturnNotFound()
        {
            var controller = new CustomerController();

            var result = controller.GetCustomer(0);
            
            // Check if it is a NotFound object
            Assert.That(result, Is.TypeOf<NotFound>());
            
            // Check if it is a NotFound object or one of its derivatives
            // Assert.That(result, Is.InstanceOf<NotFound>());
        }

        [Test]
        public void GetCustomer_IdIsZero_ReturnOk()
        {
            var controller = new CustomerController();

            var result = controller.GetCustomer(1);
            
            Assert.That(result, Is.TypeOf<Ok>());
        }
    }
}