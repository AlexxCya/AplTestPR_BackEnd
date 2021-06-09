using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PR.Data;
using PR.Service.Repository;


namespace PR.Service
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        private Dictionary<Type, object> _repository = new Dictionary<Type, object>();
        protected ModelContext _context;
        public Service(ModelContext context)
        {
            if (_context == null)
            {
                _context = context;
            }
        }

        public IRepository<TEntity> GetRepository()
        {

            if (_repository.Keys.Contains(typeof(TEntity)) == true)
            {
                return _repository[typeof(TEntity)] as IRepository<TEntity>;
            }
            IRepository<TEntity> respositorio = new Repository<TEntity>(_context);
            _repository.Add(typeof(TEntity), respositorio);
            return respositorio;
        }

        public IRepository<TEntity> GetDAORepository()
        {
            throw new NotImplementedException();
        }
    }
}
