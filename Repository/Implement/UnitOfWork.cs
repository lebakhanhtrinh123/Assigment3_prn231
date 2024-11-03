using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ViroCureFal2024dbContext _context;
        private IGenericRepository<Person> _persons;
        private IGenericRepository<PersonVirus> _personViruses;
        private IGenericRepository<ViroCureUser> _viroCureUsers;
        private IGenericRepository<Virus> _viruses;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(ViroCureFal2024dbContext context)
        {
            _context = context;
        }
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }

            var repositoryInstance = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repositoryInstance);
            return repositoryInstance;
        }
        public IGenericRepository<Person> Persons => _persons ??= new GenericRepository<Person>(_context);
        public IGenericRepository<PersonVirus> PersonViruses => _personViruses ??= new GenericRepository<PersonVirus>(_context);
        public IGenericRepository<ViroCureUser> ViroCureUsers => _viroCureUsers ??= new GenericRepository<ViroCureUser>(_context);
        public IGenericRepository<Virus> Viruses => _viruses ??= new GenericRepository<Virus>(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollBack()
        {
            // Logic to roll back a transaction
        }

        public void Dispose()
        {
            _context.Database.RollbackTransaction();
        }
    }
}
