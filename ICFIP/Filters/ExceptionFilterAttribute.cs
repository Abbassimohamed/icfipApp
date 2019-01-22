using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICFIP.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ExceptionFilterAttribute : HandleErrorAttribute
    {
        private ILog _objLog4Net = null;
        string logLevel = string.Empty;
        public override void OnException(ExceptionContext filterContext)
        {

            _objLog4Net = LogManager.GetLogger(filterContext.RouteData.Values["controller"].ToString());
            _objLog4Net.Error(filterContext.Exception.Message, filterContext.Exception);
            //logLevel = GetLogLevel();
            //if (logLevel == "1" || logLevel == "2")
            //{
            //    _objLog4Net.Error(filterContext.Exception.Message, filterContext.Exception);
            //}

            //if (logLevel == "3")
            //{
            //    if (filterContext.Exception.GetType().IsSubclassOf(typeof(ApplicationException)))
            //    {
            //        _objLog4Net.Error(filterContext.Exception.Message, filterContext.Exception);
            //    }
            //}
        }


    }
}