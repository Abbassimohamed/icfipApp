using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICFIP.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LoggerFilterAttribute : ActionFilterAttribute
    {


        private ILog _objLog4Net = null;
        string logLevel = string.Empty;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _objLog4Net = LogManager.GetLogger(filterContext.RouteData.Values["controller"].ToString());
            _objLog4Net.Debug(string.Concat("Entered ", filterContext.Controller.GetType().ToString(), "'s ", filterContext.ActionDescriptor.ActionName, " method"));
            //logLevel = GetLogLevel();
            //if (logLevel == "1")
            //{
                
            //}
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _objLog4Net = LogManager.GetLogger(filterContext.RouteData.Values["controller"].ToString());
            _objLog4Net.Debug(string.Concat("Existing ", filterContext.Controller.GetType().ToString(), "'s ", filterContext.ActionDescriptor.ActionName, " method"));
            //logLevel = GetLogLevel();
            //if (logLevel == "1")
            //{
                
            //}
        }

        //private string GetLogLevel()
        //{
        //    return ConfigurationManager.AppSettings["LOGLEVEL"].ToLower().ToString();
        //}




    }
}