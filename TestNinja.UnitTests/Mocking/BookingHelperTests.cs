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
        private Booking _existingBooking;
        private Mock<IBookingRepository> _repository;

        [SetUp]
        public void Setup()
        {
            _existingBooking = new Booking
            {
                Id = 2,
                ArrivalDate = ArriveOn(2021, 01, 13),
                DepartureDate = DepartOn(2021, 01, 15),
                Reference = "a"
            };
            
            _repository = new Mock<IBookingRepository>();
        }

        [Test]
        public void OverlappingBookingsExist_BookingStatusCancelled_ReturnsEmptyString()
        {
            var booking = new Booking 
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate),
                Status = "Cancelled"
            };
            
            var result = BookingHelper.OverlappingBookingsExist(booking, _repository.Object);
            
            Assert.That(result, Is.Empty);
        }
        
        [Test]
        public void OverlappingBookingsExist_WhenCalled_GetsUnitsOfWork()
        {
            var booking = new Booking();
            
            BookingHelper.OverlappingBookingsExist(booking, _repository.Object);
            
            _repository.Verify(r => r.GetExistingBookings(booking));
        }
        
        [Test]
        public void OverlappingBookingsExist_NoOverlappingBookingExists_ReturnEmptyString()
        {
            var newBooking = new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate, days: 2),
                DepartureDate = Before(_existingBooking.ArrivalDate)
            };
            
            _repository.Setup(r => r.GetExistingBookings(newBooking))
                .Returns(new List<Booking> {_existingBooking}.AsQueryable);

            var result = BookingHelper.OverlappingBookingsExist(newBooking, _repository.Object);
            
            Assert.That(result, Is.Empty);
        }
        
        [Test]
        public void OverlappingBookingsExist_NewBookingFinishesInTheMiddleOfExistingBooking_ReturnExistingBookingReference()
        {
            var newBooking = new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = new DateTime(_existingBooking.ArrivalDate.Year, 
                    _existingBooking.ArrivalDate.Month,
                    _existingBooking.ArrivalDate.Day, 
                    17, 0, 0)
            };
            
            _repository.Setup(r => r.GetExistingBookings(newBooking))
                .Returns(new List<Booking> {_existingBooking}.AsQueryable);

            var result = BookingHelper.OverlappingBookingsExist(newBooking, _repository.Object);
            
            Assert.That(result == _existingBooking.Reference);
        }
        
        [Test]
        public void OverlappingBookingsExist_NewBookingStartsBeforeAndFinishesAfterExistingBooking_ReturnExistingBookingReference()
        {
            var newBooking = new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate)
            };
            
            _repository.Setup(r => r.GetExistingBookings(newBooking))
                .Returns(new List<Booking> {_existingBooking}.AsQueryable);

            var result = BookingHelper.OverlappingBookingsExist(newBooking, _repository.Object);
            
            Assert.That(result == _existingBooking.Reference);
        }
        
        [Test]
        public void OverlappingBookingsExist_NewBookingStartsBeforeAndFinishesInTheMiddleOfExistingBooking_ReturnExistingBookingReference()
        {
            var newBooking = new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = Before(_existingBooking.DepartureDate)
            };
            
            _repository.Setup(r => r.GetExistingBookings(newBooking))
                .Returns(new List<Booking> {_existingBooking}.AsQueryable);

            var result = BookingHelper.OverlappingBookingsExist(newBooking, _repository.Object);
            
            Assert.That(result == _existingBooking.Reference);
        }
        
        [Test]
        public void OverlappingBookingsExist_NewBookingStartsInTheMiddleOfExistingBookingButFinishesAfter_ReturnExistingBookingReference()
        {
            var newBooking = new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate)
            };
            
            _repository.Setup(r => r.GetExistingBookings(newBooking))
                .Returns(new List<Booking> {_existingBooking}.AsQueryable);

            var result = BookingHelper.OverlappingBookingsExist(newBooking, _repository.Object);
            
            Assert.That(result == _existingBooking.Reference);
        }
        
        [Test]
        public void OverlappingBookingsExist_NewBookingStartsAndFinishesAfterExistingBooking_ReturnExistingBookingReference()
        {
            var newBooking = new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.DepartureDate),
                DepartureDate = After(_existingBooking.DepartureDate, days: 2)
            };
            
            _repository.Setup(r => r.GetExistingBookings(newBooking))
                .Returns(new List<Booking> {_existingBooking}.AsQueryable);

            var result = BookingHelper.OverlappingBookingsExist(newBooking, _repository.Object);
            
            Assert.That(result, Is.Empty);
        }

        private DateTime Before(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(-days);
        }
        
        private DateTime After(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(days);
        }

        private DateTime ArriveOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}