using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class ReservationTests
    {
        [Test]
        public void CanBeCancelledBy_UserIsAdmin_ReturnTrue()
        {
            // Arrange
            var reservation = new Reservation();
            var user = new User { IsAdmin = true };

            // Act
            var result = reservation.CanBeCancelledBy(user);

            // Assert
            Assert.That(result == true);
        }

        [Test]
        public void CanBeCancelledBy_UserIsSameAsUserWhoMadeReservation_ReturnTrue()
        {
            // Arrange
            var user = new User();
            var reservation = new Reservation { MadeBy = user };

            // Act
            var result = reservation.CanBeCancelledBy(user);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanBeCancelledBy_UserIsNotAdminOrSameAsUserWhoMadeReservation_ReturnFalse()
        {
            // Arrange
            var user = new User { IsAdmin = true };
            var reservation = new Reservation { MadeBy = user };
            var unauthorized_user = new User();

            // Act
            var result = reservation.CanBeCancelledBy(unauthorized_user);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
