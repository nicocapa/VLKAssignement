using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VLKAssignement.DataAccess.Repositories.Interfaces
{
    public interface IRepositoryBase<TModel> where TModel : class, new()
    {
        TModel Add(TModel item);
        TModel Update(TModel item);
        void Delete(Guid id);
        TModel GetById(Guid id);
        List<TModel> FindAll(Expression<Func<TModel, bool>> criteria = null);
    }    
}
