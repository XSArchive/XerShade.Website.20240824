﻿using System.Linq.Expressions;

namespace XerShade.Website.Core.Data.Interfaces;
public interface IDbRepository<DataType> where DataType : class
{
    Task AddAsync(DataType entity);
    Task AddAsync(IEnumerable<DataType> entities);
    Task<IEnumerable<DataType>> GetAllAsync();
    Task<DataType> GetAsync(Expression<Func<DataType, bool>> predicate);
    Task<DataType> GetByIdAsync(int id);
    Task<IEnumerable<DataType>> GetRangeAsync(Expression<Func<DataType, bool>> predicate);
    Task RemoveAsync(DataType entity);
    Task RemoveAsync(Expression<Func<DataType, bool>> predicate);
    Task RemoveAsync(IEnumerable<DataType> entities);
    Task RemoveAsync(int id);
    Task SaveChangesAsync();
}