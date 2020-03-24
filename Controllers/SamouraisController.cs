using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using Dojo.Models;
using TP_LeDojo.Data;

namespace TP_LeDojo.Controllers
{
    public class SamouraisController : Controller
    {
        private Context db = new Context();

        // GET: Samourais
        public ActionResult Index()
        {
            var samourais = db.Samourais.Include(s => s.Arme).Include(s => s.ArtMartials).ToList();
            return View(samourais);
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Include(s => s.Arme).Include(s => s.ArtMartials).Where(s => s.Id == id).FirstOrDefault();
            if (samourai == null)
            {
                return HttpNotFound();
            }
            //samourai.Potentiel = (samourai.Force + samourai.Arme.Degats) * (samourai.ArtMartials.Count + 1);
            samourai.Potentiel = samourai.calculPotentiel();
            return View(samourai);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            SamouraiVM samouraiVM = new SamouraiVM();

            var allArmes = db.Armes.ToList();
            List<Arme> armesLibres = new List<Arme>();
            foreach(Arme arme in allArmes)
            {
                if(!db.Samourais.Where(s => s.Arme.Id == arme.Id).ToList().Any())
                {
                    armesLibres.Add(arme);
                }
            }
            samouraiVM.Armes = armesLibres;
            samouraiVM.ArtMartials = db.ArtMartials.ToList();
            return View(samouraiVM);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                if (samouraiVM.IdSelectedArme.HasValue)
                {
                    Arme arme = db.Armes.FirstOrDefault(a => a.Id == samouraiVM.IdSelectedArme.Value);
                    samouraiVM.Samourai.Arme = arme;
                    //Vérifier si l'arme n'est pas déjà associée à un samourai
                    var samouraisWithArme = db.Samourais.Where(s => s.Arme.Id == arme.Id).ToList();
                    if (samouraisWithArme.Any())
                    {
                        samouraiVM.Samourai.Arme = null;
                    } 
                }
                if (samouraiVM.IdsSelectedArtMartiaux != null)
                {
                    foreach (var idSelectedArtMartial in samouraiVM.IdsSelectedArtMartiaux)
                    {
                        ArtMartial artMartial = db.ArtMartials.FirstOrDefault(a => a.Id == idSelectedArtMartial);
                        samouraiVM.Samourai.ArtMartials.Add(artMartial);
                    }
                }

                // On ajoute le nouveau samouraï au DbSet, puis on enregistre les changements en base
                db.Samourais.Add(samouraiVM.Samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            samouraiVM.Armes = db.Armes.ToList();
            samouraiVM.ArtMartials = db.ArtMartials.ToList();
            return View(samouraiVM);
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Include(s => s.Arme).Include(s => s.ArtMartials).Where(s => s.Id == id).FirstOrDefault();
            if (samourai == null)
            {
                return HttpNotFound();
            }
            SamouraiVM samouraiVM = new SamouraiVM();

            var allArmes = db.Armes.ToList();
            List<Arme> armesLibres = new List<Arme>();
            foreach (Arme arme in allArmes)
            {
                if (!db.Samourais.Where(s => s.Arme.Id == arme.Id).ToList().Any())
                {
                    armesLibres.Add(arme);
                }
            }
            armesLibres.Add(samourai.Arme);
            samouraiVM.Armes = armesLibres;
            samouraiVM.ArtMartials = db.ArtMartials.ToList();
            samouraiVM.Samourai = samourai;

            if (samourai.Arme != null)
            {
                samouraiVM.IdSelectedArme = samourai.Arme.Id;
            }
            if (samourai.ArtMartials.Any())
            {
                samouraiVM.IdsSelectedArtMartiaux = samourai.ArtMartials.Select(a => a.Id).ToList();
            }

            return View(samouraiVM);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                Samourai samouraiDb = db.Samourais.Find(samouraiVM.Samourai.Id);
                samouraiDb.Force = samouraiVM.Samourai.Force;
                samouraiDb.Nom = samouraiVM.Samourai.Nom;
                samouraiDb.Arme = null;
                samouraiDb.ArtMartials = new List<ArtMartial>();

                if (samouraiVM.IdSelectedArme.HasValue)
                {
                    Arme arme = db.Armes.FirstOrDefault(a => a.Id == samouraiVM.IdSelectedArme.Value);
                    samouraiVM.Samourai.Arme = arme;
                    //Vérifier si l'arme n'est pas déjà associée à un samourai
                    var samouraisWithArme = db.Samourais.Where(s => s.Arme.Id == arme.Id).ToList();
                    if (samouraisWithArme.Any())
                    {
                        samouraiVM.Samourai.Arme = null;
                        //samouraiDb.Arme = null;
                    }
                }

                if (samouraiVM.IdsSelectedArtMartiaux != null)
                {
                    samouraiVM.Samourai.ArtMartials = new List<ArtMartial>();
                    foreach (var idSelectedArtMartial in samouraiVM.IdsSelectedArtMartiaux)
                    {
                        ArtMartial artMartial = db.ArtMartials.FirstOrDefault(a => a.Id == idSelectedArtMartial);
                        samouraiVM.Samourai.ArtMartials.Add(artMartial);
                    }
                    samouraiDb.ArtMartials = samouraiVM.Samourai.ArtMartials;
                }
                samouraiDb = samouraiVM.Samourai;
                
                db.Entry(samouraiVM.Samourai).State = EntityState.Modified;
                db.Entry(samouraiDb).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(samouraiVM);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Include(s => s.Arme).Include(s => s.ArtMartials).Where(s => s.Id == id).FirstOrDefault();
            if (samourai == null)
            {
                return HttpNotFound();
            }
            samourai.Potentiel = samourai.calculPotentiel();
            return View(samourai);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Include(s => s.Arme).Include(s => s.ArtMartials).Where(s => s.Id == id).FirstOrDefault();
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
