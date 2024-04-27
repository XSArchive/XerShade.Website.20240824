using System.Linq.Expressions;

namespace XerShade.Website.Core.Services.Interfaces;

public interface IRWDService<TDataType> where TDataType : class
{
    TDataType? Read(Expression<Func<TDataType, bool>> predicate);
    IQueryable<TDataType>? ReadAll();
    void Write(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction);
    void Delete(Expression<Func<TDataType, bool>> predicate);

    Task<TDataType?> ReadAsync(Expression<Func<TDataType, bool>> predicate);
    Task<IQueryable<TDataType>?> ReadAllAsync();
    Task WriteAsync(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction);
    Task DeleteAsync(Expression<Func<TDataType, bool>> predicate);
}
