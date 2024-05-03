using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;


namespace Accounting.DataLayer.Services
{
    public class GenericRepository<TEntity> where  TEntity: class
    {
        private Accounting_DBEntities _db;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(Accounting_DBEntities db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where=null)
        {
            IQueryable<TEntity> query = _dbSet;
            
            if (where != null)
            {
                query = query.Where(where);
            }
            return query.ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            if (_db.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = GetById(id);
            Delete(entity);
        }

        public IEnumerable<TransactionsNames> GetTransactionsWithName()
        {
            var Items = new List<TransactionsNames>();

            using (var db = new UnitOfWork())
            {
                var result = db.TransactionRepository.Get();
                foreach (var item in result)
                {
                    var Counterparty = db.CustomerRepository.GetCustomerIdNames()
                                                            .Where(n => n.Id == item.CustomerID)
                                                            .Select(n => n.Name)
                                                            .First();

                    var TransactionWithName = new TransactionsNames
                    {
                        ID = item.ID,
                        CustomerID = item.CustomerID,
                        Name = Counterparty,
                        TrType = item.TrType,
                        Amount = item.Amount,
                        Description = item.Description,
                        Date = item.Date,
                    };

                    Items.Add(TransactionWithName);
                }

                return Items;
            }
        }
    }

    public class TransactionsNames
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public int TrType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
