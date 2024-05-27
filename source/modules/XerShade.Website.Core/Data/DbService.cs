using System.Linq.Expressions;
using XerShade.Website.Core.Data.Interfaces;

namespace XerShade.Website.Core.Data;

public class DbService<DataType>(IDbRepository<DataType> repository) : IDbService<DataType> where DataType : class
{
    protected readonly IDbRepository<DataType> Repository = repository;

    public virtual async Task<DataType> GetAsync(Expression<Func<DataType, bool>> predicate) => await this.Repository.GetAsync(predicate);
    public virtual async Task<IEnumerable<DataType>> GetRangeAsync(Expression<Func<DataType, bool>> predicate) => await this.Repository.GetRangeAsync(predicate);
    public virtual async Task<IEnumerable<DataType>> GetAllAsync() => await this.Repository.GetAllAsync();
    public virtual async Task<DataType> GetByIdAsync(int id) => await this.Repository.GetByIdAsync(id);
    public virtual async Task AddAsync(DataType entity) => await this.Repository.AddAsync(entity);
    public virtual async Task AddRangeAsync(IEnumerable<DataType> entities) => await this.Repository.AddRangeAsync(entities);
    public virtual async Task RemoveAsync(DataType entity) => await this.Repository.RemoveAsync(entity);
    public virtual async Task RemoveRangeAsync(IEnumerable<DataType> entities) => await this.Repository.RemoveRangeAsync(entities);
    public virtual async Task RemoveRangeAsync(Expression<Func<DataType, bool>> predicate) => await this.Repository.RemoveRangeAsync(predicate);
    public virtual async Task RemoveByIdAsync(int id) => await this.Repository.RemoveByIdAsync(id);
    public virtual async Task UpdateAsync(DataType entity) => await this.Repository.UpdateAsync(entity);
    public virtual async Task UpdateRangeAsync(IEnumerable<DataType> entities) => await this.Repository.UpdateRangeAsync(entities);
    public virtual async Task UpdateByIdAsync(int id, DataType updatedEntity) => await this.Repository.UpdateByIdAsync(id, updatedEntity);
    public virtual async Task SaveChangesAsync() => await this.Repository.SaveChangesAsync();
}
