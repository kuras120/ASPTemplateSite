using PortfolioASP.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioASP.NET.BLL.UserUtilities.Interfaces
{
    interface ILogIn
    {
        bool Validate(string username, string password);
        User GetUser(string username);
    }
}
 