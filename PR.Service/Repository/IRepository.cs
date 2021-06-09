using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PR.Service.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Insert(TEntity obj);
        Task<TEntity> InsertAsync(TEntity obj);
        TEntity Update(TEntity obj);
        Task<TEntity> UpdateAsync(TEntity obj);
        void Delete(int id);
        Task DeleteAsync(int id);
        Task<ICollection<TEntity>> ListAsync();
        //Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate,
        //                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        //                                    params Expression<Func<TEntity, object>>[] include);
        Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include);

        object ExecuteSP(string spName, IEnumerable<SqlParameter> parameters);

        //Este metodo se ejecuta en una segunda vez
        object ExecuteSP(string spName, IEnumerable<SqlParameter> parameters, string dbConnection);
    }
}
