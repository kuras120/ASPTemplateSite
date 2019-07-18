using ASPTemplateSite.BLL.UserUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPTemplateSite.DAL.Interfaces;
using ASPTemplateSite.DAL.Services;
using ASPTemplateSite.Models;
using ASPTemplateSite.DAL;

namespace ASPTemplateSite.BLL.UserUtilities
{
    public class UserManager : ILogIn, IRegister
    {
        private static UserManager instance = null;
        private IGetData<User> getData;
        private ISetData<User> setData;
        private static NLog.Logger logger;

        public User CurrentUser { get; set; }
        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new UserManager();
                return instance;
            }
        }
        private UserManager()
        {
            getData = new DataService<User>();
            setData = new DataService<User>();
            logger = NLog.LogManager.GetCurrentClassLogger();
        }
        bool ILogIn.Validate(string username, string password)
        {
            var hashedPassword = TokenGenerator.GetHashedPassword(password);
            var user = getData.getEntity(x => x.Login == username && x.Password == hashedPassword);

            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            else return false;
        }
        bool IRegister.Validate(string username)
        {
            var user = getData.getEntity(x => x.Login == username);
            if (user == null)
            {
                return true;
            }
            else return false;

        }

        void IRegister.AddUser(string username, string password, string email)
        {
            using (var context = DataModel.Create())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var settings = new Settings
                    {
                        BackgroundType = 0,
                        BanTime = null
                    };

                    var user = new User
                    {
                        Login = username,
                        Password = TokenGenerator.GetHashedPassword(password),
                        Email = email,
                        UserSettings = settings
                    };

                    context.Settings.Add(settings);
                    context.Users.Add(user);

                    logger.Info("Created " + user.Login);

                    var log = new Logs
                    {
                        Type = LogType.Info,
                        Content = user.Login + " created",
                        Date = DateTime.Today,
                        UserLogs = user
                    };

                    context.Logs.Add(log);

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    logger.Error(ex.Message);
                    transaction.Rollback();
                }
            }
        }

        User ILogIn.GetUser(string username)
        {
            return getData.getEntity(x => x.Login == username);
        }
    }
}