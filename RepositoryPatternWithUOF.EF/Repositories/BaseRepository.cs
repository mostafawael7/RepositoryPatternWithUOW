using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOF.Core.Consts;
using RepositoryPatternWithUOF.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOF.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public T GetById(int id) => _context.Set<T>().Find(id);
        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public IEnumerable<T> GetAll() =>  _context.Set<T>().ToList();

        public T Find(Expression<Func<T, bool>> match, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            return query.SingleOrDefault(match);

        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> match, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            return query.Where(match).ToList();

        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> match, int take, int skip)
        {
            return _context.Set<T>().Where(match).Skip(skip).Take(take).ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> match, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(match);
            if (take.HasValue)
                query = query.Take(take.Value);


            if (skip.HasValue)
                query = query.Take(skip.Value);

            if(orderBy != null){
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            return query.ToList();
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return entity;
        }
        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();

            return entities;
        }
    }
}
