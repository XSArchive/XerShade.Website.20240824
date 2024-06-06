using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using XerShade.Website.Core.Framework.Data.Interfaces;

namespace XerShade.Website.Core.Framework.Data;

public class DbService<TDataType>(DataDbContext dbContext) : IDisposable, IAsyncDisposable, IDbService<TDataType> where TDataType : class
{
    protected readonly DataDbContext dbContext = dbContext;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.dbContext.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return this.dbContext.DisposeAsync();
    }

    public virtual bool Has(Expression<Func<TDataType, bool>> predicate) => this.dbContext.Set<TDataType>().Any(predicate);

    public virtual TDataType Read(Expression<Func<TDataType, bool>> predicate) => this.dbContext.Set<TDataType>().First(predicate);

    public virtual List<TDataType> ReadRange(Expression<Func<TDataType, bool>> predicate) => this.dbContext.Set<TDataType>().Where(predicate).ToList();

    public virtual IQueryable<TDataType> ReadAll() => this.dbContext.Set<TDataType>();

    public virtual void Write(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction)
    {
        TDataType entry = this.dbContext.Set<TDataType>().FirstOrDefault(predicate) ?? this.CreateNewEntity(writeAction);

        writeAction(entry);

        _ = this.dbContext.Set<TDataType>().Update(entry);
        _ = this.dbContext.SaveChanges();
    }

    public virtual void Delete(Expression<Func<TDataType, bool>> predicate)
    {
        List<TDataType> entries = [.. this.dbContext.Set<TDataType>().Where(predicate)];
        if (entries.Count != 0)
        {
            this.dbContext.Set<TDataType>().RemoveRange(entries);
            _ = this.dbContext.SaveChanges();
        }
    }

    public virtual async Task<bool> HasAsync(Expression<Func<TDataType, bool>> predicate) => await this.dbContext.Set<TDataType>().AnyAsync(predicate);

    public virtual async Task<TDataType> ReadAsync(Expression<Func<TDataType, bool>> predicate) => await this.dbContext.Set<TDataType>().FirstAsync(predicate);

    public virtual async Task<List<TDataType>> ReadRangeAsync(Expression<Func<TDataType, bool>> predicate) => await this.dbContext.Set<TDataType>().Where(predicate).ToListAsync();

    public virtual async Task<IQueryable<TDataType>> ReadAllAsync() => await Task.Run(this.dbContext.Set<TDataType>);

    public virtual async Task WriteAsync(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction)
    {
        TDataType entry = await this.dbContext.Set<TDataType>().FirstOrDefaultAsync(predicate) ?? await this.CreateNewEntityAsync(writeAction);

        writeAction(entry);

        _ = this.dbContext.Set<TDataType>().Update(entry);
        _ = await this.dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Expression<Func<TDataType, bool>> predicate)
    {
        List<TDataType> entries = await this.dbContext.Set<TDataType>().Where(predicate).ToListAsync();
        if (entries.Count != 0)
        {
            this.dbContext.Set<TDataType>().RemoveRange(entries);
            _ = await this.dbContext.SaveChangesAsync();
        }
    }

    private TDataType CreateNewEntity(Action<TDataType> writeAction)
    {
        TDataType newEntity = Activator.CreateInstance<TDataType>();

        writeAction(newEntity);

        _ = this.dbContext.Set<TDataType>().Add(newEntity);
        return newEntity;
    }

    private async Task<TDataType> CreateNewEntityAsync(Action<TDataType> writeAction)
    {
        TDataType newEntity = Activator.CreateInstance<TDataType>();

        writeAction(newEntity);

        _ = await this.dbContext.Set<TDataType>().AddAsync(newEntity);
        return newEntity;
    }
}
