using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using XerShade.Website.Core.Data;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Services;

public class RWDService<TDataType> : IRWDService<TDataType> where TDataType : class
{
    protected readonly GeneralDbContext dbContext;

    protected RWDService() => this.dbContext = new GeneralDbContext();

    public virtual TDataType? Read(Expression<Func<TDataType, bool>> predicate) => this.dbContext.Set<TDataType>().FirstOrDefault(predicate);
    public virtual List<TDataType>? ReadRange(Expression<Func<TDataType, bool>> predicate) => [.. this.dbContext.Set<TDataType>().Where(predicate)];
    public virtual IQueryable<TDataType>? ReadAll() => this.dbContext.Set<TDataType>();
    public virtual void Write(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction)
    {
        TDataType? entry = this.dbContext.Set<TDataType>().FirstOrDefault(predicate);

        if (entry == null)
        {
            TDataType? newEntity = Activator.CreateInstance(typeof(TDataType)) as TDataType ?? throw new NullReferenceException(nameof(newEntity));

            writeAction(newEntity);

            _ = this.dbContext.Set<TDataType>().Add(newEntity);
        }
        else
        {
            writeAction(entry);
        }

        _ = this.dbContext.SaveChanges();
    }
    public virtual void Delete(Expression<Func<TDataType, bool>> predicate)
    {
        List<TDataType>? entries = [.. this.dbContext.Set<TDataType>().Where(predicate)];

        if (entries is null || entries?.Count != 0)
        { return; }

        this.dbContext.Set<TDataType>().RemoveRange(entries);

        _ = this.dbContext.SaveChanges();
    }

    public virtual async Task<TDataType?> ReadAsync(Expression<Func<TDataType, bool>> predicate) => await this.dbContext.Set<TDataType>().FirstOrDefaultAsync(predicate);
    public virtual async Task<List<TDataType>?> ReadRangeAsync(Expression<Func<TDataType, bool>> predicate) => await this.dbContext.Set<TDataType>().Where(predicate).ToListAsync();
    public virtual async Task<IQueryable<TDataType>?> ReadAllAsync() => await Task.FromResult(this.dbContext.Set<TDataType>());
    public virtual async Task WriteAsync(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction)
    {
        TDataType? entry = await this.dbContext.Set<TDataType>().FirstOrDefaultAsync(predicate);

        if(entry == null)
        {
            TDataType? newEntity = Activator.CreateInstance(typeof(TDataType)) as TDataType ?? throw new NullReferenceException(nameof(newEntity));
            
            writeAction(newEntity);

            _ = await this.dbContext.Set<TDataType>().AddAsync(newEntity);
        }
        else
        {
            writeAction(entry);
        } 

        _ = await this.dbContext.SaveChangesAsync();
    }
    public virtual async Task DeleteAsync(Expression<Func<TDataType, bool>> predicate)
    {
        List<TDataType>? entries = await this.dbContext.Set<TDataType>().Where(predicate).ToListAsync();

        if (entries is null || entries?.Count != 0)
        { return; }

        this.dbContext.Set<TDataType>().RemoveRange(entries);

        _ = await this.dbContext.SaveChangesAsync();
    }
}
