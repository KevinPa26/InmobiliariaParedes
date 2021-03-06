using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaParedes.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace InmobiliariaParedes.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino repositorio;

        public InquilinoController()
        {
            repositorio = new RepositorioInquilino();
        }
        // GET: InquilinoController
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

        // GET: InquilinoController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var i = repositorio.ObtenerPorId(id);
            return View(i);
        }

        // GET: InquilinoController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Estados = Propietario.ObtenerEstados();
            return View();
        }

        // POST: InquilinoController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Inquilino i = new Inquilino();
                i.nombre    = collection["nombre"];
                i.apellido  = collection["apellido"];
                i.dni       = collection["dni"];
                i.tel       = collection["tel"];
                i.email     = collection["email"];
                i.direccion = collection["direccion"];

                repositorio.Alta(i);

                TempData["Mensaje"] = "Inquilino creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var inquilino = repositorio.ObtenerPorId(id);
            ViewBag.Estados = Propietario.ObtenerEstados();
            return View(inquilino);//pasa el modelo a la vista
        }

        // POST: InquilinoController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino i = null;

            try
            {
                i = repositorio.ObtenerPorId(id);

                i.nombre    = collection["nombre"];
                i.apellido  = collection["apellido"];
                i.dni       = collection["dni"];
                i.tel       = collection["tel"];
                i.email     = collection["email"];
                i.direccion = collection["direccion"];
                i.estado    = byte.Parse(collection["estado"]);
                
                repositorio.Modificacion(i);

                TempData["Mensaje"] = "Datos guardados correctamente";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinoController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);
            return View(propietario);
        }

        // POST: InquilinoController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                int id = Int32.Parse(collection["inquilinoid"]);
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminaci??n realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: InquilinoController/ObtenerP/5
        public ActionResult ObtenerInquilinoIdJson(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return Json(p);
        }

        public ActionResult ObtenerInquilinosJson()
        {
            var p = repositorio.ObtenerTodos();
            return Json(p);
            //pasar a json.
        }
    }
}