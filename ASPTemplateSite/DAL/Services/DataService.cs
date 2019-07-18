using ASPTemplateSite.Models;
using ASPTemplateSite.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace ASPTemplateSite.DAL.Services
{
    public class DataService<T> : IGetData<T>, ISetData<T> where T : class, IIdentity
    {
        DataModel context = new DataModel();

        public List<T> getAll()
        {
            List<T> entities = new List<T>();
            foreach(var item in context.Set<T>())
            {
                entities.Add(item);
            }

            return entities;
        }

        public T getEntity(int id)
        {
            return context.Set<T>().Find(id);
        }

        public T getEntity(Expression<Func<T, bool>> request)
        {
            return context.Set<T>().Where(request).FirstOrDefault();
        }
  
        public void addEntity(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public void deleteEntity(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public void updateEntity(T entity)
        {
            var oldEntity = context.Set<T>().Find(entity.Id);
            context.Entry(oldEntity).CurrentValues.SetValues(entity);
            context.SaveChanges();
        }

        public void deleteAll()
        {
            var itemsToDelete = context.Set<T>();
            context.Set<T>().RemoveRange(itemsToDelete);
            context.SaveChanges();
        }
    }
}