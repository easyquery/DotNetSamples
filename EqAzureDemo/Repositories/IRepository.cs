using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EqAzureDemo.Repositories
{
    public interface IRepository<T> where T: class, new() {

        void Add(T entity);

        void Delete(T entity);

        void Update(T entity);

        T Get(string id);

        IEnumerable<T> Filter(string filterString);

        IEnumerable<T> GetAll();
    }
}
