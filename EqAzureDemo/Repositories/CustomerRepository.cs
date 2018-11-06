using System;
using System.Collections.Generic;

using Korzh.WindowsAzure.Storage;

using EqAzureDemo.Models;
using Korzh.EasyQuery.Linq;

namespace EqAzureDemo.Repositories
{
    public class CustomerRepository : IRepository<Customer> {

        private readonly AzureTable<Customer> _table;

        public CustomerRepository(AzureStorageContext azureStorageContext) {
            _table = new AzureTable<Customer>(azureStorageContext, "Customers");
        }

        public void Add(Customer entity) {
            var result = _table.InsertEntityAsync(entity).Result;
        }

        public void Update(Customer entity) {
            var result = _table.InsertOrUpdateEntityAsync(entity).Result;
        }

        public void Delete(Customer entity) {
            _table.DeleteEntityAsync(entity).Wait();
        }

        public Customer Get(string id) {
            return _table.GetEntityByKeysAsync("Customer", id).Result;
        }

        public IEnumerable<Customer> Filter(string filterString) {
            return _table.GetEntitiesByFilterAsync(filterString).Result;
        }

        public IEnumerable<Customer> GetAll() {
            return _table.GetEntitiesByPartitionKeyAsync("Customer").Result;
        }

    }
}
