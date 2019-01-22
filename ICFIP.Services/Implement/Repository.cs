using ICFIP.Entites;
using ICFIP.Services.Contrat;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ICFIP.Services.Implement
{
    public class Repository<T> : IRepository<T> where T : class
    {
        
        ICFIPEntities DbContext;
        public Repository(string StrConnection)
        {
            DbContext = new ICFIPEntities(StrConnection);
        }
        public List<string> GetKey<T>()
        {
            // List<string> ListKey;
            Type t = typeof(T);
            ObjectContext objectContext = ((IObjectContextAdapter)this.DbContext).ObjectContext;

            MethodInfo method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                                                     .MakeGenericMethod(t);
            dynamic objectSet = method.Invoke(objectContext, null);
            IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.KeyMembers;
            return (keyMembers.Select(k => (string)k.Name).ToList());


        }
        public int Add(T entity)
        {
            ((IObjectContextAdapter)this.DbContext).ObjectContext.AddObject(typeof(T).Name, entity);
            
            int result = this.DbContext.SaveChanges();
            ((IObjectContextAdapter)this.DbContext).ObjectContext.Refresh(RefreshMode.StoreWins, entity);

            this.DbContext.SaveChanges();
            ((IObjectContextAdapter)this.DbContext).ObjectContext.AcceptAllChanges();
            return result;
        }

        public IList<T> GetAll(Expression<Func<T, bool>> expression, bool bEnableLazyLoading)
        {
            this.DbContext.Configuration.LazyLoadingEnabled = bEnableLazyLoading;
            IList<T> list=(expression!=null)?
                ((IObjectContextAdapter)this.DbContext).ObjectContext.CreateQuery<T>(typeof(T).Name).Where(expression).ToList() :
                 ((IObjectContextAdapter)this.DbContext).ObjectContext.CreateQuery<T>(typeof(T).Name).ToList();

            return list;
         
        }


        public int Update(T entity)
        {
            object originalItem;
            EntityKey key = ((IObjectContextAdapter)this.DbContext).ObjectContext.CreateEntityKey(typeof(T).Name, entity);
            object oObject = entity;
            if (((IObjectContextAdapter)this.DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                ((IObjectContextAdapter)this.DbContext).ObjectContext.ApplyCurrentValues(key.EntitySetName, oObject);
            }

            int result = this.DbContext.SaveChanges();
            return result;
        }
        public IQueryable<T> GetQuerybale(Expression<Func<T, bool>> expression, bool bEnableLazyLoading)
        {


            this.DbContext.Configuration.LazyLoadingEnabled = bEnableLazyLoading;
            IList<T> list = expression != null ?
                ((IObjectContextAdapter)this.DbContext).ObjectContext.CreateQuery<T>("[" + typeof(T).Name + "]").Where(expression).ToList() :
                ((IObjectContextAdapter)this.DbContext).ObjectContext.CreateQuery<T>("[" + typeof(T).Name + "]").ToList();

            return list.AsQueryable();

        }
       

        public T GetSingle(Expression<Func<T, bool>> expression, bool bEnableLazyLoading)
        {
            ((IObjectContextAdapter)this.DbContext).ObjectContext.ContextOptions.LazyLoadingEnabled = bEnableLazyLoading;
            T result = ((IObjectContextAdapter)this.DbContext).ObjectContext
                .CreateQuery<T>(
                "[" + typeof(T).Name + "]")
                .Where(expression)
                .FirstOrDefault();
            return result;
        }
        public int Delete(T entity)
        {
            object originalItem;
            EntityKey key = ((IObjectContextAdapter)this.DbContext).ObjectContext.CreateEntityKey(typeof(T).Name, entity);
            if (((IObjectContextAdapter)this.DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                ((IObjectContextAdapter)this.DbContext).ObjectContext.DeleteObject(originalItem);
            }

            int result = this.DbContext.SaveChanges();
            return result;
        }
    }
}
