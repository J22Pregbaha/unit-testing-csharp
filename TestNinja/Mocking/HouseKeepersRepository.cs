using System.Linq;

namespace TestNinja.Mocking
{
    public interface IHouseKeepersRepository
    {
        IQueryable<Housekeeper> GetHouseKeepers();
    }

    public class HouseKeepersRepository : IHouseKeepersRepository
    {
        public IQueryable<Housekeeper> GetHouseKeepers()
        {
            var housekeepers = new UnitOfWork().Query<Housekeeper>();

            return housekeepers;
        }
    }
}