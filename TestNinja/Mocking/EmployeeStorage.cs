namespace TestNinja.Mocking
{
    public interface IEmployeeStorage
    {
        void RemoveEmployee(int id);
    }

    public class EmployeeStorage : IEmployeeStorage
    {
        public void RemoveEmployee(int id)
        {
            var db = new EmployeeContext();
            var employee = db.Employees.Find(id);
            
            if (employee == null) return;
            
            db.Employees.Remove(employee);
            db.SaveChanges();
        }
    }
}