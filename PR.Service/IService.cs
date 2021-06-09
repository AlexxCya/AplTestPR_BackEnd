using PR.Service.Repository;

namespace PR.Service
{
    public interface IService<TEntity> where TEntity : class
    {

        IRepository<TEntity> GetRepository();

    }
}
