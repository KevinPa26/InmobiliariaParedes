using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaParedes.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaParedes.Controllers
{
    public class ContratoController : Controller
    {
        private readonly RepositorioContrato repositorio;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly RepositorioPago repositorioPago;

        public ContratoController()
        {
            repositorio             = new RepositorioContrato();
            repositorioInmueble     = new RepositorioInmueble();
            repositorioInquilino    = new RepositorioInquilino();
            repositorioPago         = new RepositorioPago();
        }
        // GET: PropietarioController
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: PropietarioController
        [Authorize]
        public ActionResult BuscarPorInmueble(int id)
        {
            var lista = repositorio.ObtenerPorInmueble(id);
            return View("Index", lista);
        }

        public ActionResult BuscarPorInmuebleJson(int id)
        {
            var lista = repositorio.ObtenerPorInmueble(id);
            return Json(lista);
        }

        [Authorize]
        public ActionResult BuscarPorInquilino(int id)
        {
            var lista = repositorio.ObtenerPorInquilino(id);
            return View("Index", lista);
        }

        // GET: PropietarioController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Contrato c = repositorio.ObtenerPorId(id);
            return View(c);
        }

        // GET: PropietarioController/Create
        [Authorize]
        public ActionResult Create()
        {
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
                Contrato c = new Contrato();
                c.inmuebleid            = int.Parse(collection["inmuebleid"]);
                c.inquilinoid           = int.Parse(collection["inquilinoid"]);
                c.fecha_inicio          = System.DateTime.Parse(collection["fecha_inicio"]);
                c.fecha_fin             = System.DateTime.Parse(collection["fecha_fin"]);
                c.monto_mes             = double.Parse(collection["monto_mes"]);
                c.garante_nombre        = collection["garante_nombre"];
                c.garante_apellido      = collection["garante_apellido"];
                c.garante_dni           = collection["garante_dni"];
                c.garante_tel           = collection["garante_tel"];
                c.creado                = System.DateTime.Now;
                c.modificado            = System.DateTime.Now;

                repositorio.Alta(c);

                TempData["Mensaje"] = "Contrato creado correctamente";


                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return View();
            }
        }

        // GET: ContratoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var contrato = repositorio.ObtenerPorId(id);
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            return View(contrato);//pasa el modelo a la vista
        }

        // POST: ContratoController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Contrato c = null;

            try
            {
                c = repositorio.ObtenerPorId(id);

                c.inmuebleid            = int.Parse(collection["inmuebleid"]);
                c.inquilinoid           = int.Parse(collection["inquilinoid"]);
                c.fecha_inicio          = System.DateTime.Parse(collection["fecha_inicio"]);
                c.fecha_fin             = System.DateTime.Parse(collection["fecha_fin"]);
                c.monto_mes             = double.Parse(collection["monto_mes"]);
                c.garante_nombre        = collection["garante_nombre"];
                c.garante_apellido      = collection["garante_apellido"];
                c.garante_dni           = collection["garante_dni"];
                c.garante_tel           = collection["garante_tel"];
                c.creado                = System.DateTime.Now;
                c.modificado            = System.DateTime.Now;

                repositorio.Modificacion(c);

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
            var contrato = repositorio.ObtenerPorId(id);
            return View(contrato);
        }

        // POST: PropietarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                int id = Int32.Parse(collection["contratoidbaja"]);
                var pagos = repositorioPago.ObtenerPorContrato(id);
                foreach (var pago in pagos){
                    repositorioPago.Baja(pago.id);
                }
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PropietarioController/ObtenerP/5
        public ActionResult ObtenerContratoIdJson(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return Json(p);
            //pasar a json.
        }
    }
}