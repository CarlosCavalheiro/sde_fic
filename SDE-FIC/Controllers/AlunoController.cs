using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDE_FIC.DAO;
using SDE_FIC.Models;
using PagedList;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using SDE_FIC.Util;

namespace SDE_FIC.Controllers
{
    public class AlunoController : Controller
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

        public ActionResult ModeloImportarAlunos()
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=\"ModeloImportarAlunos.xlsx\"");
            Response.WriteFile(Server.MapPath("~/Content/Download/ModeloImportarAlunos.xlsx"));
            return View("~/Views/Sistema/Aluno/Importar.cshtml");
        }

        public ActionResult Importar(int? pagina, string strCriterio)
        {
            List<Aluno> lAlunos = new List<Aluno>();
            ViewBag.listarAlunos = lAlunos;
            return View("~/Views/Sistema/Aluno/Importar.cshtml");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase excelfile)
        {
            List<Aluno> lAlunos = new List<Aluno>();            
            ViewBag.listarAlunos = lAlunos;

            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            if (excelfile == null || excelfile.ContentLength == 0)
            {

                TempData["Erro"] = "1";
                TempData["Mensagem"] = "For favor selecione o arquivo em excel";
                return View("~/Views/Sistema/Aluno/Importar.cshtml");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/" + excelfile.FileName);
                    
                    //if (System.IO.File.Exists(path))
                    //    System.IO.File.Delete(path);
                    //excelfile.SaveAs(path);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    excelfile.SaveAs(path);

                    //Lendo o arquivo do Excel
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    
                    for (int Rows = 2; Rows <= range.Rows.Count; Rows++)
                    {
                        Aluno _aluno = new Aluno();
                        _aluno.DataCadastro = DateTime.Now;
                        //_aluno.DataCadastro = Convert.ToDateTime(((Excel.Range)range.Cells[Rows, 1]).Text);
                        _aluno.Nome = ((Excel.Range)range.Cells[Rows, 2]).Text;
                        _aluno.TelefoneResidencial = ((Excel.Range)range.Cells[Rows, 3]).Text;
                        _aluno.TelefoneCelular = ((Excel.Range)range.Cells[Rows, 4]).Text;
                        _aluno.Email = ((Excel.Range)range.Cells[Rows, 5]).Text;
                        _aluno.Observacao = ((Excel.Range)range.Cells[Rows, 6]).Text;
                        _aluno.CPF = ((Excel.Range)range.Cells[Rows, 7]).Text;
                        lAlunos.Add(_aluno);
                        
                        _alunoDAO.Insert(_aluno);

                    }
                    workbook.Save();
                    workbook.Close();
                    
                    ViewBag.listarAlunos = lAlunos;
                    TempData["Erro"] = "0";
                    TempData["Mensagem"] = "Alunos cadastrado com Sucesso!";

                    //ViewBag.Error = "Sucesso!<br>";
                    return View("~/Views/Sistema/Aluno/Importar.cshtml");
                }
                else
                {
                    TempData["Erro"] = "1";
                    TempData["Mensagem"] = "Formato incorreto, selecione um arquivo em formato de Excel";
                    return View("~/Views/Sistema/Aluno/Importar.cshtml");
                }
            }

        }

        //
        // GET: /Aluno/

        public ActionResult Index(int? pagina,  string strCriterio)
        {

            VerificarSessao();
            
            int tamanhoPagina = 15;
            int numeroPagina = pagina ?? 1;

            AlunoDAO alunoDAO = new AlunoDAO(ref _db);
            List<Aluno> lAluno = new List<Aluno>();
            ViewBag.listarAlunos = lAluno;
            if (strCriterio == null)
            {
                lAluno = alunoDAO.All().OrderBy(x => x.Nome.ToUpper()).ToList();                
            }
            else {
                lAluno = alunoDAO.All().Where(x => x.Nome.ToUpper().Contains(strCriterio.ToUpper())).OrderBy(x => x.Nome.ToUpper()).ToList();
                ViewBag.strCriterio = strCriterio.ToString();
            }

            return View("~/Views/Sistema/Aluno/Index.cshtml", lAluno.ToPagedList(numeroPagina, tamanhoPagina));
        }

        //
        // GET: /Aluno/Details/5
        public ActionResult Details(int Id = 0)
        {
            VerificarSessao();

            AlunoDAO alunoDAO = new AlunoDAO(ref _db);
            Aluno _aluno = new Aluno();
            _aluno = alunoDAO.LoadById(Id);
            
            
            if (_aluno == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Sistema/Aluno/Details.cshtml", _aluno);
        }


        //
        // GET: /Aluno/Create
        public ActionResult Create(int id = 0)
        {
            VerificarSessao();
            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            Aluno _aluno = new Aluno();

            if (id == 0) {
                _aluno = null;
                return View("~/Views/Sistema/Aluno/Create.cshtml", _aluno);
            }

            try
            {
                _aluno = _alunoDAO.LoadById(id);
            }
            catch (Exception ex)
            {
                this.Danger(string.Format("Falha ao carregar Registro.<br />", ex.Message));
            }
            finally
            {
                if (_db != null)
                {
                    _db.Dispose();

                }
            }
           
            return View("~/Views/Sistema/Aluno/Create.cshtml", _aluno);
        }

        //
        // POST: /Aluno/Create
        [HttpPost]
        public ActionResult Create(Aluno aluno)
        {

            if (ModelState.IsValid)
            {
                AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
                _alunoDAO.Insert(aluno);
                this.Success(string.Format("Registro salvo com sucesso!"), true);
                return RedirectToAction("Index");
            }
            else
            {
                this.Danger(string.Format("Falha ao Cadastrar Registros! Verifique se os campos estão preenchidos."));
                return View("~/Views/Sistema/Aluno/Create.cshtml", aluno);
            }
        }

        //
        // GET: /Aluno/Edit/5
        public ActionResult Edit(int id = 0)
        {
            VerificarSessao();

            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            Aluno _aluno = new Aluno();

            _aluno = _alunoDAO.LoadById(id);

            if (_aluno == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Sistema/Aluno/Edit.cshtml", _aluno);
        }

        //
        // POST: /Aluno/Edit/5
        [HttpPost]
        public ActionResult Edit(Aluno aluno)
        {
            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            Aluno _aluno = new Aluno();

            if (ModelState.IsValid)
            {
                _alunoDAO.Update(aluno);
                this.Success(string.Format("Registro alterado com sucesso!"), true);
                return RedirectToAction("Index");
            }
            else
            {
                this.Danger(string.Format("Registro Não foi Salvo, verifique se os campos estão preechidos!"), true);
                return View("~/Views/Sistema/Aluno/Edit.cshtml", _aluno);
            }
        }

        public ActionResult Delete(int id = 0)
        {
            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            Aluno _aluno = new Aluno();
            _aluno = _alunoDAO.LoadById(id);

            if (_aluno != null)
            {
                _alunoDAO.Delete(_aluno.IdAluno);
                this.Success(string.Format("Registro Excluído com sucesso!"), true);
                return RedirectToAction("Index");
            }
            else
            {
                this.Danger(string.Format("Registro NÃO foi excluído. Tente novamente!"), true);
                return RedirectToAction("Index");
            }
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