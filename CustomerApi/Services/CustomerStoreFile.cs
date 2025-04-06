using CustomerApi.Models;
using System.Text.Json;

namespace CustomerApi.Services
{
    public class CustomerStoreFile : ICustomerStore
    {
        private readonly List<Customer> _customers = new();
        private readonly string _filePath = "customers.json";
        private readonly object _lock = new();

        public CustomerStoreFile()
        {
            LoadFile();
        }

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
                SaveFile();
            }
        }

        private void InsertSorted(Customer newCustomer)
        {
            int left = 0;
            int right = _customers.Count - 1;
            int insertIndex = _customers.Count; // Default to appending

            // Use binary search to find the insertion position
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                var comparison = CompareCustomers(newCustomer, _customers[mid]);

                if (comparison < 0)
                {
                    insertIndex = mid;
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            _customers.Insert(insertIndex, newCustomer);
        }

        private static int CompareCustomers(Customer a, Customer b)
        {
            int last = string.Compare(a.LastName, b.LastName, StringComparison.Ordinal);
            if (last != 0) return last;
            return string.Compare(a.FirstName, b.FirstName, StringComparison.Ordinal);
        }

        private void LoadFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                var list = JsonSerializer.Deserialize<List<Customer>>(json);
                if (list != null) _customers.AddRange(list);
            }
        }

        private void SaveFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(_customers, options);
            File.WriteAllText(_filePath, json);
        }
    }
}