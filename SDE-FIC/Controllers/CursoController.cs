using SDE_FIC.DAO;
using SDE_FIC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SDE_FIC.Util;

namespace SDE_FIC.Controllers
{
    public class CursoController : Controller
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
        // GET: /Curso/

        public ActionResult Index(int? pagina, string strCriterioCurso)
        {
            VerificarSessao();

            int tamanhoPagina = 15;
            int numeroPagina = pagina ?? 1;

            CursoDAO _cursoDAO = new CursoDAO(ref _db);
            List<Curso> lCurso = new List<Curso>();

            if (strCriterioCurso == null)
            {
                lCurso = _cursoDAO.All().OrderBy(x => x.CursoNome.ToUpper()).ToList();
            }
            else
            {
                lCurso = _cursoDAO.All().Where(x => x.CursoNome.ToUpper().Contains(strCriterioCurso.ToUpper())).OrderBy(x => x.CursoNome.ToUpper()).ToList();
            }
            return View("~/Views/Sistema/Curso/Index.cshtml", lCurso.ToPagedList(numeroPagina, tamanhoPagina));
        }

        //
        // GET: /Curso/Details/5

        public ActionResult Details(int id = 0)
        {
            VerificarSessao();
            CursoDAO _cursoDAO = new CursoDAO(ref _db);
            Curso _curso = new Curso();
            _curso = _cursoDAO.LoadById(id);

            if (_curso == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Sistema/Curso/Details.cshtml", _curso);
        }

        //
        // GET: /Curso/Create

        public ActionResult Create()
        {
            VerificarSessao();
            return View("~/Views/Sistema/Curso/Create.cshtml");
        }

        //
        // POST: /Curso/Create

        [HttpPost]
        public ActionResult Create(Curso curso)
        {

           
            if (ModelState.IsValid)
            {
                CursoDAO _cursoDAO = new CursoDAO(ref _db);
                _cursoDAO.Insert(curso);
                this.Success(string.Format("Registro salvo com sucesso!"), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("Falha ao Cadastrar Registros! Verifique se os campos estão preenchidos."));
            return View("~/Views/Sistema/Curso/Create.cshtml", curso);
        }

        
         // GET: /Curso/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VerificarSessao();
            CursoDAO _cursoDAO = new CursoDAO(ref _db);
            Curso _curso = new Curso();

            _curso = _cursoDAO.LoadById(id);

            if (_curso == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Sistema/Curso/Edit.cshtml", _curso);
        }

        //
        // POST: /Curso/Edit/5

        [HttpPost]
        public ActionResult Edit(Curso curso)
        {
            if (ModelState.IsValid)
            {
                CursoDAO _cursoDAO = new CursoDAO(ref _db);
                _cursoDAO.Update(curso);
                this.Success(string.Format("Registro salvo com sucesso!"), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("Falha ao Alterar Registro! Verifique se os campos estão preenchidos."));
            return RedirectToAction("Index");
        }

        //
        // GET: /Curso/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CursoDAO _cursoDAO = new CursoDAO(ref _db);
            Curso curso = new Curso();

            curso = _cursoDAO.LoadById(id);

            if (ModelState.IsValid)
            {
                _cursoDAO.Delete(curso);
                this.Success(string.Format("Registro EXCLUÍDO com sucesso!"), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("Falha ao EXCLUIR Registros!"));
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
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