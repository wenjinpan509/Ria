using CustomerApi.Models;

namespace CustomerApi.Services;

public class CustomerStoreMemory : ICustomerStore
{
    private readonly List<Customer> _customers = new();
    private readonly object _lock = new();

    public IEnumerable<Customer> GetAll()
    {
        lock (_lock) return _customers.ToList();
    }

    public bool Exists(int id)
    {
        lock (_lock) return _customers.Any(c => c.Id == id);
    }

    public void AddCustomers(IEnumerable<Customer> customers)
    {
        lock (_lock)
        {
            foreach (var customer in customers)
            {
                InsertSorted(customer);
            }
        }
    }

    private void InsertSorted(Customer newCustomer)
    {
        int left = 0, right = _customers.Count - 1, index = _customers.Count;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            var cmp = string.Compare(newCustomer.LastName, _customers[mid].LastName, StringComparison.Ordinal);
            if (cmp == 0)
                cmp = string.Compare(newCustomer.FirstName, _customers[mid].FirstName, StringComparison.Ordinal);

            if (cmp < 0)
            {
                index = mid;
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
        }

        _customers.Insert(index, newCustomer);
    }
}
