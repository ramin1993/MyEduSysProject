using EduSys.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduSys.Repository.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T>where T :class
    {
        protected readonly AppDbContext _context;

        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entites)
        {
            await _dbSet.AddRangeAsync(entites);
        }
        public async Task<bool> AnyAsync(Expression<Func<T,bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }
        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable(); 
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            //Manual olaraq bu sekilde statin deyise bilerik.
            //Qeyd : Delete etdiyimiz zaman data Db-de derhal delete olunmur ,ilk once C# terefde
            //onun stati deyisir daha sonra Entityden gelen SaveChanges methodu vasitesi ile Db-de Delete olur.
            // _context.Entry(entity).State = EntityState.Modified;
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);

        }
        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
