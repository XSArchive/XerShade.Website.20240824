using System.Linq.Expressions;

namespace XerShade.Website.Core.Data.Interfaces;

public interface IDbService<DataType> where DataType : class
{
    Task AddAsync(DataType entity);
    Task AddRangeAsync(IEnumerable<DataType> entities);
    Task<IEnumerable<DataType>> GetAllAsync();
    Task<DataType> GetAsync(Expression<Func<DataType, bool>> predicate);
    Task<DataType> GetByIdAsync(int id);
    Task<IEnumerable<DataType>> GetRangeAsync(Expression<Func<DataType, bool>> predicate);
    Task RemoveAsync(DataType entity);
    Task RemoveByIdAsync(int id);
    Task RemoveRangeAsync(Expression<Func<DataType, bool>> predicate);
    Task RemoveRangeAsync(IEnumerable<DataType> entities);
    Task SaveChangesAsync();
    Task UpdateAsync(DataType entity);
    Task UpdateByIdAsync(int id, DataType updatedEntity);
    Task UpdateRangeAsync(IEnumerable<DataType> entities);
}