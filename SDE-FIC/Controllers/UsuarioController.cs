using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDE_FIC.DAO;
using SDE_FIC.Models;
using PagedList;
using SDE_FIC.Util;

namespace SDE_FIC.Controllers
    
{
   
    public class UsuarioController : Controller
     
    {
        private DBConnect _db = new DBConnect();

        public ViewResult VerificarSessao()
        {
            if (Session["LogedUserNomeCompleto"] == null)
            {
                ViewBag.msg_Error = "Tempo de Sessão Expirou! Autentique novamente.";
                this.Danger(string.Format("Tempo de Sessão Expirou! Autentique novamente."));
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                var NomeComleto = Session["LogedUserNomeCompleto"].ToString();
            }
            return View();
        }
   
        //
        // GET: /Usuario/

        public ActionResult Index(int? pagina, string strCriterio)
        {
            VerificarSessao();

            int tamanhoPagina = 15;
            int numeroPagina = pagina ?? 1;

            CursoDAO _cursoDAO = new CursoDAO(ref _db);
            UsuarioDAO usuarioDAO = new UsuarioDAO(ref _db);

            List<Usuario> lUsuario = new List<Usuario>();
            

            if (strCriterio == null)
            {
                lUsuario = usuarioDAO.All().OrderBy(x => x.NomeCompleto).ToList();
            }
            else
            {
                lUsuario = usuarioDAO.All().OrderBy(x => x.NomeCompleto).Where(x => x.NomeCompleto.ToUpper().Contains(strCriterio.ToUpper())).ToList();
            }


            return View("~/Views/Sistema/Usuario/Index.cshtml", lUsuario.ToPagedList(numeroPagina, tamanhoPagina));
        }

        //
        // GET: /Usuario/Details/5

        public ActionResult Details(int id = 0)
        {
            VerificarSessao();

            UsuarioDAO usuarioDAO = new UsuarioDAO(ref _db);
            Usuario _usuario = new Usuario();
            _usuario = usuarioDAO.LoadById(id);

            return View("~/Views/Sistema/Usuario/Details.cshtml", _usuario);
        }

        //
        // GET: /Usuario/Create

        public ActionResult Create()
        {
            VerificarSessao();

            return View("~/Views/Sistema/Usuario/Create.cshtml");
        }


        //
        // POST: /Usuario/Create

        [HttpPost]
        public ActionResult Create(Usuario usuario, int? pagina)
        {

            if (ModelState.IsValid)
            {
                UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
                _usuarioDAO.Insert(usuario);

                this.Success(string.Format("Registro salvo com sucesso!"),
                                                            true);
                return RedirectToAction("Index");
            }
            else
            {                
                this.Danger(string.Format("Falha ao Cadastrar Registro! Verifique se os campos estão preenchidos."));
            }
            return View("~/Views/Sistema/Usuario/Create.cshtml", usuario);
        }

        //
        // GET: /Usuario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VerificarSessao();

            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
            Usuario _usuario = new Usuario();

            _usuario = _usuarioDAO.LoadById(id);

            if (_usuario == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Sistema/Usuario/Edit.cshtml", _usuario);
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        public ActionResult Edit(Usuario usuario, int? pagina)
        {
            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
            Usuario _usuario = _usuarioDAO.LoadById(usuario.IdUsuario);

            if (ModelState.IsValid)
            {

                _usuarioDAO.Update(usuario);
                this.Success(string.Format("Registro alterado com sucesso!"),
                                                            true);
                return RedirectToAction("Index");
            }
            else
            {
                this.Danger(string.Format("Falha ao Alterar Registro! Verifique se os campos estão preenchidos."));
            }
            return View("~/Views/Sistema/Usuario/Create.cshtml", usuario);



        }

        //
        // POST: /Usuario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
            Usuario usuario = new Usuario();
            usuario = _usuarioDAO.LoadById(id);

            if (usuario != null)
            {
                _usuarioDAO.Delete(usuario);
                this.Success(string.Format("Registro Excluído com sucesso!"), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("Registro NÃO Excluído, tente novamente!"), true);
            return RedirectToAction("Index");
        }

        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }
        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }
        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }
    }
}