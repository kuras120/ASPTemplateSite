using ASPTemplateSite.Models;
using ASPTemplateSite.Logger;
using ASPTemplateSite.DAL.Interfaces;
using ASPTemplateSite.DAL.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ASPTemplateSite.DAL
{
    public class DataManager
    {
        private DataModel context;
        private Random rand;
        private static NLog.Logger logger;
        public DataManager()
        {
            context = new DataModel();
            rand = new Random();
            logger = NLog.LogManager.GetCurrentClassLogger();
        }
        public void CreateRelease()
        {
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
                        Login = "admin",
                        Password = TokenGenerator.GetHashedPassword("admin1"),
                        Email = "admin@gmail.com",
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
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    transaction.Rollback();
                }
            }
        }

        public void CreateDebug(int testUnits)
        {
            for(int i = 0; i < testUnits; i++)
            {
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
                            Login = "user" + i,
                            Password = TokenGenerator.GetHashedPassword("user1" + i),
                            Email = "user" + i + "@gmail.com",
                            UserSettings = settings
                        };

                        var log = new Logs
                        {
                            Type = LogType.Info,
                            Content = user.Login + " created",
                            Date = DateTime.Today,
                            UserLogs = user
                        };

                        context.Settings.Add(settings);
                        context.Users.Add(user);
                        logger.Info("Created " + user.Login);
                        context.Logs.Add(log);

                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        transaction.Rollback();
                    }
                }
                //ctrl+k+c / ctrl+k+u

                using (var transaction = context.Database.BeginTransaction())
                {
                    IGetData<User> userService = new DataService<User>();
                    var user = userService.getEntity(i + 1);

                    try
                    {
                        var post = new Post
                        {
                            Name = StringGenerators.GenerateTitle(),
                            Content = StringGenerators.GenerateContent(1),
                            Date = DateTime.Today,
                            UserId = user.Id
                        };

                        var comment = new Comment
                        {
                            Content = StringGenerators.GenerateContent(1),
                            Date = DateTime.Today,
                            UserId = user.Id,
                            ParentPost = post
                        };

                        var log = new Logs
                        {
                            Type = LogType.Info,
                            Content = user.Login + " wrote something",
                            Date = DateTime.Today,
                            UserId = user.Id
                        };

                        context.Posts.Add(post);
                        context.Comments.Add(comment);
                        context.Logs.Add(log);

                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        transaction.Rollback();
                    }
                }
            }
        }
        
        public void DeleteAll()
        {
            List<string> tableNames = context.Database.SqlQuery<string>("SELECT name FROM sys.tables ORDER BY name").ToList();
            tableNames.Remove("__MigrationHistory");

            foreach (var tableName in tableNames)
            {
                context.Database.ExecuteSqlCommand("DELETE FROM [" + tableName + "]");
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT(" + tableName + ", RESEED, 0)");
            }
        }

        [Obsolete("Use DeleteAll instead")]
        public void DeleteAllOld()
        {
            var objectItemCollection =
                (ObjectItemCollection)((IObjectContextAdapter)context)
                .ObjectContext.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);

            foreach (var entityType in objectItemCollection.GetItems<EntityType>())
            {
                System.Diagnostics.Debug.WriteLine(objectItemCollection.GetClrType(entityType).FullName);

                var entity = objectItemCollection.GetClrType(entityType).FullName;
                var genericType = typeof(DataService<>).MakeGenericType(Type.GetType(entity));
                var instance = Activator.CreateInstance(genericType);
                var method = genericType.GetMethod("deleteAll");

                var result = method.Invoke(instance, null);

                System.Diagnostics.Debug.WriteLine(result);
            }
        }
    }
}