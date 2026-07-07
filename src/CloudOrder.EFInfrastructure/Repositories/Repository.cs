using CloudOrder.Business.Repositories;
using CloudOrder.EFInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace CloudOrder.EFInfrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly CloudOrderDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(CloudOrderDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }
    public async virtual Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async virtual Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        await Task.CompletedTask;
    }

    public async virtual Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async virtual Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id).ConfigureAwait(false);
    }

    public async virtual Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }


}
