using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICFIP.Entites
{
    public partial class Enseignant
    {
        public string MotdePasse { get; set; }
        public int IsPresnece { get; set; }
        public bool BoolIsPresnece
        {
            get { return IsPresnece == 1; }
            set
            {
                if (value)
                    IsPresnece = 1;
                else
                    IsPresnece = 0;
            }
        }

    }
}
