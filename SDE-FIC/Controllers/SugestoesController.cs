using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDE_FIC.DAO;
using SDE_FIC.Models;
using PagedList;
using SDE_FIC.Util;

namespace SDE_FIC.Controllers
{
    public class SugestoesController : Controller
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
        // GET: /UnidadeCurricular/

        public ActionResult Index(int? pagina)
        {
            VerificarSessao();

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            CursoDAO _cursoDAO = new CursoDAO(ref _db);

            SugestoesDAO _sugestoesDAO = new SugestoesDAO(ref _db);

            return View("~/Views/Sistema/Sugestoes/Index.cshtml", _sugestoesDAO.All().ToPagedList(numeroPagina, tamanhoPagina));
        }

 
        //
        // GET: /UnidadeCurricular/Create

        public ActionResult Create(int id = 0)
        {
            VerificarSessao();

            id = Convert.ToInt32(Session["LogedUserId"].ToString());

            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
            Usuario usuario = new Usuario();
            usuario = _usuarioDAO.LoadById(id);

            ViewBag.usuario_idusuario = new SelectList(_usuarioDAO.All().ToList(), "idusuario", "username");

            Sugestoes sugestoes = new Sugestoes();
            sugestoes.Usuario = usuario;

            return View("~/Views/Sistema/Sugestoes/Create.cshtml", sugestoes);
        }

        //
        // POST: /UnidadeCurricular/Create

        [HttpPost]
        public ActionResult Create(Sugestoes sugestoes)
            {
                SugestoesDAO _sugestoesDAO = new SugestoesDAO(ref _db);
                UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
                sugestoes.Usuario = _usuarioDAO.LoadById(sugestoes.IdUsuario);

                if (ModelState.IsValid)
                {
                    _sugestoesDAO.Insert(sugestoes);

                    this.Success(string.Format("Sugestão Cadastrada com sucesso!"), true);
                    return RedirectToAction("Index");
                }
    
                this.Danger(string.Format("Erro ao cadastrar, tente novamente!"), true);
                ViewBag.usuario_idusuario = new SelectList(_usuarioDAO.All().ToList(), "idusuario", "username");
                return View("~/Views/Sistema/Sugestoes/Create.cshtml", sugestoes);
            }

        //
        // GET: /UnidadeCurricular/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VerificarSessao();

            SugestoesDAO _sugestoesDAO = new SugestoesDAO(ref _db);
            Sugestoes _sugestoes = new Sugestoes();

            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
            Usuario _usuario = new Usuario();
            
            _sugestoes = _sugestoesDAO.LoadById(id);

            if (_sugestoes == null)
            {
                return HttpNotFound();
            }
            ViewBag.usuario_idusuario = new SelectList(_usuarioDAO.All().ToList(), "idusuario", "username", _sugestoes.Usuario.IdUsuario);
            return View("~/Views/Sistema/Sugestoes/Edit.cshtml", _sugestoes);
        }

        //
        // POST: /UnidadeCurricular/Edit/5

        [HttpPost]
        public ActionResult Edit(Sugestoes sugestoes)
        {
            SugestoesDAO _sugestoesDAO = new SugestoesDAO(ref _db);
            Sugestoes _sugestoes = new Sugestoes();
            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
            Usuario _usuario = new Usuario();

            if (ModelState.IsValid)
            {
                _sugestoesDAO.Update(sugestoes);
                this.Success(string.Format("Registro Alterado com sucesso!"), true);
                return RedirectToAction("Index");
            }
            TempData["Erro"] = "1";
            this.Success(string.Format("Sugestão não alterada, tente novamente!"), true);
            ViewBag.usuario_idusuario = new SelectList(_usuarioDAO.All().ToList(), "idusuario", "username", sugestoes.Usuario.IdUsuario);
            return View("~/Views/Sistema/Sugestoes/Edit.cshtml", _sugestoes);
        }



        public ActionResult Delete(Sugestoes sugestoes)
        {
            SugestoesDAO _sugestoesDAO = new SugestoesDAO(ref _db);
            Sugestoes _sugestoes = new Sugestoes();
            _sugestoes = _sugestoesDAO.LoadById(sugestoes.IdSugestao);

            if (_sugestoes != null)
            {
                _sugestoesDAO.Delete(sugestoes.IdSugestao);

                this.Success(string.Format("Registro Excluído com sucesso!"), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("Erro ao Excluir, tente novamente!"), true);
            return View("~/Views/Sistema/Sugestoes/Delete.cshtml", sugestoes);
        }

        //    protected override void Dispose(bool disposing)
        //    {
        //        db.Dispose();
        //        base.Dispose(disposing);
        //    }
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