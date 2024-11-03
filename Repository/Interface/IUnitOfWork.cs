using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Person> Persons { get; }
        IGenericRepository<PersonVirus> PersonViruses { get; }
        IGenericRepository<ViroCureUser> ViroCureUsers { get; }
        IGenericRepository<Virus> Viruses { get; }

        IGenericRepository<T> GetRepository<T>() where T : class;
        Task SaveAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
