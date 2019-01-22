using ICFIP.Common.Config;
using ICFIP.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICFIP.Common.Tools
{
   public  class AppUtils
    {
       public ICFIPEntities GetDbContext(string strConn)
       {
           return new ICFIPEntities(AppConfig.DbConnexionString);
       }
    }
}
