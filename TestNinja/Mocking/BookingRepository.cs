using System.Linq;

namespace TestNinja.Mocking
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetExistingBookings(Booking booking);
    }

    public class BookingRepository : IBookingRepository
    {
        public IQueryable<Booking> GetExistingBookings(Booking booking)
        {
            var unitOfWork = new UnitOfWork();
            var bookings =
                unitOfWork.Query<Booking>()
                    .Where(
                        b => b.Id != booking.Id && b.Status != "Cancelled");
            return bookings;
        }
    }
}