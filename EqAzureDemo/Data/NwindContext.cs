using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korzh.WindowsAzure.Storage;

using EqAzureDemo.Repositories;

namespace EqAzureDemo.Data
{
    public class NwindContext {

        public CustomerRepository Customers { get; private set; }

        public NwindContext(AzureStorageContext azureStorageContext) {
            Customers = new CustomerRepository(azureStorageContext);
        }

    }
}
