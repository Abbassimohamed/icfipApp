using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eficProject.Models
{
    public class Cour
    {
        public int IdCour { get; set; }
        public string Libellé { get; set; }
        public int nbHeur { get; set; }
        public int Coefciant { get; set; }
    }
}