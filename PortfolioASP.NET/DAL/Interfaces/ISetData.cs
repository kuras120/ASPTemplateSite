using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioASP.NET.DAL.Interfaces
{
    interface ISetData<T>
    {
        void addEntity(T entity);
        void updateEntity(T entity);
        void deleteEntity(T entity);
        void deleteAll();
    }
}
