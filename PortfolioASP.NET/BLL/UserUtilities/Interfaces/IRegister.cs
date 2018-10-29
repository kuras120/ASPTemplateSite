using PortfolioASP.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioASP.NET.BLL.UserUtilities.Interfaces
{
    interface IRegister
    {
        bool Validate(string username);
        void AddUser(string username, string password, string email);
        
    }
}
