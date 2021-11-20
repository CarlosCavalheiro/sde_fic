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
using System.Data.OleDb;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SDE_FIC.Controllers
{
    public class MatriculaController : Controller
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
                var NomeCompleto = Session["LogedUserNomeCompleto"].ToString();
            }
            return View();
        }

        public ActionResult ModeloImportarAlunos(int idTurma)
        {
            VerificarSessao();
            Turma turma = new Turma();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

            turma = _turmaDAO.LoadById(idTurma);

            ViewBag.turma_descricao = turma.Descricao + " | " + turma.Curso.CursoNome;

            ViewBag.turma_idturma = turma.Idturma;


            List<Aluno> lAlunos = new List<Aluno>();
            ViewBag.listarAlunos = lAlunos;
            
            //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + strNomeArquivo);
            //HttpContext.Current.Response.ContentType = "APPLICATION/MS-EXCEL";


            //Response.AddHeader("Content-Length", "ModeloImportarAlunos.xlsx".Length.ToString());
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.AddHeader("Content-Disposition", "attachment; filename=\"ModeloImportarAlunos.xls\"");
            Response.WriteFile(Server.MapPath("~/Content/Download/ModeloImportarAlunos.xls"));

            return View("~/Views/Sistema/Matricula/Importar.cshtml");
        }

        public ActionResult Importar(int id = 0)
        {
            VerificarSessao();
            Turma turma = new Turma();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

            Matricula _matricula = new Matricula();

            turma = _turmaDAO.LoadById(id);

            ViewBag.turma_descricao = turma.Descricao + " | " + turma.Curso.CursoNome;

            ViewBag.turma_idturma = turma.Idturma;

            _matricula.Turma = turma;

            ViewBag.listarAlunos = null;
            ViewBag.listarAlunosErro = null;

            List<Aluno> lAlunos = new List<Aluno>();

            return View("~/Views/Sistema/Matricula/Importar.cshtml", _matricula);
        }

        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase excelfile, int idTurma, Matricula matricula)
        //{
        //    VerificarSessao();
        //    List<Aluno> lAlunos = new List<Aluno>();
        //    ViewBag.listarAlunos = lAlunos;

        //    AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
        //    if (excelfile == null || excelfile.ContentLength == 0)
        //    {

        //        TempData["Erro"] = "1";
        //        TempData["Mensagem"] = "For favor selecione o arquivo em excel";
        //        return View("~/Views/Sistema/Matricula/Importar.cshtml");
        //    }
        //    else
        //    {
        //        if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
        //        {
        //            string path = Server.MapPath("~/Content/Arquivos/Temp/" + excelfile.FileName);

        //            //if (System.IO.File.Exists(path))
        //            //    System.IO.File.Delete(path);
        //            //excelfile.SaveAs(path);

        //            if (System.IO.File.Exists(path))
        //            {
        //                System.IO.File.Delete(path);
        //            }
        //            excelfile.SaveAs(path);

        //            //Lendo o arquivo do Excel
        //            Excel.Application application = new Excel.Application();
        //            Excel.Workbook workbook = application.Workbooks.Open(path);
        //            Excel.Worksheet worksheet = workbook.ActiveSheet;
        //            Excel.Range range = worksheet.UsedRange;

        //            //Matricula _matricula = new Matricula();
        //            //List<Matricula> lMatriculas = new List<Matricula>();
        //            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
        //            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);

        //            for (int Rows = 2; Rows <= range.Rows.Count; Rows++)
        //            {
        //                Aluno _aluno = new Aluno();
        //                _aluno.DataCadastro = DateTime.Now;
        //                //_aluno.DataCadastro = Convert.ToDateTime(((Excel.Range)range.Cells[Rows, 1]).Text);
        //                _aluno.Nome = ((Excel.Range)range.Cells[Rows, 2]).Text;
        //                _aluno.TelefoneResidencial = ((Excel.Range)range.Cells[Rows, 3]).Text;
        //                _aluno.TelefoneCelular = ((Excel.Range)range.Cells[Rows, 4]).Text;
        //                _aluno.Email = ((Excel.Range)range.Cells[Rows, 5]).Text;
        //                _aluno.Observacao = ((Excel.Range)range.Cells[Rows, 6]).Text;
        //                _aluno.CPF = ((Excel.Range)range.Cells[Rows, 7]).Text;
        //                lAlunos.Add(_aluno);

        //                _alunoDAO.Insert(_aluno);
                        
        //                matricula.Turma = _turmaDAO.LoadById(idTurma);
        //                matricula.Aluno = _alunoDAO.LoadById(_aluno.IdAluno);
        //                matricula.Situacao = "Ativo";
        //                matricula.DataMatricula = DateTime.Now;
        //                matricula.DataSituacao = DateTime.Now;

        //                if (ModelState.IsValid)
        //                {
        //                    _matriculaDAO.Insert(matricula);
        //                    //TempData["Erro"] = "0";
        //                    //TempData["Mensagem"] = "Matricula Realizada com Sucesso!";
        //                    //return RedirectToAction("Create", new { idTurma = matricula.Turma.Idturma });
        //                }

                        
        //            }
        //            workbook.Save();
        //            workbook.Close();

        //            ViewBag.listarAlunos = lAlunos;
        //            TempData["Erro"] = "0";
        //            TempData["Mensagem"] = "Alunos cadastrado com Sucesso!";

        //            //ViewBag.Error = "Sucesso!<br>";
        //            return View("~/Views/Sistema/Matricula/Importar.cshtml");
        //        }
        //        else
        //        {
        //            TempData["Erro"] = "1";
        //            TempData["Mensagem"] = "Formato incorreto, selecione um arquivo em formato de Excel";
        //            return View("~/Views/Sistema/Matricula/Importar.cshtml");
        //        }
        //    }

        //}


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase excelfile, int idTurma, Matricula matricula)
        {
            //ConnectionUtil cnn;

            VerificarSessao();
            List<Aluno> lAlunos = new List<Aluno>();
            ViewBag.listarAlunos = lAlunos;
            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);

            DataSet ds = new DataSet();

            if (excelfile == null || excelfile.ContentLength == 0)
            {
                //TempData["Erro"] = "1";
                //TempData["Mensagem"] = "For favor selecione o arquivo em excel";
                this.Danger(string.Format("For favor selecione o arquivo em excel."));
                return View("~/Views/Sistema/Matricula/Importar.cshtml", matricula);
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {


                    try 
                    {
                        string path = Server.MapPath("~/Content/Arquivos/Temp/" + excelfile.FileName);

                        //Se o arquivo existir exlui e adiciona o novo
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        excelfile.SaveAs(path);

                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                        //Cria a Conexão string
                        if (excelfile.FileName.EndsWith("xls"))
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        else if (excelfile.FileName.EndsWith("xlsx"))
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }

                        //Cria a conexão com o arquivo
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();


                        //Lendo o arquivo do Excel
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }
                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;

                        //Salva cada linha em um temporario
                        foreach (DataRow Rows in dt.Rows)
                        {
                            excelSheets[t] = Rows["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                        string query = string.Format("Select * from [{0}]", excelSheets[0]);

                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }

                        //Instanciar Conexao


                        ////////////////////////////////////////////////////////////////////////////////////////////
                        //Executa a Importação///////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////                    
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Aluno _aluno = new Aluno();
                            _aluno.DataCadastro = DateTime.Now;
                            //_aluno.DataCadastro = Convert.ToDateTime(((Excel.Range)range.Cells[Rows, 1]).Text);
                            //ds.Tables[0].Rows[i][0].ToString();
                            _aluno.DataCadastro = DateTime.Now; ;
                            _aluno.Nome = ds.Tables[0].Rows[i][0].ToString();
                            _aluno.TelefoneResidencial = ds.Tables[0].Rows[i][1].ToString();
                            _aluno.TelefoneCelular = ds.Tables[0].Rows[i][2].ToString();
                            _aluno.Email = ds.Tables[0].Rows[i][3].ToString();
                            _aluno.Observacao = ds.Tables[0].Rows[i][4].ToString();
                            _aluno.CPF = ds.Tables[0].Rows[i][5].ToString();

                            //Adiciona Aluno em uma Lista
                            lAlunos.Add(_aluno);

                            //Adiciona Aluno no Cadastro de Aluno
                            _alunoDAO.Insert(_aluno);

                            matricula.Turma = _turmaDAO.LoadById(idTurma);
                            matricula.Aluno = _alunoDAO.LoadById(_aluno.IdAluno);
                            matricula.Situacao = "Ativo";
                            matricula.DataMatricula = DateTime.Now;
                            matricula.DataSituacao = DateTime.Now;

                            //Realizar a Matricula do Aluno
                            _matriculaDAO.Insert(matricula);

                        }
                        excelConnection.Close();
                        ViewBag.listarAlunos = lAlunos.ToList();

                        //TempData["Erro"] = "0";
                        //TempData["Mensagem"] = "Alunos cadastrado com Sucesso!";
                        this.Success("Alunos cadastrado com Sucesso!");

                       

                    } catch (Exception ex)
                    {
                        this.Danger("ERRO Cadastrado. Verifique se todos os campos foram preenchidos na tabela!" + ex.Message);

                    }

                    return View("~/Views/Sistema/Matricula/Importar.cshtml", matricula);
                }
                else
                {
                    //TempData["Erro"] = "1";
                    //TempData["Mensagem"] = "Erro de arquivo. Escolha um arquivo, conforme modelo!";
                    this.Danger("Erro de arquivo. Escolha um arquivo, conforme modelo!");

                    return View("~/Views/Sistema/Matricula/Importar.cshtml", matricula);
                }

            }
              
        }

        private string GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            if (cell == null)
            {
                return null;
            }

            var value = cell.CellFormula != null
                ? cell.CellValue.InnerText
                : cell.InnerText.Trim();

            // If the cell represents an integer number, you are done. 
            // For dates, this code returns the serialized value that 
            // represents the date. The code handles strings and 
            // Booleans individually. For shared strings, the code 
            // looks up the corresponding value in the shared string 
            // table. For Booleans, the code converts the value into 
            // the words TRUE or FALSE.
            if (cell.DataType == null)
            {
                return value;
            }
            switch (cell.DataType.Value)
            {
                case CellValues.SharedString:

                    // For shared strings, look up the value in the
                    // shared strings table.
                    var stringTable =
                        workbookPart.GetPartsOfType<SharedStringTablePart>()
                            .FirstOrDefault();

                    // If the shared string table is missing, something 
                    // is wrong. Return the index that is in
                    // the cell. Otherwise, look up the correct text in 
                    // the table.
                    if (stringTable != null)
                    {
                        value =
                            stringTable.SharedStringTable
                                .ElementAt(int.Parse(value)).InnerText;
                    }
                    break;

                case CellValues.Boolean:
                    switch (value)
                    {
                        case "0":
                            value = "FALSE";
                            break;
                        default:
                            value = "TRUE";
                            break;
                    }
                    break;
            }
            return value;
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase excelfile, int idTurma, Matricula matricula)
        {
            //ConnectionUtil cnn;

            VerificarSessao();
            List<Aluno> lAlunos = new List<Aluno>();
            List<Aluno> lAlunosErro = new List<Aluno>();

            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            //Matricula matricula = new Matricula();
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);

            //Carrega dados da turma
            Turma turma = new Turma();
            turma = _turmaDAO.LoadById(idTurma);

            //Carrega dado da turma dentro da maticula
            matricula.Turma = turma;
            ViewBag.turma_descricao = turma.Descricao + " | " + turma.Curso.CursoNome;
            ViewBag.turma_idturma = turma.Idturma;
           
            DataSet ds = new DataSet();

            if (excelfile == null || excelfile.ContentLength == 0)
            {
                //TempData["Erro"] = "1";
                //TempData["Mensagem"] = "For favor selecione o arquivo em excel";
                this.Danger(string.Format("For favor selecione o arquivo em excel."));
                return View("~/Views/Sistema/Matricula/Importar.cshtml", matricula);
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {


                    try
                    {
                        string path = Server.MapPath("~/Content/Arquivos/Temp/" + excelfile.FileName);

                        //Se o arquivo existir exlui e adiciona o novo
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        excelfile.SaveAs(path);


                        //Lendo o arquivo do Excel
                        SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(path, false);
                        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();


                        foreach (var row in sheetData.Elements<Row>())
                        {
                            Aluno _aluno = new Aluno();
                            List<string> item = new List<string>();

                            foreach (var cell in row.Elements<Cell>())
                            {
                                item.Add(GetCellValue(cell, workbookPart).ToString());
                            }
                            _aluno.DataCadastro = DateTime.Now;
                            _aluno.Nome = item[0];
                            _aluno.TelefoneResidencial = item[1];
                            _aluno.TelefoneCelular = item[2];
                            _aluno.Email = item[3];
                            _aluno.Observacao = item[4];
                            _aluno.CPF = item[5];

                            //Adiciona Aluno em uma Lista
                            if (_aluno.Nome == "")
                            {
                                lAlunosErro.Add(_aluno);

                            }
                            else
                            {
                                lAlunos.Add(_aluno);

                                //Adiciona Aluno no Cadastro de Aluno
                                _alunoDAO.Insert(_aluno);

                                matricula.Turma = _turmaDAO.LoadById(idTurma);
                                matricula.Aluno = _alunoDAO.LoadById(_aluno.IdAluno);
                                matricula.Situacao = "Ativo";
                                matricula.DataMatricula = DateTime.Now;
                                matricula.DataSituacao = DateTime.Now;

                                //Realizar a Matricula do Aluno
                                _matriculaDAO.Insert(matricula);

                            }
                        }


                        //Excel.Application application = new Excel.Application();
                        //Excel.Workbook workbook = application.Workbooks.Open(path);
                        //Excel.Worksheet worksheet = workbook.ActiveSheet;
                        //Excel.Range range = worksheet.UsedRange;

                        //for (int row = 2; row <= range.Rows.Count; row++)
                        //{
                        //    Aluno _aluno = new Aluno();
                        //    _aluno.DataCadastro = DateTime.Now;
                        //    //_aluno.DataCadastro = Convert.ToDateTime(((Excel.Range)range.Cells[Rows, 1]).Text);
                        //    //ds.Tables[0].Rows[i][0].ToString();
                        //    _aluno.DataCadastro = DateTime.Now; ;
                        //    _aluno.Nome = ((Excel.Range)range.Cells[row, 1]).Text;
                        //    _aluno.TelefoneResidencial = ((Excel.Range)range.Cells[row, 2]).Text;
                        //    _aluno.TelefoneCelular = ((Excel.Range)range.Cells[row, 3]).Text;
                        //    _aluno.Email = ((Excel.Range)range.Cells[row, 4]).Text;
                        //    _aluno.Observacao = ((Excel.Range)range.Cells[row, 5]).Text;
                        //    _aluno.CPF = ((Excel.Range)range.Cells[row, 6]).Text;

                        //    //Adiciona Aluno em uma Lista
                        //    if(_aluno.Nome == "")
                        //    {
                        //        lAlunosErro.Add(_aluno);

                        //    }
                        //    else
                        //    {
                        //        lAlunos.Add(_aluno);

                        //        //Adiciona Aluno no Cadastro de Aluno
                        //        _alunoDAO.Insert(_aluno);

                        //        matricula.Turma = _turmaDAO.LoadById(idTurma);
                        //        matricula.Aluno = _alunoDAO.LoadById(_aluno.IdAluno);
                        //        matricula.Situacao = "Ativo";
                        //        matricula.DataMatricula = DateTime.Now;
                        //        matricula.DataSituacao = DateTime.Now;

                        //        //Realizar a Matricula do Aluno
                        //        _matriculaDAO.Insert(matricula);
                        //    }

                        //}
                        ////Fechar arquivo
                        //application.Workbooks.Close();
                        ViewBag.listarAlunos = lAlunos.ToList();
                        ViewBag.listarAlunosErro = lAlunosErro.ToList();


                        this.Success("Alunos cadastrado com Sucesso!");

                    }
                    catch (Exception ex)
                    {
                        this.Danger("ERRO Cadastrado. Verifique se todos os campos foram preenchidos na tabela!" + ex.Message);

                    }

                    return View("~/Views/Sistema/Matricula/Importar.cshtml", matricula);
                }
                else
                {
                    //TempData["Erro"] = "1";
                    //TempData["Mensagem"] = "Erro de arquivo. Escolha um arquivo, conforme modelo!";
                    this.Danger("Erro de arquivo. Escolha um arquivo, conforme modelo!");

                    return View("~/Views/Sistema/Matricula/Importar.cshtml", matricula);
                }

            }

        }

        //
        // GET: /Matricula/

        public ActionResult Index(int? pagina, string strCriterioCurso, string strCriterioTurma, string strCriterioProfessor, int strCriterioStatus = 2)
        {

            VerificarSessao();
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;


            ViewBag.strCriterioTurma = strCriterioTurma;
            ViewBag.strCriterioCurso = strCriterioCurso;
            ViewBag.strCriterioProfessor = strCriterioProfessor;
            ViewBag.strCriterioStatus = strCriterioStatus;

            CursoDAO _cursoDAO = new CursoDAO(ref _db);

            Funcionario _funcionario = new Funcionario();
            FuncionarioDAO funcionarioDAO = new FuncionarioDAO(ref _db);

            Matricula _matricula = new Matricula();
            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);

            Turma _turma = new Turma();
            TurmaDAO turmasDAO = new TurmaDAO(ref _db);
            
            //Criterio Curso
            //List<Curso> lCurso = new List<Curso>();
            List<Turma> lTurma = new List<Turma>();

            //if (strCriterioCurso != null)
            //{
            //    lTurma = turmasDAO.All().Where(x => x.Curso.CursoNome.Contains(strCriterioCurso)).ToList(); 
            //}

            //Criterio Turma

            //if (strCriterioTurma == null)
            //{
            //    if (strCriterioCurso == null)
            //    {
            //        lTurma = turmasDAO.All().OrderBy(x => x.Descricao.ToUpper()).ToList();
            //    }
            //    else
            //    {
            //        lTurma = turmasDAO.All().Where(x => x.Curso.CursoNome.ToUpper().Contains(strCriterioCurso.ToUpper())).OrderBy(x => x.Descricao.ToUpper()).ToList();
            //    }
            //}
            //else
            //{
            //    if (strCriterioCurso == null)
            //    {
            //        lTurma = turmasDAO.All().Where(x => x.Descricao.ToUpper().Contains(strCriterioTurma.ToUpper())).OrderBy(x => x.Descricao.ToUpper()).ToList();
            //    }
            //    else
            //    {
            //        lTurma = turmasDAO.All().Where(x => x.Curso.CursoNome.ToUpper().Contains(strCriterioCurso.ToUpper())).Where(x => x.Descricao.ToUpper().Contains(strCriterioTurma.ToUpper())).OrderBy(x => x.Descricao.ToUpper()).ToList();
            //    }

            //}

            if (strCriterioProfessor == null && strCriterioCurso == null && strCriterioTurma == null && strCriterioStatus == 2)
            {
                lTurma = turmasDAO.All().OrderBy(x => x.Funcionario.NomeCompleto).ToList();
            }
            else
            {

                lTurma = turmasDAO.All().ToList().OrderBy(x => x.Descricao).ToList();

                if (strCriterioStatus == 1)
                {
                    lTurma = lTurma.Where(x => x.Status == true).ToList();
                }
                else if (strCriterioStatus == 0)
                {
                    lTurma = lTurma.Where(x => x.Status == false).ToList();
                }

                //lTurma = lTurma.Where(x => x.Funcionario.NomeCompleto.ToUpper().Contains(strCriterioProfessor.ToUpper())).ToList();
                //lTurma = lTurma.Where(x => x.Curso.CursoNome.ToUpper().Contains(strCriterioCurso.ToUpper())).ToList();
                //lTurma = lTurma.Where(x => x.Descricao.ToUpper().Contains(strCriterioTurma.ToUpper())).ToList();

                if (strCriterioProfessor != null)
                    lTurma = lTurma.Where(x => x.Funcionario.NomeCompleto.ToUpper().Contains(strCriterioProfessor.ToUpper())).ToList();

                if (strCriterioCurso != null)
                    lTurma = lTurma.Where(x => x.Curso.CursoNome.ToUpper().Contains(strCriterioCurso.ToUpper())).ToList();

                if (strCriterioTurma != null)
                    lTurma = lTurma.Where(x => x.Descricao.ToUpper().Contains(strCriterioTurma.ToUpper())).ToList();

            }


            if (Session["LogedUserPerfil"].ToString() != "professor")
            {
                //List<Turma> lTurma = turmasDAO.All().ToList();
                ViewBag.lMatricula = matriculaDAO.All();

                return View("~/Views/Sistema/Matricula/Index.cshtml", lTurma.OrderBy(x => x.Descricao).ToPagedList(numeroPagina, tamanhoPagina));
            }
            else
            {
                var NomeCompleto = Session["LogedUserNomeCompleto"].ToString();
                _funcionario.IdFuncionario = funcionarioDAO.LoadByNome(NomeCompleto).IdFuncionario;           
                lTurma = turmasDAO.LoadByFuncionario(ref _funcionario, true).ToList();
                ViewBag.lMatricula = matriculaDAO.All();

                if (lTurma == null)
                {
                    return View("~/Views/Sistema/Matricula/Index.cshtml");
                }
                else
                {
                    return View("~/Views/Sistema/Matricula/Index.cshtml", lTurma.OrderBy(x => x.Descricao).ToPagedList(numeroPagina, tamanhoPagina));
                }

            }

        }

        public ActionResult Matricular(int id, int? pagina)        
        {
            VerificarSessao();

            int tamanhoPagina = 16;
            int numeroPagina = pagina ?? 1;

            Turma turma = new Turma();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

            turma = _turmaDAO.LoadById(id);

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);

            List<Matricula> lMatricula = matriculaDAO.LoadByTurma(turma).OrderBy(x => x.Aluno.Nome).ToList();
            
            ViewBag.turma_descricao = turma.Descricao + " | " + turma.Curso.CursoNome;

            ViewBag.turma_idturma = turma.Idturma;

            return View("~/Views/Sistema/Matricula/Matricular.cshtml", lMatricula.ToPagedList(numeroPagina, tamanhoPagina));

        }

        public ActionResult Matriculas(int id)
        {
            VerificarSessao();

            Turma turma = new Turma();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

            turma = _turmaDAO.LoadById(id);
            

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);

            turma.ListaMatriculas = matriculaDAO.LoadByTurma(turma).OrderBy(x => x.Aluno.Nome).ToList();          

            //return View("~/Views/Sistema/Matricula/Matriculas.cshtml", matriculaDAO.LoadByTurma(turma).OrderBy(x => x.Aluno.Nome).ToPagedList(numeroPagina, tamanhoPagina));
            return View("~/Views/Sistema/Matricula/Matriculas.cshtml", turma);

        }

        private List<Matricula> GetMatriculas(int idTurma)
        {
            List<Matricula> lMatricula = new List<Matricula>();

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);

            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

            lMatricula = matriculaDAO.LoadByTurma(_turmaDAO.LoadById(idTurma)).OrderBy(x => x.Aluno.Nome.ToUpper()).ToList();

            return lMatricula;
        }

        //
        // GET: /Matricula/Details/5
        public ActionResult Details(int Id = 0)
        {
            VerificarSessao();

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            Matricula _matricula = new Matricula();
            
            _matricula = matriculaDAO.LoadById(Id);

            Turma turma = new Turma();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

            turma = _turmaDAO.LoadById(_matricula.Turma.Idturma);

            ViewBag.turma_descricao = turma.Descricao + " | " + turma.Curso.CursoNome;

            ViewBag.turma_idturma = turma.Idturma;


            if (_matricula == null)
            {             
                return HttpNotFound();
            
            }

            return View("~/Views/Sistema/Matricula/Details.cshtml", _matricula);
        }

        //
        // GET: /Matricula/Create

        public ActionResult Create(int id = 0)
        {
            VerificarSessao();

            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            ViewBag.aluno_idaluno = new SelectList(_alunoDAO.All().OrderBy(x => x.Nome.ToUpper()).ToList(), "idaluno", "nome");

            Turma _turma = new Turma();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            ViewBag.turma_idturma = _turmaDAO.LoadById(id).Idturma;
            ViewBag.turma_descricao = _turmaDAO.LoadById(id).Descricao + " - " + _turmaDAO.LoadById(id).Curso.CursoNome;


            _turma = _turmaDAO.LoadById(id);
 
            return View("~/Views/Sistema/Matricula/Create.cshtml", new Matricula() {listaMatriculas = GetMatriculas(id), Turma = _turma});
        }


        //
        // POST: /Matricula/Create

        [HttpPost]
        public ActionResult Create(Matricula matricula)
        {
            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);
            matricula.Turma = _turmaDAO.LoadById(matricula.IdTurma);
            matricula.Aluno = _alunoDAO.LoadById(matricula.IdAluno);
            
            if (ModelState.IsValid)
            {
                _matriculaDAO.Insert(matricula);
                TempData["Erro"] = "0";
                TempData["Mensagem"] = "Matricula Realizada com Sucesso!";
                return RedirectToAction("Create", new { idTurma = matricula.Turma.Idturma });
            }

            TempData["Erro"] = "1";
            TempData["Mensagem"] = "Matricula NÃO realizada!";
            ViewBag.aluno_idaluno = new SelectList(_alunoDAO.All().ToList(), "idaluno", "nome".ToUpper());
            ViewBag.turma_idturma = new SelectList(_turmaDAO.All().ToList(), "idturma", "descricao".ToUpper());
 
            return View("Create ", new { idTurma = matricula.Turma.Idturma });
        }

        //
        // GET: /Matricula/Edit/5
        public ActionResult Edit(int id = 0)
        {
            VerificarSessao();

            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);
            Matricula _matricula = new Matricula();

            _matricula = _matriculaDAO.LoadById(id);

            if (_matricula == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.turma_descricao = _matricula.Turma.Descricao + " | " + _matricula.Turma.Curso.CursoNome;
            ViewBag.turma_idturma = _matricula.Turma.Idturma;

            //ViewBag.aluno_idaluno = new SelectList(_alunoDAO.All().ToList(), "idaluno", "nome", _matricula.Aluno.IdAluno);
            //ViewBag.turma_idturma = new SelectList(_turmaDAO.All().ToList(), "idturma", "descricao", _matricula.Turma.Idturma);

            _matricula.listaMatriculas = GetMatriculas(_matricula.Turma.Idturma).OrderBy(x => x.Aluno.Nome).ToList();

            return View("~/Views/Sistema/Matricula/Edit.cshtml", _matricula);
        }

        //
        // POST: /Matricula/Edit/5
        [HttpPost]
        public ActionResult Edit(Matricula matricula)
        {
            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);
            //matricula = _matriculaDAO.LoadById(matricula.IdMatricula);
            matricula.Aluno = _alunoDAO.LoadById(matricula.IdAluno);
           
            matricula.Turma = _turmaDAO.LoadById2(matricula.IdTurma);

            if (ModelState.IsValid)
            {
                _matriculaDAO.Update(matricula);
                this.Success("Matricula alterada com sucesso.");
                return RedirectToAction("Matriculas", new { id = matricula.IdTurma });
            }
            else
            {
               
                this.Danger("Não foi possível alterar a matricula, tente novamente.");

                matricula.listaMatriculas = GetMatriculas(matricula.IdTurma).OrderBy(x => x.Aluno.Nome).ToList();

                ViewBag.aluno_idaluno = new SelectList(_alunoDAO.All().ToList(), "idaluno", "nome", matricula.IdAluno);
                ViewBag.turma_idturma = new SelectList(_turmaDAO.All().ToList(), "idturma", "descricao", matricula.IdTurma);
            }
            return View("~/Views/Sistema/Matricula/Edit.cshtml", matricula);
        }

        //
        // POST: /Matricula/Delete/5
        public ActionResult Delete(int id = 0)
        {
            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);
            Matricula _matricula = new Matricula();


            _matricula = _matriculaDAO.LoadById(Convert.ToInt32(id));
            _turma = _matricula.Turma;
            //_turma = _turmaDAO.LoadById(Convert.ToInt32(_matricula.IdMatricula));
           
            if (ModelState.IsValid)
            {
                _matriculaDAO.Delete(_matricula);
                this.Success("Matricula removida com sucesso");
                return RedirectToAction("Create", new { id = _turma.Idturma });
            }
            this.Danger("Matricula não pode ser removida, tente novamente!");
            ViewBag.aluno_idaluno = new SelectList(_alunoDAO.All().ToList(), "idaluno", "nome", _matricula.Aluno.IdAluno);
            ViewBag.turma_idturma = new SelectList(_turmaDAO.All().ToList(), "idturma", "descricao", _matricula.Turma.Idturma);
            return View("~/Views/Sistema/Matricula/Delete.cshtml", _matricula);

            
        }

        public ActionResult DeleteItem(int id = 0)
        {
            AlunoDAO _alunoDAO = new AlunoDAO(ref _db);
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);
            Matricula _matricula = new Matricula();


            _matricula = _matriculaDAO.LoadById(Convert.ToInt32(id));
            _turma = _matricula.Turma;
            //_turma = _turmaDAO.LoadById(Convert.ToInt32(_matricula.IdMatricula));

            if (ModelState.IsValid)
            {
                _matriculaDAO.Delete(_matricula);
                this.Success("Matricula removida com sucesso");
                return RedirectToAction("Matriculas", new { id = _turma.Idturma });
            }
            this.Danger("Matricula não pode ser removida, tente novamente!");
            ViewBag.aluno_idaluno = new SelectList(_alunoDAO.All().ToList(), "idaluno", "nome", _matricula.Aluno.IdAluno);
            ViewBag.turma_idturma = new SelectList(_turmaDAO.All().ToList(), "idturma", "descricao", _matricula.Turma.Idturma);
            
            return View("~/Views/Sistema/Matricula/Delete.cshtml", _matricula);


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