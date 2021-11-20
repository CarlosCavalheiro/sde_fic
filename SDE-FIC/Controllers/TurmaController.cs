using SDE_FIC.DAO;
using SDE_FIC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SDE_FIC.Util;

namespace SDE_FIC.Controllers
{
    public class TurmaController : Controller
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
        // GET: /Turma/

        public ActionResult Index(int? pagina, string strCriterioTurma, string strCriterioProfessor, string strCriterioCurso, int strCriterioStatus = 1)
        {
            VerificarSessao();

            int tamanhoPagina = 15;
            int numeroPagina = pagina ?? 1;

            CursoDAO _cursoDAO = new CursoDAO(ref _db);
            
            TurmaDAO turmasDAO = new TurmaDAO(ref _db);            
            FuncionarioDAO funcionarioDAO = new FuncionarioDAO(ref _db);
            Funcionario _funcionario = new Funcionario();

            List<Turma> lTurma = new List<Turma>();

            ViewBag.strCriterioTurma = strCriterioTurma;
            ViewBag.strCriterioCurso = strCriterioCurso;
            ViewBag.strCriterioProfessor = strCriterioProfessor;

            if (Session["LogedUserPerfil"].ToString() != "professor")
            {

                lTurma = turmasDAO.All().OrderByDescending(x => x.Idturma).ToList();

                if (strCriterioStatus == 1)
                {
                    lTurma = lTurma.Where(x => x.Status == true).ToList();
                }
                else if (strCriterioStatus == 0)
                {
                    lTurma = lTurma.Where(x => x.Status == false).ToList();
                }

                if (strCriterioProfessor != null)
                    lTurma = lTurma.Where(x => x.Funcionario.NomeCompleto.ToUpper().Contains(strCriterioProfessor.ToUpper())).ToList();

                if (strCriterioCurso != null)
                    lTurma = lTurma.Where(x => x.Curso.CursoNome.ToUpper().Contains(strCriterioCurso.ToUpper())).ToList();

                if (strCriterioTurma != null)
                    lTurma = lTurma.Where(x => x.Descricao.ToUpper().Contains(strCriterioTurma.ToUpper())).ToList();

                return View("~/Views/Sistema/Turma/Index.cshtml", lTurma.ToPagedList(numeroPagina, tamanhoPagina));
            }
            else 
            {
                var NomeCompleto = Session["LogedUserNomeCompleto"].ToString();
                _funcionario.IdFuncionario = funcionarioDAO.LoadByNome(NomeCompleto).IdFuncionario;

                lTurma = turmasDAO.LoadByFuncionario(ref _funcionario, true).OrderByDescending(x => x.Idturma).ToList();

                //lTurma = turmasDAO.All().OrderByDescending(x => x.Idturma).ToList();

                if (strCriterioStatus == 1)
                {
                    lTurma = lTurma.Where(x => x.Status == true).ToList();
                }
                else if (strCriterioStatus == 0)
                {
                    lTurma = lTurma.Where(x => x.Status == false).ToList();
                }

                if (strCriterioProfessor != null)
                    lTurma = lTurma.Where(x => x.Funcionario.NomeCompleto.ToUpper().Contains(strCriterioProfessor.ToUpper())).ToList();

                if (strCriterioCurso != null)
                    lTurma = lTurma.Where(x => x.Curso.CursoNome.ToUpper().Contains(strCriterioCurso.ToUpper())).ToList();

                if (strCriterioTurma != null)
                    lTurma = lTurma.Where(x => x.Descricao.ToUpper().Contains(strCriterioTurma.ToUpper())).ToList();


                if (lTurma == null)
                {
                    return View("~/Views/Sistema/Turma/Index.cshtml");
                }
                else {
                    return View("~/Views/Sistema/Turma/Index.cshtml", lTurma.OrderBy(x => x.Descricao).ToPagedList(numeroPagina, tamanhoPagina));
                }
                
            }

        }

        //
        // GET: /Turma/Details/5

        public ActionResult Details(int Id = 0)
        {
            VerificarSessao();
            TurmaDAO turmasDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();
            _turma = turmasDAO.LoadById(Id);

            if (_turma == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Sistema/Turma/Details.cshtml", _turma);
        }

        //
        // GET: /Turma/Create

        public ActionResult Create()
        {
            VerificarSessao();

            CursoDAO _cursoDAO = new CursoDAO(ref _db);

            FuncionarioDAO _funcionarioDAO = new FuncionarioDAO(ref _db);

            ViewBag.curso_idcurso = new SelectList(_cursoDAO.All().OrderBy(x => x.CursoNome.ToUpper()), "idcurso", "cursonome");
            ViewBag.funcionario_idfuncionario = new SelectList(_funcionarioDAO.All().Where(x => x.Usuario.Perfil == "professor").OrderBy(x => x.NomeCompleto.ToUpper()), "idfuncionario", "nomecompleto");
            return View("~/Views/Sistema/Turma/Create.cshtml");

        }

        //
        // POST: /Turma/Create

        [HttpPost]
        public ActionResult Create(Turma turma)
        {
            CursoDAO _cursoDAO = new CursoDAO(ref _db);
            FuncionarioDAO _funcionarioDAO = new FuncionarioDAO(ref _db);
            turma.Curso = _cursoDAO.LoadById(turma.Curso.IdCurso);
            turma.Funcionario = _funcionarioDAO.LoadById(turma.Funcionario.IdFuncionario);
            if (turma.Funcionario != null)
            {
                string NomeCompletoFuncionario = turma.Funcionario.NomeCompleto;
                turma.Funcionario.NomeCompleto = NomeCompletoFuncionario;
            }

            if (ModelState.IsValid)
            {
                TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
                _turmaDAO.Insert(turma);
                this.Success(string.Format("Turma Cadastrada com sucesso."), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("ERRO ao Incluir cadastro! Tente novamente."));
            ViewBag.curso_idcurso = new SelectList(_cursoDAO.All().OrderBy(x => x.CursoNome.ToUpper()), "idcurso", "cursonome");
            ViewBag.funcionario_idfuncionario = new SelectList(_funcionarioDAO.All().OrderBy(x => x.NomeCompleto.ToUpper()), "idfuncionario", "nomecompleto"); 
            return View("~/Views/Sistema/Turma/Create.cshtml", turma);
        }

        //
        // GET: /Turma/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VerificarSessao();

            TurmaDAO _turmasDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();
            _turma = _turmasDAO.LoadById(id);

            CursoDAO _cursoDAO = new CursoDAO(ref _db);

            FuncionarioDAO _funcionarioDAO = new FuncionarioDAO(ref _db);

            if (_turma == null)
            {
                return HttpNotFound();
            }

            ViewBag.data_inicio = _turma.DataInicio;
            ViewBag.curso_idcurso = new SelectList(_cursoDAO.All().ToList().OrderBy(x => x.CursoNome.ToUpper()), "idcurso", "cursonome", _turma.Curso.IdCurso);
            ViewBag.funcionario_idfuncionario = new SelectList(_funcionarioDAO.All().OrderBy(x => x.NomeCompleto.ToUpper()).Where(x => x.Usuario.Perfil == "professor").ToList(), "idfuncionario", "nomecompleto", _turma.Funcionario.IdFuncionario);
            
            return View("~/Views/Sistema/Turma/Edit.cshtml", _turma);
        }

        //
        // POST: /Turma/Edit/5

        [HttpPost]
        public ActionResult Edit(Turma turma)
        {
            CursoDAO _cursoDAO = new CursoDAO(ref _db);

            FuncionarioDAO _funcionarioDAO = new FuncionarioDAO(ref _db);

            TurmaDAO _turmasDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            if (ModelState.IsValid)
            {
                _turmasDAO.Update(turma);
                this.Success(string.Format("Turma Alterada com sucesso."), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("ERRO Turma não cadastrada! Tente novamente."));
            ViewBag.curso_idcurso = new SelectList(_cursoDAO.All().ToList().OrderBy(x => x.CursoNome.ToUpper()), "idcurso", "cursonome", _turma.Curso.IdCurso);
            ViewBag.funcionario_idfuncionario = new SelectList(_funcionarioDAO.All().ToList().OrderBy(x => x.NomeCompleto.ToUpper()), "idfuncionario", "nomecompleto", _turma.Funcionario.IdFuncionario);
            
            return View("~/Views/Sistema/Turma/Edit.cshtml", turma);
        }

        public ActionResult Delete(int id = 0)
        {
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = _turmaDAO.LoadById(id);

            CursoDAO _cursoDAO = new CursoDAO(ref _db);

            FuncionarioDAO _funcionarioDAO = new FuncionarioDAO(ref _db);

            if (_turma !=null)
            {
                _turmaDAO.Delete(_turma);

                this.Success(string.Format("Turma EXCLUÍDA com sucesso."), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("ERRO ao EXCLUIR Turma! Tente novamente."));
            ViewBag.curso_idcurso = new SelectList(_cursoDAO.All().ToList(), "idcurso", "cursonome", _turma.Curso.IdCurso);
            ViewBag.funcionario_idfuncionario = new SelectList(_funcionarioDAO.All().ToList(), "idfuncionario", "nomecompleto", _turma.Funcionario.IdFuncionario);

            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    //db.Dispose();
        //    //base.Dispose(disposing);
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