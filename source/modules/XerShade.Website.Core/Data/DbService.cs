using System.Linq.Expressions;
using XerShade.Website.Core.Data.Interfaces;

namespace XerShade.Website.Core.Data;

public class DbService<DataType>(IDbRepository<DataType> repository) : IDbService<DataType> where DataType : class
{
    protected readonly IDbRepository<DataType> Repository = repository;

    public async Task<DataType> GetAsync(Expression<Func<DataType, bool>> predicate) => await this.Repository.GetAsync(predicate) ?? throw new NullReferenceException();
    public async Task<DataType> GetByIdAsync(int id) => await this.Repository.GetByIdAsync(id) ?? throw new NullReferenceException();
    public async Task<IEnumerable<DataType>> GetRangeAsync(Expression<Func<DataType, bool>> predicate) => await this.Repository.GetRangeAsync(predicate) ?? throw new NullReferenceException();
    public async Task<IEnumerable<DataType>> GetAllAsync() => await this.Repository.GetAllAsync();
    public async Task AddAsync(DataType entity) => await this.Repository.AddAsync(entity);
    public async Task AddAsync(IEnumerable<DataType> entities) => await this.Repository.AddAsync(entities);
    public async Task RemoveAsync(DataType entity) => await this.Repository.RemoveAsync(entity);
    public async Task RemoveAsync(int id) => await this.Repository.RemoveAsync(id);
    public async Task RemoveAsync(IEnumerable<DataType> entities) => await this.Repository.RemoveAsync(entities);
    public async Task RemoveAsync(Expression<Func<DataType, bool>> predicate) => await this.Repository.RemoveAsync(predicate);
    public async Task SaveChangesAsync() => await this.Repository.SaveChangesAsync();
}
