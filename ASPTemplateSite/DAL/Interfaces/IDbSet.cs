using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPTemplateSite.DAL.Interfaces
{
    interface IDbSet
    {
        void wipe();
        void create();
        void delete();
    }
}
