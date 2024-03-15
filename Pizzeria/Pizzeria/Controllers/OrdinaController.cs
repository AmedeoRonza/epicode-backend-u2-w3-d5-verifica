using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pizzeria.Models;

namespace Pizzeria.Controllers
{
    public class OrdinaController : Controller
    {
        // GET: Ordina
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Ordina(string note, string indirizzo)
        {
            ModelDbContext db = new ModelDbContext();
            var userId = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name).IdUtente;
            var cart = Session["Carrello"] as List<Pizza>;

            if (cart != null && cart.Any())
            {
                foreach (var pizza in cart)
                {
                    Ordine newOrder = new Ordine();
                    newOrder.FK_IdUtente = userId;
                    newOrder.FK_IdPizza = pizza.IdPizza; // Ottieni l'ID della pizza dall'oggetto pizza nel carrello
                    newOrder.IndirizzoConsegna = indirizzo;
                    newOrder.Totale = pizza.Prezzo; // Usa il prezzo della pizza come totale dell'ordine
                    newOrder.Nota = note;

                    db.Ordine.Add(newOrder);
                }

                db.SaveChanges();
                cart.Clear();
            }

            TempData["CreateMess"] = "L'ordine è stato inviato correttamente";
            return RedirectToAction("Index", "Pizza");
        }
    }
}