using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eficProject.Models
{
    public class Etudiant
    {
        public int IdEtudiant { get; set; }
        public string Nom { get; set; }
        public DateTime DateNaissance { get; set; }
        public char Sexe { get; set; }
        public string Niveau { get; set; }
        public int Tel { get; set; }
        public string Email { get; set; }
      
    }
}