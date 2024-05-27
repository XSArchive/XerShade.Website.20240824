using System.Linq.Expressions;
using XerShade.Website.Core.Data.Interfaces;

namespace XerShade.Website.Core.Data;

public class DbService<DataType>(IDbRepository<DataType> repository) : IDbService<DataType> where DataType : class
{
    protected readonly IDbRepository<DataType> Repository = repository;

    public async Task<DataType> GetAsync(Expression<Func<DataType, bool>> predicate) => await this.Repository.GetAsync(predicate);
    public async Task<IEnumerable<DataType>> GetRangeAsync(Expression<Func<DataType, bool>> predicate) => await this.Repository.GetRangeAsync(predicate);
    public async Task<IEnumerable<DataType>> GetAllAsync() => await this.Repository.GetAllAsync();
    public async Task<DataType> GetByIdAsync(int id) => await this.Repository.GetByIdAsync(id);
    public async Task AddAsync(DataType entity) => await this.Repository.AddAsync(entity);
    public async Task AddRangeAsync(IEnumerable<DataType> entities) => await this.Repository.AddRangeAsync(entities);
    public async Task RemoveAsync(DataType entity) => await this.Repository.RemoveAsync(entity);
    public async Task RemoveRangeAsync(IEnumerable<DataType> entities) => await this.Repository.RemoveRangeAsync(entities);
    public async Task RemoveRangeAsync(Expression<Func<DataType, bool>> predicate) => await this.Repository.RemoveRangeAsync(predicate);
    public async Task RemoveByIdAsync(int id) => await this.Repository.RemoveByIdAsync(id);
    public async Task UpdateAsync(DataType entity) => await this.Repository.UpdateAsync(entity);
    public async Task UpdateRangeAsync(IEnumerable<DataType> entities) => await this.Repository.UpdateRangeAsync(entities);
    public async Task UpdateByIdAsync(int id, DataType updatedEntity) => await this.Repository.UpdateByIdAsync(id, updatedEntity);
    public async Task SaveChangesAsync() => await this.Repository.SaveChangesAsync();
}
