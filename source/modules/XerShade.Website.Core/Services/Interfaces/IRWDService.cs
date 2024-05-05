using System.Linq.Expressions;

namespace XerShade.Website.Core.Services.Interfaces;

public interface IRWDService<TDataType> where TDataType : class
{
    bool Has(Expression<Func<TDataType, bool>> predicate);
    TDataType? Read(Expression<Func<TDataType, bool>> predicate);
    List<TDataType>? ReadRange(Expression<Func<TDataType, bool>> predicate);
    IQueryable<TDataType>? ReadAll();
    void Write(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction);
    void Delete(Expression<Func<TDataType, bool>> predicate);

    Task<bool> HasAsync(Expression<Func<TDataType, bool>> predicate);
    Task<TDataType?> ReadAsync(Expression<Func<TDataType, bool>> predicate);
    Task<List<TDataType>?> ReadRangeAsync(Expression<Func<TDataType, bool>> predicate);
    Task<IQueryable<TDataType>?> ReadAllAsync();
    Task WriteAsync(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction);
    Task DeleteAsync(Expression<Func<TDataType, bool>> predicate);
}
