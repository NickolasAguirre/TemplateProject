using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateProject.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        List<T> UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task DeleteByIdAsync(int id);
        Task DeleteByIdAsync(int[] ids);
        void Inactive(T entity);
        void InactiveRange(IEnumerable<T> entities);
        Task InactiveByIdAsync(int id);
        Task InactiveByIdAsync(int[] ids);
        void DeleteLogical(T entity);
        void DeleteLogicalRange(IEnumerable<T> entities);
        Task DeleteLogicalById(int id);
        Task DeleteLogicalById(int[] ids);
        Task SaveChangesAsync();
    }

}
