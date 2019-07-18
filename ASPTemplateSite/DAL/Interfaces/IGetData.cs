using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ASPTemplateSite.DAL.Interfaces
{
    interface IGetData<T>
    {
        List<T> getAll();
        T getEntity(int id);
        T getEntity(Expression<Func<T, bool>> request);
    }
}
