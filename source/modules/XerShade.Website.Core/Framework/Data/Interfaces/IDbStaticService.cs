using System.Linq.Expressions;

namespace XerShade.Website.Core.Framework.Data.Interfaces;
public interface IDbStaticService<TDataType> where TDataType : class
{
    void Delete(Expression<Func<TDataType, bool>> predicate);
    Task DeleteAsync(Expression<Func<TDataType, bool>> predicate);
    bool Has(Expression<Func<TDataType, bool>> predicate);
    Task<bool> HasAsync(Expression<Func<TDataType, bool>> predicate);
    TDataType Read(Expression<Func<TDataType, bool>> predicate);
    IQueryable<TDataType> ReadAll();
    Task<IQueryable<TDataType>> ReadAllAsync();
    Task<TDataType> ReadAsync(Expression<Func<TDataType, bool>> predicate);
    List<TDataType> ReadRange(Expression<Func<TDataType, bool>> predicate);
    Task<List<TDataType>> ReadRangeAsync(Expression<Func<TDataType, bool>> predicate);
    void Write(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction);
    Task WriteAsync(Expression<Func<TDataType, bool>> predicate, Action<TDataType> writeAction);
}