using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VLKAssignement.DataAccess.Repositories
{
    public abstract class RepositoryBase<TModel> where TModel : class, new()
    {
        protected readonly AppDbContext _context;        
        public RepositoryBase(AppDbContext context)
        {
            _context = context;            
        }

        public virtual TModel Add(TModel item)
        {
            var result = _context.Set<TModel>().Add(item);
            _context.SaveChanges();
            return result.Entity;
        }

        public virtual TModel Update(TModel item)
        {
            _context.Entry<TModel>(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

        public virtual void Delete(Guid id)
        {
            TModel itemToRemove = GetModelById(id);
            _context.Set<TModel>().Remove(itemToRemove);
            _context.SaveChanges();
        }

        public virtual TModel GetById(Guid id)
        {
            return GetModelById(id);
        }


        public virtual List<TModel> FindAll(Expression<Func<TModel, bool>> criteria = null)
        {
            IQueryable<TModel> items;
            if (criteria == null)
            {
                items = _context.Set<TModel>().AsQueryable();
            }
            else
            {
                items = _context.Set<TModel>().Where(criteria);
            }
            return items.ToList();
        }

        private TModel GetModelById(Guid id)
        {
            var item = _context.Set<TModel>().Find(id);
            return item;
        }
    }
}
