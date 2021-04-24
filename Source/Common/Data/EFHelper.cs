using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;

namespace Common.Data
{
    /// <summary>
    /// Provides static helper methods for using Enitity Framework
    /// </summary>
    public static class EFHelper
    {
        /// <summary>
        /// Returns the current TContext object that is associated to the current HTTP request.
        /// </summary>
        /// <typeparam name="TContext">Context type</typeparam>
        /// <returns>A TContext object that is associated to current HTTP request</returns>
        public static TContext GetContext<TContext>() where TContext : DbContext, new()
        {
            var dataContext = HttpContext.Current.Items[typeof(TContext).Name] as TContext;
            if (dataContext == null) {
                dataContext = new TContext();
                System.Web.HttpContext.Current.Items.Add(typeof(TContext).Name, dataContext);
            }
            return dataContext;
        }

        /// <summary>
        /// Saves all changes made in TContext to the underlaying database.
        /// </summary>
        public static void SaveChanges<TContext>() where TContext : DbContext
        {
            var context = HttpContext.Current.Items[typeof(TContext).Name] as TContext;
            if (context != null) {
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Dispose the TContext object that is associated to the current HTTP request
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        public static void DisposeContext<TContext>() where TContext : DbContext
        {
            string contextName = typeof(TContext).Name;
            TContext dataContext = HttpContext.Current.Items[contextName] as TContext;
            if (dataContext != null) {
                dataContext.Dispose();
            }
        }

    }
}
