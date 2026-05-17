using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Domain.Common;
using TemplateProject.Domain.Interfaces;

namespace TemplateProject.Infrastructure.Repositories.Generic
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelDbContext _databaseContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(HotelDbContext databaseContext)
        {
            _hotelDbContext = hotelDbContext;
            _dbSet = _hotelDbContext.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public List<T> UpdateRange(IEnumerable<T> entities)
        {
            var list = entities.ToList();
            _dbSet.UpdateRange(list);
            return list;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public async Task DeleteByIdAsync(int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                    _dbSet.Remove(entity);
            }
        }

        public void Inactive(T entity)
        {
            if (entity is AuditableEntity auditable)
                auditable.IsActive = false;
        }

        public void InactiveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is AuditableEntity auditable)
                    auditable.IsActive = false;

            }
        }

        public async Task InactiveByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is AuditableEntity auditable)
                auditable.IsActive = false;
        }

        public async Task InactiveByIdAsync(int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity is AuditableEntity auditable)
                    auditable.IsActive = false;
            }
        }

        public void DeleteLogical(T entity)
        {
            if (entity is AuditableEntity auditable)
                auditable.State = false;
        }

        public void DeleteLogicalRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is AuditableEntity auditable)
                    auditable.State = false;
            }
        }

        public async Task DeleteLogicalById(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is AuditableEntity auditable)
                auditable.State = false;
        }

        public async Task DeleteLogicalById(int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity is AuditableEntity auditable)
                    auditable.State = false;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _hotelDbContext.SaveChangesAsync();
        }
    }
}
