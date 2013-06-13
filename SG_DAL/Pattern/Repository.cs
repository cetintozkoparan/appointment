using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SG_DAL.Context;

namespace SG_DAL.Pattern
{
    public class Repository<T> where T : class
    {
        IObjectContextAdapter _context;
        IObjectSet<T> _objectSet;

        public Repository(SGContext context)
        {
            _context = context;
            _objectSet = _context.ObjectContext.CreateObjectSet<T>();
        }

        public IQueryable<T> AsQueryable()
        {
            return _objectSet;
        }

        public T First(Expression<Func<T, bool>> where)
        {

            return _objectSet.First(where);
        }

        public T First()
        {
            return _objectSet.First();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where);
        }

        public void Delete(T entity)
        {
            //_context.ObjectContext.AttachTo("SinavGorevli", entity);
            _objectSet.Attach(entity);
            _objectSet.DeleteObject(entity);
            _context.ObjectContext.SaveChanges();
        }


        public bool Add(T entity)
        {
            _objectSet.AddObject(entity);
            _context.ObjectContext.SaveChanges();
            return true;
        }

        public void Attach(T entity)
        {
            _objectSet.Attach(entity);
            _context.ObjectContext.SaveChanges();
        }

        public List<T> Listele()
        {
            List<T> liste = _objectSet.ToList();
            return liste;
        }

        public bool UpdateSaveChanges()
        {
            _context.ObjectContext.SaveChanges();
            return true;
        }
        // İçerisine aldığı order by sorgusuna göre sıralama yapar
        public List<T> Listele2<F>(Expression<Func<T, F>> where)
        {
            return _objectSet.OrderBy(where).ToList();
        }

        public List<T> Listele3<F>(Expression<Func<T, bool>> where, Expression<Func<T, F>> orderBy)
        {
            return _objectSet.Where(where).OrderBy(orderBy).ToList();
        }
    }
}
