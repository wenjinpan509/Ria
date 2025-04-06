using CustomerApi.Models;

namespace CustomerApi.Services
{
    public interface ICustomerStore
    {
        IEnumerable<Customer> GetAll();
        bool Exists(int id);
        void AddCustomers(IEnumerable<Customer> customers);
    }
}
