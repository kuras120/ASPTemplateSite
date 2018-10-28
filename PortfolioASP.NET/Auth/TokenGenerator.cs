using PortfolioASP.NET.DAL.Interfaces;
using PortfolioASP.NET.DAL.Services;
using PortfolioASP.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PortfolioASP.NET.DAL
{
    public class TokenGenerator
    {
        private const int _expirationMinutes = 60;
        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh"; // Generated at https://www.random.org/strings
        public static string GenerateToken(string username, string password, string ip, string userAgent, long ticks, bool hashed)
        {
            string hash = string.Join(":", new string[] { username, ip, userAgent, ticks.ToString() });
            string hashLeft = "";
            string hashRight = "";
            using (HMAC hmac = HMAC.Create(_alg))
            {
                var pswd = password;
                if (!hashed) pswd = GetHashedPassword(password);

                hmac.Key = Encoding.UTF8.GetBytes(pswd);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", new string[] { username, ticks.ToString() });
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }
        public static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });
            using (HMAC hmac = HMAC.Create(_alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hmac.Hash);
            }
        }
        public static bool IsTokenValid(string token, string ip, string userAgent)
        {
            bool result = false;
            try
            {
                // Base64 decode the string, obtaining the token:username:timeStamp.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    string username = parts[1];
                    long ticks = long.Parse(parts[2]);
                    DateTime timeStamp = new DateTime(ticks);
                    // Ensure the timestamp is valid.
                    bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
                    if (!expired)
                    {
                        var id = HttpContext.Current.Session["UserID"];
                        if (id != null)
                        {

                            IGetData<User> data = new DataService<User>();
                            var user = data.getEntity(Int32.Parse(id.ToString()));

                            if(user != null)
                            {

                                if (username == user.Login)
                                {
                                    string password = user.Password;
                                    // Hash the message with the key to generate a token.
                                    string computedToken = GenerateToken(username, password, ip, userAgent, ticks, true);
                                    // Compare the computed token with the one supplied and ensure they match.
                                    result = (token == computedToken);
                                }
                            }

                        }
                    }
                    else
                    {

                    }
                }
            }
            catch
            {
            }
            return result;
        }
    }
}