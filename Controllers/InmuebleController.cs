using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaParedes.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaParedes.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble repositorio;
        private readonly RepositorioPropietario repositorioPropietario;

        public InmuebleController()
        {
            repositorio = new RepositorioInmueble();
            repositorioPropietario = new RepositorioPropietario();
        }
        // GET: InmuebleController
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: InmuebleController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Inmueble inmueble = repositorio.ObtenerPorId(id);
            return View(inmueble);
        }

        // GET: InmuebleController/BuscarPorPropietario/5
        [Authorize]
        public ActionResult BuscarPorPropietario(int id)
        {
            var lista = repositorio.BuscarPorPropietario(id);
            return View("Index", lista);
        }

        // GET: InmuebleController/BuscarPorFecha/5
        [Authorize]
        public ActionResult BuscarPorFecha(DateTime inicio, DateTime fin)
        {
            var lista = repositorio.BuscarPorFecha(inicio, fin);
            return View("Index", lista);
        }

        // GET: InmuebleController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Estados = Inmueble.ObtenerEstados();
            ViewBag.EstadosUso = Inmueble.ObtenerUsos();
            ViewBag.EstadosTipo = Inmueble.ObtenerTipos();
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
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
                Inmueble i = new Inmueble();

                i.propietarioid  = int.Parse(collection["propietarioid"]);
                i.direccion      = collection["direccion"];
                i.uso            = byte.Parse(collection["uso"]);
                i.tipo           = byte.Parse(collection["tipo"]);
                i.cant_ambiente  = int.Parse(collection["Cant_ambiente"]);
                i.precio         = double.Parse(collection["Precio"]);
                i.superficie     = int.Parse(collection["Superficie"]);

                repositorio.Alta(i);

                TempData["Mensaje"] = "inmuble creado correctamente";


                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return View();
            }
        }

        // GET: InmubleController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            ViewBag.Estados = Inmueble.ObtenerEstados();
            ViewBag.EstadosUso = Inmueble.ObtenerUsos();
            ViewBag.EstadosTipo = Inmueble.ObtenerTipos();
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            return View(inmueble);//pasa el modelo a la vista
        }

        // POST: PropietarioController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inmueble i = null;

            try
            {
                i = repositorio.ObtenerPorId(id);

                i.propietarioid     = int.Parse(collection["propietarioid"]);
                i.direccion         = collection["direccion"];
                i.uso               = byte.Parse(collection["uso"]);
                i.tipo              = byte.Parse(collection["tipo"]);
                i.cant_ambiente     = int.Parse(collection["Cant_ambiente"]);
                i.precio            = double.Parse(collection["Precio"]);
                i.superficie        = int.Parse(collection["Superficie"]);
                i.estado            = byte.Parse(collection["Estado"]);

                repositorio.Modificacion(i);

                TempData["Mensaje"] = "Datos guardados correctamente";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return View();
            }
        }

        // GET: InmuebleController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            return View(inmueble);
        }

        // POST: PropietarioController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/ObtenerInmuebleIdJson/5
        public ActionResult ObtenerInmuebleIdJson(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return Json(p);
            //pasar a json.
        }

        public ActionResult ObtenerInmueblesJson()
        {
            var p = repositorio.ObtenerTodos();
            return Json(p);
            //pasar a json.
        }
    }
}