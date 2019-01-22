using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICFIP.Services.Contrat
{
    interface IRepository<T> where T:class 
    {
       
        IList<T> GetAll(Expression<Func<T, bool>> expression, bool bEnableLazyLoading);
        T GetSingle(Expression<Func<T, bool>> expression, bool bEnableLazyLoading);
        int Add(T entity);
        int Delete(T entity);
        int Update(T entity);
        
    }
}
