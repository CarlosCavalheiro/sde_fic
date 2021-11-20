using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDE_FIC.DAO;
using SDE_FIC.Models;
using PagedList;
using SDE_FIC.Util;

namespace SDE_FIC.Controllers
{
    public class FuncionarioController : Controller
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
        // GET: /Funcionario/
        public ActionResult Index(int? pagina, string strCriterio)
        {
            VerificarSessao();

            int tamanhoPagina = 15;
            int numeroPagina = pagina ?? 1;

            FuncionarioDAO funcionarioDAO = new FuncionarioDAO(ref _db);

            List<Funcionario> lFuncionario = new List<Funcionario>();

            ViewBag.listarFuncionario = lFuncionario;

            if (strCriterio == null)
            {
                lFuncionario = funcionarioDAO.All().OrderBy(x => x.NomeCompleto.ToUpper()).ToList();
            }
            else
            {
                lFuncionario = funcionarioDAO.All().Where(x => x.NomeCompleto.ToUpper().Contains(strCriterio.ToUpper())).OrderBy(x => x.NomeCompleto.ToUpper()).ToList();
                ViewBag.strCriterio = strCriterio.ToString();
            }

            return View("~/Views/Sistema/Funcionario/Index.cshtml", lFuncionario.ToPagedList(numeroPagina, tamanhoPagina));
        }

        //
        // GET: /Professor/Details/5

        public ActionResult Details(int id = 0)
        {
            VerificarSessao();

            FuncionarioDAO funcionarioDAO = new FuncionarioDAO(ref _db);
            Funcionario _funcionario = new Funcionario();
            _funcionario = funcionarioDAO.LoadById(id);
            if (_funcionario == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Sistema/Funcionario/Details.cshtml", _funcionario);
        }

        //
        // GET: /Funcionario/Create

        public ActionResult Create()
        {
            VerificarSessao();

            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
            ViewBag.usuario_idusuario = new SelectList(_usuarioDAO.All().ToList(), "idusuario", "username");

            return View("~/Views/Sistema/Funcionario/Create.cshtml");
        }

        //
        // POST: /Funcionario/Create

        [HttpPost]
        public ActionResult Create(Funcionario funcionario)
        {
            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);

            if (ModelState.IsValid)
            {               
                FuncionarioDAO _funcionarioDAO = new FuncionarioDAO(ref _db);
                _funcionarioDAO.Insert(funcionario);
                this.Success(string.Format("Registro salvo com sucesso!"), true);
                return RedirectToAction("Index");
            }
            else {
                ViewBag.usuario_idusuario = new SelectList(_usuarioDAO.All().ToList(), "idusuario", "username");
                this.Danger(string.Format("Falha ao Cadastrar Registro! Verifique se os campos estão preenchidos."));
                return View("~/Views/Sistema/Funcionario/Create.cshtml");
            }

            
        }

        //
        // GET: /Funcionario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VerificarSessao();

            FuncionarioDAO _funcionarioDAO = new FuncionarioDAO(ref _db);
            Funcionario _funcionario = new Funcionario();

            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db); 
            Usuario _usuario = new Usuario();

            _funcionario = _funcionarioDAO.LoadById(id);

            if (_funcionario == null)
            {
                return HttpNotFound();
            }
            ViewBag.usuario_idusuario = new SelectList(_usuarioDAO.All().ToList(), "idusuario", "username", _funcionario.Usuario.IdUsuario);
            return View("~/Views/Sistema/Funcionario/Edit.cshtml", _funcionario);
        }

        //
        // POST: /Funcionario/Edit/5
        [HttpPost]
        public ActionResult Edit(Funcionario funcionario)
        {
            FuncionarioDAO _funcionarioDAO = new FuncionarioDAO(ref _db);
            Funcionario _funcionario = new Funcionario();

            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
            Usuario _usuario = new Usuario();

            if (ModelState.IsValid)
            {
                _funcionarioDAO.Update(funcionario);
                this.Success(string.Format("Registro salvo com sucesso!"), true);
                return RedirectToAction("Index");
            }

            //ViewBag.usuario_idusuario = new SelectList(_usuarioDAO.All().ToList(), "idusuario", "username", _funcionario.Usuario.IdUsuario);
            ViewBag.usuario_idusuario = new SelectList(_usuarioDAO.All().ToList(), "idusuario", "username");
            this.Danger(string.Format("Falha ao Cadastrar Registros! Verifique se os campos estão preenchidos."));
            return View("~/Views/Sistema/Funcionario/Edit.cshtml", _funcionario);
        }

       //
        // POST: /Funcionario/Delete/5
        public ActionResult Delete(int id = 0)
        {
            FuncionarioDAO _funcionarioDAO = new FuncionarioDAO(ref _db);
            Funcionario _funcionario = _funcionarioDAO.LoadById(id);
           
            if (_funcionario != null)
            {
                _funcionarioDAO.Delete(_funcionario.IdFuncionario);
                this.Success(string.Format("Registro EXCLUÍDO com sucesso!"), true);
                return RedirectToAction("Index");
            }
            this.Danger(string.Format("Falha ao Excluir Registros!"));
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