using ExportManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ExportManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static CacheItemRemovedCallback OnCacheRemove;
        private void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        public void CacheItemRemoved(string taskName, object seconds, CacheItemRemovedReason removalReason)
        {

            var send_mail = new EmailSend();
            {
                send_mail.send_expiry_mail();
            }

            AddTask(taskName, Convert.ToInt32(seconds));
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AddTask("EmailDailyTask", 86400);
        }
    }
}
