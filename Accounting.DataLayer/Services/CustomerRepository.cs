using Accounting.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Services
{
    public class CustomerRepository : ICustomerRepository
    {

        private Accounting_DBEntities db;

        public CustomerRepository(Accounting_DBEntities context)
        {
            this.db = context;
        }

        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch { return false; }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                var customer = GetCustomerById(customerId);
                DeleteCustomer(customer);
                return true;
            }
            catch { return false; }
        }

        public List<Customers> GetAllCustomers()
        {
            return db.Customers.ToList();
        }

        public Customers GetCustomerById(int customerId)
        {
            return db.Customers.Find(customerId);
        }

        public List<CustomerIdName> GetCustomerIdNames()
        {
            return db.Customers.Select(c => new CustomerIdName
            {
                Id = c.CustomerID,
                Name = c.FullName
            }
            ).ToList();
        }

        public IEnumerable<Customers> GetCustomersByFilter(string filter)
        {
            return db.Customers.Where(c => c.FullName.Contains(filter) || c.Email.Contains(filter) || c.Mobile.Contains(filter)).ToList();
        }

        public bool InsertCustomer(Customers customer)
        {
            try
            {
                db.Customers.Add(customer);
                return true;
            }
            catch { return false; }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public bool UpdateCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Modified;
                return true;
            }
            catch { return false; }
        }
    }

    public class CustomerIdName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
