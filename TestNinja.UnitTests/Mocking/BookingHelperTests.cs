using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelperTests
    {
        [Test]
        public void OverlappingBookingsExist_BookingStatusCancelled_ReturnsEmptyString()
        {
            var repository = new Mock<IBookingRepository>();
            var booking = new Booking {Status = "Cancelled"};
            
            var result = BookingHelper.OverlappingBookingsExist(booking, repository.Object);
            
            Assert.That(result == "");
        }
        
        [Test]
        public void OverlappingBookingsExist_WhenCalled_GetsUnitsOfWork()
        {
            var repository = new Mock<IBookingRepository>();
            var booking = new Booking();
            
            var result = BookingHelper.OverlappingBookingsExist(booking, repository.Object);
            
            repository.Verify(r => r.GetUnit(booking));
        }
        
        [Test]
        public void OverlappingBookingsExist_NoOverlappingBookingExists_ReturnEmptyString()
        {
            var repository = new Mock<IBookingRepository>();
            var booking = new Booking
            {
                Id = 1,
                ArrivalDate = new DateTime(2021, 01, 10, 14, 0, 0),
                DepartureDate = new DateTime(2021, 01, 12, 14, 0, 0)
            };

            repository.Setup(r => r.GetUnit(booking))
                .Returns(new List<Booking>
                {
                    new Booking
                    {
                        Id = 2,
                        ArrivalDate = new DateTime(2021, 01, 13, 14, 0, 0),
                        DepartureDate = new DateTime(2021, 01, 15, 14, 0, 0)
                    }
                }.AsQueryable);
            
            var result = BookingHelper.OverlappingBookingsExist(booking, repository.Object);
            
            Assert.That(result == "");
        }
    }
}