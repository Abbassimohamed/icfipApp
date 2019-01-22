using ICFIP.Common.Config;
using ICFIP.Services.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using ICFIP.Entites;
using ICFIP.Filters;

namespace ICFIP.Controllers
{
    [InitializeSimpleMembershipAttribute]
    public class EnseignantController : Controller
    {
        //
        // GET: /Enseignant/

        public ActionResult Index()
        {
            Repository<Formation> _repFor = new Repository<Formation>(AppConfig.DbConnexionString);
            List<Formation> Formation = _repFor.GetAll(null, false).ToList();

            ViewBag.IdGroupFormation = new SelectList(Formation, "IdFormation", "LibelleFormation", "DescriptionFormation");


            Repository<TypeSpecialite> _repSpec = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
            List<TypeSpecialite> Specialite = _repSpec.GetAll(null, false).ToList();

            ViewBag.IdGroupspecialite = new SelectList(Specialite, "IdSpetialite", "LibelleSpecialite", "DescriptionSpetialite");


            Repository<Cour> _repradbtn = new Repository<Cour>(AppConfig.DbConnexionString);
            List<Cour> Cour = _repradbtn.GetAll(null, false).ToList();

            ViewBag.IdGroupCour = new SelectList(Cour, "IdCour", "Libelle");


            Repository<Niveau> _repNIV = new Repository<Niveau>(AppConfig.DbConnexionString);
            List<Niveau> Niveau = _repNIV.GetAll(null, false).ToList();

            ViewBag.IdGroupNiveau = new SelectList(Niveau, "IdNiveau", "Libelle");



            Repository<Semestre> _repTri = new Repository<Semestre>(AppConfig.DbConnexionString);
            List<Semestre> simestre = _repTri.GetAll(null, false).ToList();

            ViewBag.IdGroupsimestre = new SelectList(simestre, "IdSemestre", "LibelleSemestre");

            return View();
        }



        public ActionResult AjouterAbs(int Idfo, int Idspec, int Idcour, int Idni, int Idsem)
        {

            Etudiants Letudiants = new Etudiants();
            Repository<Etudiant> _repcheck = new Repository<Etudiant>(AppConfig.DbConnexionString);
            List<Etudiant> _letudiant = _repcheck.GetAll(x => x.IdFormation == Idfo && x.IdSpetialite == Idspec && x.IdNiveau == Idni, false).ToList();
            Letudiants.ListeEtudiants = _letudiant;






            return View("AjouterAbs", Letudiants);    

       
        }


        [HttpPost]
        public ActionResult AjouterAbs(Etudiants etudi, int Idfo, int Idspec, int Idcour, int Idni, int Idsem)
        {

            Repository<Presence> _reppresence = new Repository<Presence>(AppConfig.DbConnexionString);
            List<Presence> _lAbsnece = new List<Presence>();

            DateTime dt = DateTime.Now;//.AddDays(1);

            _lAbsnece = (from e in etudi.ListeEtudiants
                         select new Presence()
                         {
                             IdFormation = Idfo,
                             IdSpetialite = Idspec,  
                             Idetudiant = e.Idetudiant,
                             Absence = e.IsPresnece,
                             DateAbsnece = dt,
                             HeurAbsence = new TimeSpan(dt.Hour, dt.Minute, dt.Second),
                             IdNiveau = Idni,
                             IdCour = Idcour,
                             IdSemestre = Idsem
                             
                        


                         }).ToList();
            foreach (Presence p in _lAbsnece)
            {
                _reppresence.Add(p);
            }

            return RedirectToAction("Index");


        }




        public ActionResult AjouterNote(int Idfo, int Idspec, int Idcour, int Idni, int Idsem)
        {
            Etudiants Letudiants = new Etudiants();
            Repository<Etudiant> _repcheck = new Repository<Etudiant>(AppConfig.DbConnexionString);
            List<Etudiant> _letudiant = _repcheck.GetAll(x => x.IdFormation == Idfo && x.IdSpetialite==Idspec    &&x.IdNiveau==Idni , false).ToList();
            Letudiants.ListeEtudiants = _letudiant;



            return View("AjouterNote", Letudiants);




        }


        [HttpPost]
        public ActionResult AjouterNote(Etudiants etudi, int Idfo, int Idspec, int Idcour, int Idni, int Idsem)
        {

            Repository<Note> _reppresence = new Repository<Note>(AppConfig.DbConnexionString);
            List<Note> _lAbsnece = new List<Note>();
        
            DateTime dt = DateTime.Now;//.AddDays(1);

            _lAbsnece = (from e in etudi.ListeEtudiants
                         select new Note()
                         {
                             IdCour = Idcour,
                             IdFormation=Idfo,                                                         
                             Idetudiant = e.Idetudiant,
                             Note1 = e.Note1,
                             Note2 = e.Note2,
                             Note3 = e.Examen,
                             DateNote = dt,
                             HeurNote = new TimeSpan(dt.Hour, dt.Minute, dt.Second),
                             IdNiveau = Idni,
                             IdSemestre = Idsem,
                             IdSpetialite=Idspec,
                           


                         }).ToList();
            foreach (Note p in _lAbsnece)
            {
                _reppresence.Add(p);
            }
            return RedirectToAction("Index");

        }





       



        public ActionResult AjouterNotebts(int IdBTS, int IdcOURbt, int IdNivbt)
        {
            Etudiants Letudiants = new Etudiants();
            Repository<Etudiant> _repcheck = new Repository<Etudiant>(AppConfig.DbConnexionString);
            List<Etudiant> _letudiant = _repcheck.GetAll(null, false).ToList();
            Letudiants.ListeEtudiants = _letudiant;



            return View("AjouterNotebts", Letudiants);



        }

        [HttpPost]
        public ActionResult AjouterNotebts(Etudiants etudi, int IdBTS, int IdcOURbt, int IdNivbt)
        {


            Repository<Note> _reppresence = new Repository<Note>(AppConfig.DbConnexionString);
            List<Note> _lAbsnece = new List<Note>();

            DateTime dt = DateTime.Now;//.AddDays(1);

            _lAbsnece = (from e in etudi.ListeEtudiants
                         select new Note()
                         {
                             IdCour = IdcOURbt,
                             Idetudiant = e.Idetudiant,
                              Note1=e.Note1,
                              Note2=e.Note2,
                              Note3=e.Examen,
                             DateNote = dt,
                             HeurNote = new TimeSpan(dt.Hour, dt.Minute, dt.Second),
                             IdNiveau = IdNivbt,
                             IdBts = IdBTS


                         }).ToList();
            foreach (Note p in _lAbsnece)
            {
                _reppresence.Add(p);
            }

            return RedirectToAction("Index");
        }




        


     
     
    }
}
