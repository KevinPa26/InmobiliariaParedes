using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaParedes.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaParedes.Controllers
{
    public class PagoController : Controller
    {
        private readonly RepositorioPago repositorio;

        public PagoController()
        {
            repositorio = new RepositorioPago();
        }
        // GET: PagoController
        [Authorize]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: PagoController
        [Authorize]
        public ActionResult BuscarPorContrato(int id)
        {
            var lista = repositorio.ObtenerPorContrato(id);
            ViewBag.ContratoId = id;
            return View("Index", lista);
        }

        // GET: PagoController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Pago p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        // GET: PagoController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public ActionResult CreateConContrato(int id)
        {
            ViewBag.ContratoId = id;
            return View("Create");
        }

        // POST: PagoController/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Pago p = new Pago();
                p.contratoid      = int.Parse(collection["contratoid"]);
                p.fecha           = DateTime.Now;
                p.importe         = double.Parse(collection["importe"]);
                p.num_pago        = repositorio.ObtenerPorContrato(int.Parse(collection["contratoid"])).Count > 0 ? repositorio.ObtenerPorContrato(int.Parse(collection["contratoid"])).Count + 1 : 1 ;

                repositorio.Alta(p);

                TempData["Mensaje"] = "Pago creado correctamente";


                return RedirectToAction("BuscarPorContrato","Pago",new { id = p.contratoid});
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return View();
            }
        }

        // GET: PagoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var pago = repositorio.ObtenerPorId(id);
            return View(pago);//pasa el modelo a la vista
        }

        // POST: PagoController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Pago p = null;

            try
            {
                p = repositorio.ObtenerPorId(id);

                p.contratoid      = int.Parse(collection["contratoid"]);
                p.fecha           = System.DateTime.Parse(collection["fecha"]);
                p.importe         = double.Parse(collection["importe"]);
                p.num_pago        = int.Parse(collection["num_pago"]);

                repositorio.Modificacion(p);

                TempData["Mensaje"] = "Datos guardados correctamente";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return View();
            }
        }

        // GET: PagoController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var pago = repositorio.ObtenerPorId(id);
            return View(pago);
        }

        // POST: PagoController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                int id = Int32.Parse(collection["pagoid"]);
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PagoController/ObtenerPagoIdJson/5
        public ActionResult ObtenerPagoIdJson(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return Json(p);
            //pasar a json.
        }
    }
}