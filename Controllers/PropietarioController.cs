using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaParedes.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaParedes.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario repositorio;
        private readonly RepositorioInmueble repositorioInmueble;

        public PropietarioController()
        {
            repositorio = new RepositorioPropietario();
            repositorioInmueble = new RepositorioInmueble();
        }
        // GET: PropietarioController}

        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: PropietarioController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Propietario p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        // GET: PropietarioController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Estados = Propietario.ObtenerEstados();
            return View();
        }

        // POST: PropietarioController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Propietario p = new Propietario();
                p.dni           = collection["dni"];
                p.nombre        = collection["nombre"];
                p.apellido      = collection["apellido"];
                p.tel           = collection["tel"];
                p.email         = collection["email"];
                p.direccion     = collection["direccion"];

                repositorio.Alta(p);

                TempData["Mensaje"] = "Propietario creado correctamente";


                return RedirectToAction(nameof(Index));
            }
            catch (Exception e){
                Console.WriteLine("{0} Exception caught.", e);
                return View();
            }
        }

        // GET: PropietarioController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);
            ViewBag.Estados = Propietario.ObtenerEstados();
            return View(propietario);//pasa el modelo a la vista
        }

        // POST: PropietarioController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = null;

            try
            {
                p = repositorio.ObtenerPorId(id);

                p.dni           = collection["dni"];
                p.nombre        = collection["nombre"];
                p.apellido      = collection["apellido"];
                p.tel           = collection["tel"];
                p.email         = collection["email"];
                p.direccion     = collection["direccion"];
                p.estado        = byte.Parse(collection["estado"]);
                //no modifico la clave por aca
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

        // GET: PropietarioController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);
            return View(propietario);
        }

        // POST: PropietarioController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            int id = Int32.Parse(collection["propietarioid"]);
            try
            {
                var inmuebles = repositorioInmueble.BuscarPorPropietario(id);
                foreach (var inmueble in inmuebles){
                    repositorioInmueble.Baja(inmueble.id);
                }
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PropietarioController/ObtenerP/5
        public ActionResult ObtenerPropietarioIdJson(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return Json(p);
            //pasar a json.
        }

        public ActionResult ObtenerPropietariosJson()
        {
            var p = repositorio.ObtenerTodos();
            return Json(p);
            //pasar a json.
        }
    }
}