using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Table;
using Korzh.WindowsAzure.Storage;

using EqAzureDemo.Models;
using Korzh.EasyQuery.Linq;

namespace EqAzureDemo.Repositories
{
    public class CustomerRepository : IRepository<Customer> {

        private readonly TableStorageService<Customer> _modelStorage;

        public CustomerRepository(AzureStorageContext azureStorageContext) {
            _modelStorage = new TableStorageService<Customer>(azureStorageContext, "Customers");
        }

        public void Add(Customer entity) {
            var result = _modelStorage.InsertEntityAsync(entity).Result;
        }

        public void Update(Customer entity) {
            var result = _modelStorage.InsertOrUpdateEntityAsync(entity).Result;
        }

        public void Delete(Customer entity) {
            _modelStorage.DeleteEntityAsync(entity).Wait();
        }

        public Customer Get(string id) {
            return _modelStorage.GetEntityByKeysAsync("Customer", id).Result;
        }

        public IEnumerable<Customer> Filter(string filterString) {
            return _modelStorage.GetEntitiesByFilterAsync(filterString).Result;
        }

        public IEnumerable<Customer> GetAll() {
            return _modelStorage.GetEntitiesByPartitionKeyAsync("Customer").Result;
        }

    }
}
