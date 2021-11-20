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
    public class UnidadeCurricularController : Controller
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

        public ActionResult Index(int? pagina, string strCriterioCurso)
        {

            VerificarSessao();
            int tamanhoPagina = 15;
            int numeroPagina = pagina ?? 1;

            UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);
            
            List<UnidadeCurricular> lUnidadeCurricular = new List<UnidadeCurricular>();
            if (strCriterioCurso == null)
            {
                lUnidadeCurricular = _unidadecurricularDAO.All().OrderBy(x => x.Curso.CursoNome.ToUpper()).ToList();
            }
            else
            {
                lUnidadeCurricular = _unidadecurricularDAO.All().Where(x => x.Curso.CursoNome.ToUpper().Contains(strCriterioCurso.ToUpper())).OrderBy(x => x.Curso.CursoNome.ToUpper()).ToList();
            }

            return View("~/Views/Sistema/UnidadeCurricular/Index.cshtml", lUnidadeCurricular.ToPagedList(numeroPagina, tamanhoPagina));
        }

        //
        // GET: /UnidadeCurricular/Details/5

        public ActionResult Details(int id = 0)
        {
            VerificarSessao();
            UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);
            UnidadeCurricular _unidadecurricular = new UnidadeCurricular();

            _unidadecurricular = _unidadecurricularDAO.LoadById(id);

            if (_unidadecurricular == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Sistema/UnidadeCurricular/Details.cshtml", _unidadecurricular);
        }

        //
        // GET: /UnidadeCurricular/Create

        public ActionResult Create()
        {
            VerificarSessao();

            CursoDAO _cursoDAO = new CursoDAO(ref _db);

            ViewBag.curso_idcurso = new SelectList(_cursoDAO.All().ToList().OrderBy(x => x.CursoNome.ToUpper()), "idcurso" , "cursonome".ToUpper());

            return View("~/Views/Sistema/UnidadeCurricular/Create.cshtml");
        }

        //
        // POST: /UnidadeCurricular/Create

        [HttpPost]
        public ActionResult Create(UnidadeCurricular unidadecurricular)
            {
                CursoDAO _cursoDAO = new CursoDAO(ref _db);


                if (ModelState.IsValid)
                {
                    UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);
                    _unidadecurricularDAO.Insert(unidadecurricular);

                    this.Success(string.Format("Unidade Curricular Cadastrada com sucesso!"), true);
                    return RedirectToAction("Index");
                }
                this.Danger(string.Format("Registro NÃO Cadastrado, tente novamente!"), true);
                ViewBag.curso_idcurso = new SelectList(_cursoDAO.All().ToList().OrderBy(x => x.CursoNome), "idcurso", "cursonome", unidadecurricular.Curso.IdCurso);
                return View("~/Views/Sistema/UnidadeCurricular/Create.cshtml", unidadecurricular);
            }

        //
        // GET: /UnidadeCurricular/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VerificarSessao();

            UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);
            UnidadeCurricular _unidadecurricular = new UnidadeCurricular();

            CursoDAO _cursoDAO = new CursoDAO(ref _db);
            Curso _curso = new Curso();
            
            _unidadecurricular = _unidadecurricularDAO.LoadById(id);

            if (_unidadecurricular == null)
            {
                return HttpNotFound();
            }
            ViewBag.curso_idcurso = new SelectList(_cursoDAO.All().ToList().OrderBy(x => x.CursoNome), "idcurso", "cursonome", _unidadecurricular.Curso.IdCurso);
            return View("~/Views/Sistema/UnidadeCurricular/Edit.cshtml", _unidadecurricular);
        }

        //
        // POST: /UnidadeCurricular/Edit/5

        [HttpPost]
        public ActionResult Edit(UnidadeCurricular unidadecurricular)
        {
            CursoDAO _cursoDAO = new CursoDAO(ref _db);
            UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);
            UnidadeCurricular _unidadecurricular = new UnidadeCurricular();

            if (ModelState.IsValid)
            {
                _unidadecurricularDAO.Update(unidadecurricular);
                this.Success(string.Format("Registro Alterado com sucesso!"), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("Registro NÃO Alterado, tente novamente!"), true);
            ViewBag.curso_idcurso = new SelectList(_cursoDAO.All().ToList().OrderBy(x => x.CursoNome), "idcurso", "cursonome", _unidadecurricular.Curso.IdCurso);
            return View("~/Views/Sistema/UnidadeCurricular/Edit.cshtml", _unidadecurricular);
        }


        // POST: /UnidadeCurricular/Delete/5
        public ActionResult Delete(int id = 0)
        {
            UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);
            UnidadeCurricular _unidadecurricular = new UnidadeCurricular();
            _unidadecurricular = _unidadecurricularDAO.LoadById(id);

            if (_unidadecurricular != null)
            {
                _unidadecurricularDAO.Delete(_unidadecurricular);
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