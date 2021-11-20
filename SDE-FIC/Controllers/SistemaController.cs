using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDE_FIC.DAO;
using SDE_FIC.Models;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using SDE_FIC.Util;

namespace SDE_FIC.Controllers
{
        public class SistemaController : Controller
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
            // GET: /Sistema/

            public ActionResult Index()
            {
                VerificarSessao();

                int hora = DateTime.Now.Hour;
                ViewBag.Data = DateTime.Now.Date.ToString("D");
                ViewBag.Saudacao = hora < 12 ? "Bom dia" : hora < 18 ? "Boa tarde" : "Boa Noite";

                //TURMA
                TurmaDAO turmasDAO = new TurmaDAO(ref _db);
                List<Turma> lTurma = new List<Turma>();
                FuncionarioDAO funcionarioDAO = new FuncionarioDAO(ref _db);
                Funcionario _funcionario = new Funcionario();


                if (Session["LogedUserPerfil"].ToString() != "professor")
                {
                    lTurma = turmasDAO.All().ToList();

                    //Diario
                    ViewBag.Turma = lTurma.Count();
                    ViewBag.DiariosAtivo = lTurma.Where(x => x.Status == true).Count();
                    ViewBag.DiariosFinalizado = lTurma.Where(x => x.Status == false).Count();
                }
                else
                {
                    _funcionario.IdFuncionario = funcionarioDAO.LoadByNome(Session["LogedUserNomeCompleto"].ToString()).IdFuncionario;
                    lTurma = turmasDAO.LoadByFuncionario(ref _funcionario, true).ToList();


                    //Diario
                    ViewBag.Turma = lTurma.Count();
                    ViewBag.DiariosAtivo = lTurma.Where(x => x.Status == true).Count();
                    ViewBag.DiariosFinalizado = lTurma.Where(x => x.Status == false).Count();
                }

                //Usuario
                UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
                ViewBag.Usuario = _usuarioDAO.All().Count();


                return View();
            }


            public ActionResult BaixarCalendario2015()
            {
                Response.AddHeader("Content-Disposition", "attachment; filename=\"Calendario2015.xls\"");
                Response.WriteFile(Server.MapPath("~/Content/Download/Calendario2015.xls"));
                return View("Index");
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
