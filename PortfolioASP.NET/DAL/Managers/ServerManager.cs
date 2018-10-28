using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioASP.NET.DAL
{
    public static class ServerManager
    {
        public static string GetIP(HttpRequestBase request)
        {
            string ip = request.ServerVariables["X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip)) ip = request.UserHostAddress;

            return ip;
        }
    }
}