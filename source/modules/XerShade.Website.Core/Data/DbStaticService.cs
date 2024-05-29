using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using XerShade.Website.Core.Data.Interfaces;

namespace XerShade.Website.Core.Data;

public class DbStaticService<TDataType>(IDbContextFactory<GeneralDbContext> dbContextFactory) : IDisposable, IAsyncDisposable, IDbStaticService<TDataType> where TDataType : class
{
    protected readonly IDbContextFactory<GeneralDbContext> dbContextFactory = dbContextFactory;

    public void Dispose() => GC.SuppressFinalize(this);
    public async ValueTask DisposeAsync() => await Task.Run(() => GC.SuppressFinalize(this));

    public virtual bool Has(Expression<Func<TDataType, bool>> predicate) => this.dbContextFactory.CreateDbContext().Set<TDataType>().Any(predicate);

    public virtual TDataType Read(Expression<Func<TDataType, bool>> predicate) => this.dbContextFactory.CreateDbContext().Set<TDataType>().First(predicate);

    public virtual List<TDataType> ReadRange(Expression<Func<TDataType, bool>> predicate) => this.dbContextFactory.CreateDbContext().Set<TDataType>().Where(predicate).ToList();

    public virtual IQueryable<TDataType> ReadAll() => this.dbContextFactory.CreateDbContext().Set<TDataType>();

    public virtual void Write(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction)
    {
        TDataType entry = this.dbContextFactory.CreateDbContext().Set<TDataType>().FirstOrDefault(predicate) ?? this.CreateNewEntity(writeAction);

        writeAction(entry);

        _ = this.dbContextFactory.CreateDbContext().Set<TDataType>().Update(entry);
        _ = this.dbContextFactory.CreateDbContext().SaveChanges();
    }

    public virtual void Delete(Expression<Func<TDataType, bool>> predicate)
    {
        List<TDataType> entries = [.. this.dbContextFactory.CreateDbContext().Set<TDataType>().Where(predicate)];
        if (entries.Count != 0)
        {
            this.dbContextFactory.CreateDbContext().Set<TDataType>().RemoveRange(entries);
            _ = this.dbContextFactory.CreateDbContext().SaveChanges();
        }
    }

    public virtual async Task<bool> HasAsync(Expression<Func<TDataType, bool>> predicate)
    {
        GeneralDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<TDataType>().AnyAsync(predicate);
    }

    public virtual async Task<TDataType> ReadAsync(Expression<Func<TDataType, bool>> predicate)
    {
        GeneralDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<TDataType>().FirstAsync(predicate);
    }

    public virtual async Task<List<TDataType>> ReadRangeAsync(Expression<Func<TDataType, bool>> predicate)
    {
        GeneralDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<TDataType>().Where(predicate).ToListAsync();
    }

    public virtual async Task<IQueryable<TDataType>> ReadAllAsync()
    {
        GeneralDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        return await Task.Run(dbContext.Set<TDataType>);
    }

    public virtual async Task WriteAsync(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction)
    {
        GeneralDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        TDataType entry = await dbContext.Set<TDataType>().FirstOrDefaultAsync(predicate) ?? await this.CreateNewEntityAsync(writeAction);

        writeAction(entry);

        _ = dbContext.Set<TDataType>().Update(entry);
        _ = await dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Expression<Func<TDataType, bool>> predicate)
    {
        GeneralDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        List<TDataType> entries = await dbContext.Set<TDataType>().Where(predicate).ToListAsync();
        if (entries.Count != 0)
        {
            dbContext.Set<TDataType>().RemoveRange(entries);
            _ = await dbContext.SaveChangesAsync();
        }
    }

    private TDataType CreateNewEntity(Action<TDataType> writeAction)
    {
        TDataType newEntity = Activator.CreateInstance<TDataType>();

        writeAction(newEntity);

        _ = this.dbContextFactory.CreateDbContext().Set<TDataType>().Add(newEntity);
        return newEntity;
    }

    private async Task<TDataType> CreateNewEntityAsync(Action<TDataType> writeAction)
    {
        GeneralDbContext dbContext = await this.dbContextFactory.CreateDbContextAsync();
        TDataType newEntity = Activator.CreateInstance<TDataType>();

        writeAction(newEntity);

        _ = await dbContext.Set<TDataType>().AddAsync(newEntity);
        return newEntity;
    }
}
