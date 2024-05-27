using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using XerShade.Website.Core.Data.Interfaces;

namespace XerShade.Website.Core.Data;

public class DbRepository<DataType>(IDbContextFactory<GeneralDbContext> contextFactory) : IDbRepository<DataType> where DataType : class
{
    private readonly IDbContextFactory<GeneralDbContext> contextFactory = contextFactory;

    public async Task<DataType> GetAsync(Expression<Func<DataType, bool>> predicate)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        return await dbcontext.Set<DataType>().Where(predicate).FirstOrDefaultAsync() ?? throw new NullReferenceException();
    }

    public async Task<IEnumerable<DataType>> GetRangeAsync(Expression<Func<DataType, bool>> predicate)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        return await dbcontext.Set<DataType>().Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<DataType>> GetAllAsync()
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        return await dbcontext.Set<DataType>().ToListAsync();
    }

    public async Task<DataType> GetByIdAsync(int id)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        return await dbcontext.Set<DataType>().FindAsync(id) ?? throw new NullReferenceException();
    }

    public async Task AddAsync(DataType entity)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        _ = await dbcontext.Set<DataType>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<DataType> entities)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        await dbcontext.Set<DataType>().AddRangeAsync(entities);
    }

    public Task RemoveAsync(DataType entity)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        _ = dbcontext.Set<DataType>().Remove(entity);

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<DataType> entities)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        dbcontext.Set<DataType>().RemoveRange(entities);

        return Task.CompletedTask;
    }

    public async Task RemoveRangeAsync(Expression<Func<DataType, bool>> predicate)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        List<DataType> entities = await dbcontext.Set<DataType>().Where(predicate).ToListAsync();
        if (entities.Count != 0)
        {
            dbcontext.Set<DataType>().RemoveRange(entities);
        }
    }

    public async Task RemoveByIdAsync(int id)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        DataType? entity = await dbcontext.Set<DataType>().FindAsync(id);
        if (entity != null)
        {
            _ = dbcontext.Set<DataType>().Remove(entity);
        }
    }

    public Task UpdateAsync(DataType entity)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        _ = dbcontext.Set<DataType>().Update(entity);

        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<DataType> entities)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        dbcontext.Set<DataType>().UpdateRange(entities);

        return Task.CompletedTask;
    }

    public async Task UpdateByIdAsync(int id, DataType updatedEntity)
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();
        DataType? entity = await dbcontext.Set<DataType>().FindAsync(id) ?? throw new NullReferenceException();

        dbcontext.Entry(entity).CurrentValues.SetValues(updatedEntity);
        _ = dbcontext.Set<DataType>().Update(entity);
    }

    public async Task SaveChangesAsync()
    {
        using GeneralDbContext dbcontext = this.contextFactory.CreateDbContext();

        _ = await dbcontext.SaveChangesAsync();
    }
}
