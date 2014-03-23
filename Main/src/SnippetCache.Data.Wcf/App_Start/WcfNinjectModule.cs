using Ninject.Modules;
using Ninject.Web.Common;
using SnippetCache.Data.EF4;
using SnippetCache.Data.EF4.Model;

namespace SnippetCache.Data.Wcf.App_Start
{
    public class WcfNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<Language>>().To<GenericRepository<Language>>().InRequestScope();
            Bind<IRepository<Comment>>().To<GenericRepository<Comment>>().InRequestScope();
            Bind<IRepository<User>>().To<GenericRepository<User>>().InRequestScope();
            Bind<IRepository<Notification>>().To<GenericRepository<Notification>>().InRequestScope();
            Bind<IRepository<NotificationType>>().To<GenericRepository<NotificationType>>().InRequestScope();
            Bind<IRepository<File>>().To<GenericRepository<File>>().InRequestScope();
            Bind<IRepository<Hyperlink>>().To<GenericRepository<Hyperlink>>().InRequestScope();
            Bind<IRepository<Snippet>>().To<GenericRepository<Snippet>>().InRequestScope();
            Bind<DataUnitOfWork>().ToSelf().InRequestScope();
        }
    }
}