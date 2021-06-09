using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PR.Data;

namespace PR.Service.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ModelContext _context;
        private DbSet<TEntity> _dbSet;
        public Repository(ModelContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Delete(int id)
        {
            try
            {
                TEntity entityToDelete = _dbSet.Find(id);
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                    _dbSet.Attach(entityToDelete);
                var result = _dbSet.Remove(entityToDelete);
                _context.SaveChanges();
            }
            catch (Exception exp)
            {

            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                TEntity entityToDelete = _dbSet.Find(id);
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                    _dbSet.Attach(entityToDelete);
                var result = _dbSet.Remove(entityToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception exp)
            {

            }
        }

        public object ExecuteSP(string spName, IEnumerable<SqlParameter> parameters)
        {
            SqlConnection conn = null;
            var _dt = new DataTable();
            try
            {
                using (conn = (SqlConnection)_context.Database.GetDbConnection())
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = spName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);

                        using (var reader = cmd.ExecuteReader())
                        {
                            _dt.Load(reader);
                            reader.Close();
                        }
                    }
                }
                return _dt;
            }
            catch (SqlException slqEx)
            {
                Console.WriteLine(DateTime.Now.ToString() + " " + slqEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //Este metodo se ejecuta en una segunda vez
        public object ExecuteSP(string spName, IEnumerable<SqlParameter> parameters, string dbConnection)
        {
            SqlConnection conn = null;
            var _dt = new DataTable();
            try
            {
                using (conn = new SqlConnection(dbConnection))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = spName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);

                        using (var reader = cmd.ExecuteReader())
                        {
                            _dt.Load(reader);
                            reader.Close();
                        }
                    }
                }
                return _dt;
            }
            catch (SqlException slqEx)
            {
                Console.WriteLine(DateTime.Now.ToString() + " " + slqEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public TEntity Insert(TEntity obj)
        {
            try
            {
                _dbSet.Add(obj);
                _context.SaveChanges();
                return obj;
            }
            catch (Exception exp)
            {
                return null;
            }
        }

        public async Task<TEntity> InsertAsync(TEntity obj)
        {
            try
            {
                _dbSet.Add(obj);
                await _context.SaveChangesAsync();
                return obj;
            }
            catch (Exception exp)
            {
                return null;
            }
        }

        public async Task<ICollection<TEntity>> ListAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception exp)
            {
                return null;
            }
        }

        public async Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate,
                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;

                if (include != null)
                {
                    query = include(query);
                }

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }
                else
                {
                    return await query.ToListAsync();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error while executing Query in SQL: " + exp.Message);
                return null;
            }
        }

        public TEntity Update(TEntity obj)
        {
            try
            {
                //_context.Entry(obj).State = EntityState.Modified;
                _context.SaveChanges();
                return obj;
            }
            catch (Exception exp)
            {
                return null;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity obj)
        {
            try
            {
                _context.Entry(obj).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return obj;
            }
            catch (Exception exp)
            {
                return null;
            }
        }
    }
}
