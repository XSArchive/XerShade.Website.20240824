using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using XerShade.Website.Core.Data.Interfaces;

namespace XerShade.Website.Core.Data;

public class DbStaticService<TDataType>(IDbContextFactory<DataDbContext> dbContextFactory) : IDisposable, IAsyncDisposable, IDbStaticService<TDataType> where TDataType : class
{
    protected readonly IDbContextFactory<DataDbContext> dbContextFactory = dbContextFactory;

    public void Dispose() => GC.SuppressFinalize(this);
    public async ValueTask DisposeAsync() => await Task.Run(() => GC.SuppressFinalize(this));

    public virtual bool Has(Expression<Func<TDataType, bool>> predicate)
    {
        DataDbContext dbContext = this.dbContextFactory.CreateDbContext();
        return dbContext.Set<TDataType>().Any(predicate);
    }

    public virtual TDataType Read(Expression<Func<TDataType, bool>> predicate)
    {
        DataDbContext dbContext = this.dbContextFactory.CreateDbContext();
        return dbContext.Set<TDataType>().First(predicate);
    }

    public virtual List<TDataType> ReadRange(Expression<Func<TDataType, bool>> predicate)
    {
        DataDbContext dbContext = this.dbContextFactory.CreateDbContext();
        return dbContext.Set<TDataType>().Where(predicate).ToList();
    }

    public virtual IQueryable<TDataType> ReadAll()
    {
        DataDbContext dbContext = this.dbContextFactory.CreateDbContext();
        return dbContext.Set<TDataType>();
    }

    public virtual void Write(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction)
    {
        DataDbContext dbContext = this.dbContextFactory.CreateDbContext();
        TDataType entry = dbContext.Set<TDataType>().FirstOrDefault(predicate) ?? this.CreateNewEntity(writeAction);

        writeAction(entry);

        _ = dbContext.Set<TDataType>().Update(entry);
        _ = dbContext.SaveChanges();
    }

    public virtual void Delete(Expression<Func<TDataType, bool>> predicate)
    {
        DataDbContext dbContext = this.dbContextFactory.CreateDbContext();
        List<TDataType> entries = [.. dbContext.Set<TDataType>().Where(predicate)];
        if (entries.Count != 0)
        {
            dbContext.Set<TDataType>().RemoveRange(entries);
            _ = dbContext.SaveChanges();
        }
    }

    private TDataType CreateNewEntity(Action<TDataType> writeAction)
    {
        DataDbContext dbContext = this.dbContextFactory.CreateDbContext();
        TDataType newEntity = Activator.CreateInstance<TDataType>();

        writeAction(newEntity);

        _ = dbContext.Set<TDataType>().Add(newEntity);
        return newEntity;
    }

    public virtual async Task<bool> HasAsync(Expression<Func<TDataType, bool>> predicate)
    {
        DataDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<TDataType>().AnyAsync(predicate);
    }

    public virtual async Task<TDataType> ReadAsync(Expression<Func<TDataType, bool>> predicate)
    {
        DataDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<TDataType>().FirstAsync(predicate);
    }

    public virtual async Task<List<TDataType>> ReadRangeAsync(Expression<Func<TDataType, bool>> predicate)
    {
        DataDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<TDataType>().Where(predicate).ToListAsync();
    }

    public virtual async Task<IQueryable<TDataType>> ReadAllAsync()
    {
        DataDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        return await Task.Run(dbContext.Set<TDataType>);
    }

    public virtual async Task WriteAsync(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction)
    {
        DataDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        TDataType entry = await dbContext.Set<TDataType>().FirstOrDefaultAsync(predicate) ?? await this.CreateNewEntityAsync(writeAction);

        writeAction(entry);

        _ = dbContext.Set<TDataType>().Update(entry);
        _ = await dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Expression<Func<TDataType, bool>> predicate)
    {
        DataDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        List<TDataType> entries = await dbContext.Set<TDataType>().Where(predicate).ToListAsync();
        if (entries.Count != 0)
        {
            dbContext.Set<TDataType>().RemoveRange(entries);
            _ = await dbContext.SaveChangesAsync();
        }
    }

    private async Task<TDataType> CreateNewEntityAsync(Action<TDataType> writeAction)
    {
        DataDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        TDataType newEntity = Activator.CreateInstance<TDataType>();

        writeAction(newEntity);

        _ = await dbContext.Set<TDataType>().AddAsync(newEntity);
        return newEntity;
    }
}
