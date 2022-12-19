using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SDE_FIC.DAO;
using SDE_FIC.Models;
using PagedList;
//using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using SDE_FIC.Util;
//using iTextSharp.text.html.simpleparser;
//using System.Web.UI.HtmlControls;


namespace SDE_FIC.Controllers
{
    public class DiarioController : Controller
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
        // GET: /Diario/
        public ActionResult Index(int? pagina, string strCriterioTurma, string strCriterioCurso, string strCriterioProfessor, int strCriterioStatus = 1)
        {


            VerificarSessao();

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            TurmaDAO turmasDAO = new TurmaDAO(ref _db);
            List<Turma> lTurma = new List<Turma>();

            FuncionarioDAO funcionarioDAO = new FuncionarioDAO(ref _db);
            Funcionario _funcionario = new Funcionario();

            ViewBag.strCriterioTurma = strCriterioTurma;
            ViewBag.strCriterioCurso = strCriterioCurso;
            ViewBag.strCriterioProfessor = strCriterioProfessor;
            ViewBag.strCriterioStatus = strCriterioStatus;


            if (strCriterioProfessor == null && strCriterioCurso == null && strCriterioTurma == null && strCriterioStatus == 2)
            {
                if (Session["LogedUserPerfil"].ToString() != "professor")
                {
                    lTurma = turmasDAO.All().OrderByDescending(x => x.DataInicio).ToList();
                }
                else {
                    _funcionario.IdFuncionario = funcionarioDAO.LoadByNome(Session["LogedUserNomeCompleto"].ToString()).IdFuncionario;
                    lTurma = turmasDAO.LoadByFuncionario(ref _funcionario, true).OrderByDescending(x => x.DataInicio).ToList();
                }
            } 
            else
            {
                if (Session["LogedUserPerfil"].ToString() != "professor")
                {
                    lTurma = turmasDAO.All().ToList().OrderByDescending(x => x.DataInicio).ToList();
                }
                else
                {
                    _funcionario.IdFuncionario = funcionarioDAO.LoadByNome(Session["LogedUserNomeCompleto"].ToString()).IdFuncionario;
                    lTurma = turmasDAO.LoadByFuncionario(ref _funcionario, true).OrderByDescending(x => x.DataInicio).ToList();
                }
                

                if (strCriterioStatus == 1)
                {
                    lTurma = lTurma.Where(x => x.Status == true).ToList();
                }
                else if (strCriterioStatus == 0)
                {
                    lTurma = lTurma.Where(x => x.Status == false).ToList();
                }

                if(strCriterioProfessor != null)
                    lTurma = lTurma.Where(x => x.Funcionario.NomeCompleto.ToUpper().Contains(strCriterioProfessor.ToUpper())).ToList();

                if (strCriterioCurso != null)
                    lTurma = lTurma.Where(x => x.Curso.CursoNome.ToUpper().Contains(strCriterioCurso.ToUpper())).ToList();

                if (strCriterioTurma != null)
                    lTurma = lTurma.Where(x => x.Descricao.ToUpper().Contains(strCriterioTurma.ToUpper())).ToList();

            }

            return View("~/Views/Sistema/Diario/Index.cshtml", lTurma.ToPagedList(numeroPagina, tamanhoPagina));

        }



        //
        // GET: /Diario/Turma/5
        public ActionResult Turma(int id = 0)
        {
            VerificarSessao();

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            FrequenciaDAO frequenciasDAO = new FrequenciaDAO(ref _db);

            Diario _diario = new Diario();
            DiarioDAO diarioDAO = new DiarioDAO(ref _db);


            _turma = turmaDAO.LoadById2(id);
            _diario.Turma = _turma;
            _diario.IdTurma = _turma.Idturma;

            //ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;
            //ViewBag.turma_idturma = _turma.Idturma;

            //UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            //ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_turma.Curso.Unidadecurricular.ToList(), "IdUnidadeCurricular", "Descricao");
            //ViewBag.ListaUnidadesCurricular = _unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList();

            //MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            //List<Matricula> lMatriculas = _turma.ListaMatriculas.ToList();


            //_turma.Diario = diarioDAO.LoadByDiarioTurma(ref _turma).ToList();
            //ViewBag.ListaDiario = _turma.Diario.ToList();

            //TempData["TotalHoraCurso"] = _turma.Curso.CargaHoraria;
            //TempData["TotalDeAlunos"] = _turma.ListaMatriculas.Count();
            //TempData["TotalAtivos"] = _turma.ListaMatriculas.Where(x => x.Situacao == "Ativo").Count();
            //TempData["TotalEvadido"] = _turma.ListaMatriculas.Where(x => x.Situacao == "Evadido").Count();
            //TempData["TotalTransferidoE"] = _turma.ListaMatriculas.Where(x => x.Situacao == "Transferido (E)").Count();
            //TempData["TotalTransferidoS"] = _turma.ListaMatriculas.Where(x => x.Situacao == "Transferido (S)").Count();
            //TempData["TotalDesistente"] = _turma.ListaMatriculas.Where(x => x.Situacao == "Desistente").Count();

            /*
            int totalAproveitamentos = 0;
            for (int i = 0; i < lMatriculas.Count(); i++)
            {
                totalAproveitamentos = totalAproveitamentos + lMatriculas[i].listaAproveitamentos.Count();            
            }
            TempData["TotalAproveitamentos"] = totalAproveitamentos;

            if (diarioDAO.LoadByTurma(_turma).LastOrDefault() != null)
            {
                TempData["UltimoLancamento"] = _turma.Diario.Last().Data.ToShortDateString();
                TempData["TotalFaltantes"] = _turma.Diario.Last().Frequencias.Where(x => x.Presenca == "A").Count();
                if (TempData["TotalFaltantes"].ToString() == "0")
                {
                    TempData["TotalFaltantes"] = "Nenhuma falta";
                }
                List<Frequencia> lFaltantes = _turma.Diario.LastOrDefault().Frequencias.Where(x => x.Presenca == "A").ToList();
                ViewBag.AlunosFaltantes = lFaltantes;
            }
            else
            {
                TempData["UltimoLancamento"] = "Não realizado Lançamento de Frequência!";
                ViewBag.AlunosFaltantes = null;
            }
            */

            return View("~/Views/Sistema/Diario/Turma.cshtml", _diario);
        }

        // GET: /Diario/Turma/5
        public ActionResult Resumo(int id = 0)
        {
            VerificarSessao();

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            FrequenciaDAO frequenciasDAO = new FrequenciaDAO(ref _db);

            Diario _diario = new Diario();
            DiarioDAO diarioDAO = new DiarioDAO(ref _db);

            _turma = turmaDAO.LoadById2(id);
            _turma.Diario = diarioDAO.LoadByDiarioTurma(ref _turma).ToList();

            _diario.Turma = _turma;
            _diario.IdTurma = _turma.Idturma;


            return View("~/Views/Sistema/Diario/Resumo.cshtml", _diario);
        }
        //
        // GET: /Diario/Notas/5

        public ActionResult Notas(int id = 0)
        {
            VerificarSessao();

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            ViewBag.Turma = _turma;

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = _unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList();

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            List<Matricula> lMatriculas = matriculaDAO.LoadByTurma2(_turma).OrderBy(x => x.Aluno.Nome).ToList();

            return View("~/Views/Sistema/Diario/Notas.cshtml", lMatriculas);
        }

        public ActionResult Aproveitamentos(int id = 0)
        {
            VerificarSessao();

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            ViewBag.Turma = _turma;

            ViewBag.turma_idturma = _turma.Idturma;


            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");            


            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            List<Matricula> lMatriculas = matriculaDAO.LoadByTurma2(_turma).OrderBy(x => x.Aluno.Nome).ToList();
            ViewBag.matriculas_idmatriculas = new SelectList(lMatriculas.ToList(), "IdMatricula", "Aluno.Nome");


            return View("~/Views/Sistema/Diario/Aproveitamentos.cshtml", lMatriculas);
        }

        //
        //GET: /Diario/APROVEITAMENTOS
        public ActionResult AproveitamentoNovo(int id = 0)
        {
            VerificarSessao();

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            ViewBag.matriculas_idmatriculas = new SelectList(matriculaDAO.LoadByTurma(_turma).OrderBy(x => x.Aluno.Nome).ToList(), "IdMatricula", "Aluno.Nome");
            // List<Matricula> lMatriculas = matriculaDAO.LoadByTurma(_turma).OrderBy(x => x.Aluno.Nome).ToList();

            //ViewBag.lMatriculas = lMatriculas.ToList();

            return View("~/Views/Sistema/Diario/AproveitamentosNovo.cshtml");

        }

        //
        //POST: /Diario/NOVOAPROVEITAMENTOS
        [HttpPost]
        public ActionResult AproveitamentoNovo(int IdMatricula, int IdUnidadeCurricular, int id)
        {
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();
            _turma = turmaDAO.LoadById(id);

            if (ModelState.IsValid)
            {
                AproveitamentosDAO _aproveitamentoDAO = new AproveitamentosDAO(ref _db);
                _aproveitamentoDAO.Insert(IdMatricula, IdUnidadeCurricular);
                return RedirectToAction("Aproveitamentos", new { id = _turma.Idturma });
            }
            return RedirectToAction("Aproveitamentos", new { id = _turma.Idturma });

        }


        //
        // POST: /Diario/AproveitamentoExcluir
        [HttpGet]
        public ActionResult AproveitamentoExcluir(int id, int idAproveitamento)
        {

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById(id);

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            
            AproveitamentosDAO _aproveitamentoDAO = new AproveitamentosDAO(ref _db);
            Aproveitamentos _aproveitamento = new Aproveitamentos();

            _aproveitamento = _aproveitamentoDAO.LoadById(idAproveitamento);
            _aproveitamentoDAO.Delete(_aproveitamento);

            TempData["Erro"] = "0";
            TempData["Mensagem"] = "Aproveitamento EXCLUÍDO com Sucesso!";

            return RedirectToAction("Aproveitamentos", new { id = _turma.Idturma});
        }

        //
        //GET: /Diario/NotasNovo/5
        public ActionResult NotasNovo(int id = 0)
        {
            VerificarSessao();

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);
            
            ViewBag.Turma = _turma;

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");
            ViewBag.unidadecurricular_idunidadecurricular2 = _unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList();

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            List<Matricula> lMatriculas = matriculaDAO.LoadByTurma(_turma).OrderBy(x => x.Aluno.Nome).ToList();

            ViewBag.lMatriculas = lMatriculas.ToList();

            return View("~/Views/Sistema/Diario/NotasNovo.cshtml");

        }

        //
        //POST: /Diario/NotasNovo/5
        [HttpPost]
        public ActionResult NotasNovo(IList<int> IdMatricula, IList<string> Nota, int IdUnidadeCurricular, int id)
        {
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();
            _turma = turmaDAO.LoadById(id);

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;
            ViewBag.turma_idturma = _turma.Idturma;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            ViewBag.unidadecurricular_idunidadecurricular2 = _unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList();

            if (ModelState.IsValid)
            {

                NotasDAO _notasDAO = new NotasDAO(ref _db);
                for (int i = 0; i < IdMatricula.Count(); i++)
                {
                    _notasDAO.Insert(IdMatricula[i], Nota[i], IdUnidadeCurricular);
                }
                TempData["Erro"] = "0";
                TempData["Mensagem"] = "Notas cadastradas com Sucesso!";
                return RedirectToAction("Notas", new { id = _turma.Idturma });
            }
            TempData["Erro"] = "1";
            TempData["Mensagem"] = "Notas não cadastradas. Tente Novamente!";
            return RedirectToAction("NotasNovo", new { id = _turma.Idturma });

        }

        //
        //GET: /Diario/NotasNovo/5
        public ActionResult NotasNovoEditar(int IdNotas, int id)
        {
            VerificarSessao();
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            ViewBag.Turma = _turma;

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            List<Matricula> lMatriculas = matriculaDAO.LoadByTurma(_turma).OrderBy(x => x.Aluno.Nome).ToList();

            ViewBag.lMatriculas = lMatriculas.ToList();

            NotasDAO _notasDAO = new NotasDAO(ref _db);
            Notas _notas = new Notas();

            _notas = _notasDAO.LoadById(IdNotas);

            return View("~/Views/Sistema/Diario/NotasNovoEditar.cshtml", _notas);

        }

        //
        //GET: /Diario/NotasNovo/5
        [HttpPost]
        public ActionResult NotasNovoEditar(Notas notas, int id)
        {
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            if (ModelState.IsValid)
            {

                NotasDAO _notasDAO = new NotasDAO(ref _db);
                _notasDAO.Update(ref notas);
                TempData["Erro"] = "0";
                TempData["Mensagem"] = "Notas alteradas com Sucesso!";
                return RedirectToAction("Notas", new { id = _turma.Idturma });
            }
            TempData["Erro"] = "1";
            TempData["Mensagem"] = "Notas não Alteradas. Tente Novamente!";
            return RedirectToAction("NotasNovoEditar", new { id = _turma.Idturma, idNotas = notas.IdNotas });

        }

        //
        //Gerar PDF Download para a Pasta Download do Usuário do Sistema
        private void DownloadAsPDF(Turma _turma, int IdUnidadeCurricular)
        {
            UnidadeCurricular _unidadecurricular = new UnidadeCurricular();
            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            
            Aproveitamentos _aproveitamentos = new Aproveitamentos();
            AproveitamentosDAO _aproveitamentoDAO = new AproveitamentosDAO(ref _db);
            List<Aproveitamentos> lAproveitamento = new List<Aproveitamentos>();

            DiarioDAO diarioDAO = new DiarioDAO(ref _db);
            List<Diario> lDiario = new List<Diario>();

            List<Matricula> lMatriculas = _turma.ListaMatriculas.OrderBy(x => x.Aluno.Nome).ToList();            

            List<UnidadeCurricular> lUnidadeCurricular = _unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList();

            if (IdUnidadeCurricular != 0)
            {
                _unidadecurricular = _unidadeCurricularDAO.LoadById(IdUnidadeCurricular);
                lDiario = diarioDAO.LoadByConteudo(_turma).OrderBy(x => x.Data).Where(x => x.UnidadeCurricular.IdUnidadeCurricular == IdUnidadeCurricular).ToList();
            }
            else
            {
                lDiario = diarioDAO.LoadByConteudo(_turma).OrderBy(x => x.Data).ToList();
            }

            int TotalRegistros = lDiario.Count();
            int inicio = 0;
            int fim = 25;
            int kinicio = 0;
            int kfim = 25;

            /////////////////////////////////////////////////////////////////////////
            //Gerar PDF e Fazer DOWNLOAD para o Usuário            
            /////////////////////////////////////////////////////////////////////////
            using (MemoryStream ms = new MemoryStream())
            {

                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                document.Open();

                int Paginas = TotalRegistros / 25;
                int h = 0;
                int k = 0;
                int n = 0;                
                decimal tFaltas = 0;
                decimal tNota = 0;
                string tResultado = " ";
                int Parametro = 25;
                decimal Perc = 0;
                int lm = (32 - lMatriculas.Count());

                if (IdUnidadeCurricular != 0)
                {
                    while (h <= Paginas)
                    {
                        #region Tabela Conteúdo
                        /*Tabela Conteudo*/
                        iTextSharp.text.pdf.PdfPTable tabelaConteudo = new iTextSharp.text.pdf.PdfPTable(3);
                        tabelaConteudo.TotalWidth = 400;
                        tabelaConteudo.LockedWidth = true;
                        float[] widths = new float[] { 50, 50, 300 };
                        tabelaConteudo.SetWidths(widths);
                        float alturaLinha = 11;

                        /*Titulo da Tabela Conteudo*/
                        /////////////////////////////////////////////////////////////
                        iTextSharp.text.pdf.PdfPCell celTitulo = new iTextSharp.text.pdf.PdfPCell();
                        celTitulo.Phrase = new Phrase(alturaLinha, "DATA", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                        celTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                        celTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
                        celTitulo.PaddingBottom = 5;
                        tabelaConteudo.AddCell(celTitulo);

                        celTitulo = new iTextSharp.text.pdf.PdfPCell();
                        celTitulo.Phrase = new Phrase(alturaLinha, "HORAS", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                        celTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        celTitulo.PaddingBottom = 5;
                        tabelaConteudo.AddCell(celTitulo);

                        celTitulo = new iTextSharp.text.pdf.PdfPCell();
                        celTitulo.Phrase = new Phrase(alturaLinha, "CONTEÚDO FORMATIVO", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                        celTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        celTitulo.PaddingBottom = 5;
                        tabelaConteudo.AddCell(celTitulo);

                        /*Conteudo da TabelaConteudo*/
                        while (inicio < fim)
                        {
                            if (inicio < TotalRegistros)
                            {
                                iTextSharp.text.pdf.PdfPCell celConteudo = new iTextSharp.text.pdf.PdfPCell();
                                celConteudo.Phrase = new Phrase(alturaLinha, lDiario[inicio].Data.Day.ToString() + "/" + lDiario[inicio].Data.Month.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                                celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                                celConteudo.PaddingBottom = 5;
                                tabelaConteudo.AddCell(celConteudo);

                                celConteudo = new iTextSharp.text.pdf.PdfPCell();
                                celConteudo.Phrase = new Phrase(alturaLinha, lDiario[inicio].HoraAulaDia.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                                celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                                celConteudo.PaddingBottom = 5;
                                tabelaConteudo.AddCell(celConteudo);

                                celConteudo = new iTextSharp.text.pdf.PdfPCell();
                                celConteudo.Phrase = new Phrase(alturaLinha, lDiario[inicio].Conteudo.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                                celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                                celConteudo.PaddingBottom = 5;
                                tabelaConteudo.AddCell(celConteudo);
                                inicio++;
                            }
                            else
                            {
                                inicio = fim + 1;
                            };
                        };

                        //int f = 25 - lDiario.Count();

                        //for (int e = 0; e < f; e++)
                        //{
                        //    iTextSharp.text.pdf.PdfPCell celConteudo = new iTextSharp.text.pdf.PdfPCell();
                        //    celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        //    celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        //    celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        //    celConteudo.PaddingBottom = 5;
                        //    tabelaConteudo.AddCell(celConteudo);

                        //    celConteudo = new iTextSharp.text.pdf.PdfPCell();
                        //    celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        //    celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        //    celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        //    celConteudo.PaddingBottom = 5;
                        //    tabelaConteudo.AddCell(celConteudo);

                        //    celConteudo = new iTextSharp.text.pdf.PdfPCell();
                        //    celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        //    celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                        //    celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        //    celConteudo.PaddingBottom = 5;
                        //    tabelaConteudo.AddCell(celConteudo);
                        //};


                        #endregion

                        #region Tabela Cabeçalho
                        /*Tabela Dados Cabeçalho*/
                        /////////////////////////////////////////////////////////////
                        iTextSharp.text.pdf.PdfPTable tabelaDadosCabecalho = new iTextSharp.text.pdf.PdfPTable(2);
                        //tabelaConteudo.HorizontalAlignment = Image.ALIGN_LEFT;
                        tabelaDadosCabecalho.TotalWidth = 400;
                        tabelaDadosCabecalho.LockedWidth = true;
                        float[] widthtdc = new float[] { 150, 250 };
                        tabelaDadosCabecalho.SetWidths(widthtdc);

                        /*Inserindo Imagem*/
                        string imageURL = Server.MapPath(".") + "../../../Content/img/" + "logo-senai.jpg";
                        Image logoSenai = Image.GetInstance(imageURL);
                        logoSenai.ScaleToFit(130f, 130f);
                        float alturadalinha2 = 14;

                        iTextSharp.text.pdf.PdfPCell CelDadosTitulo = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosTitulo.Border = Image.NO_BORDER;
                        CelDadosTitulo.AddElement(logoSenai);
                        CelDadosTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                        CelDadosTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                        tabelaDadosCabecalho.AddCell(CelDadosTitulo);

                        CelDadosTitulo = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosTitulo.Border = Image.NO_BORDER;
                        CelDadosTitulo.Phrase = new Phrase(8, "ESCOLA SENAI - LENÇÓIS PAULISTA \n" + "Rua Aristeu Rodrigues Sampaio, 271 \n" + "(14) 3269-3969",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelDadosTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                        tabelaDadosCabecalho.AddCell(CelDadosTitulo);
                        #endregion

                        #region Tabela Dados
                        /*Tabela Dados*/
                        /////////////////////////////////////////////////////////////
                        iTextSharp.text.pdf.PdfPTable tabelaDados = new iTextSharp.text.pdf.PdfPTable(4);
                        //tabelaConteudo.HorizontalAlignment = Image.ALIGN_LEFT;
                        tabelaDados.TotalWidth = 400;
                        tabelaDados.LockedWidth = true;
                        float[] width = new float[] { 110, 110, 70, 110 };
                        tabelaDados.SetWidths(width);

                        iTextSharp.text.pdf.PdfPCell CelDadosTitulo1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosTitulo1.Colspan = 4; // either 1 if you need to insert one cell
                        CelDadosTitulo1.Border = Image.NO_BORDER;
                        CelDadosTitulo1.PaddingBottom = 5;
                        CelDadosTitulo1.Phrase = new Phrase(30, "\n\nDIÁRIO DE CLASSE POR UNIDADE CURRICULAR\n" + "FORMAÇÃO INICIAL CONTINUADA\n\n",
                                                  new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.BLACK));
                        CelDadosTitulo1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosTitulo1);


                        iTextSharp.text.pdf.PdfPCell CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosCabecalho.PaddingBottom = 5;
                        CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Nº da Proposta \n" + _turma.PropostaAno,
                                                  new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        tabelaDados.AddCell(CelDadosCabecalho);

                        CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Local de Realização \n" + _turma.LocalRealizacao,
                                                  new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        tabelaDados.AddCell(CelDadosCabecalho);

                        CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Atendimento \n" + _turma.Atendimento,
                                                  new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        tabelaDados.AddCell(CelDadosCabecalho);

                        CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Turma \n" + _turma.Descricao,
                                                  new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        tabelaDados.AddCell(CelDadosCabecalho);

                        iTextSharp.text.pdf.PdfPCell CelDadosCurso = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosCurso.Colspan = 4;
                        CelDadosCurso.PaddingBottom = 5;
                        CelDadosCurso.Phrase = new Phrase(alturadalinha2, "Curso \n" + _turma.Curso.CursoNome,
                                                  new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        tabelaDados.AddCell(CelDadosCurso);

                        iTextSharp.text.pdf.PdfPCell CelDadosUC = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosUC.Colspan = 4;
                        CelDadosUC.PaddingBottom = 5;
                        if (IdUnidadeCurricular != 0)
                        {
                            CelDadosUC.Phrase = new Phrase(alturadalinha2, "Unidade Curricular \n" + _unidadecurricular.Descricao,
                                                      new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelDadosUC.Phrase = new Phrase(alturadalinha2, "Unidade Curricular \n" + _turma.Curso.CursoNome,
                                  new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        };
                        tabelaDados.AddCell(CelDadosUC);

                        iTextSharp.text.pdf.PdfPCell CelDadosDocente = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosDocente.Colspan = 4;
                        CelDadosDocente.PaddingBottom = 5;
                        CelDadosDocente.Phrase = new Phrase(alturadalinha2, "Docente \n" + _turma.Funcionario.NomeCompleto,
                                                  new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        tabelaDados.AddCell(CelDadosDocente);

                        iTextSharp.text.pdf.PdfPCell CelDadosInicio = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosInicio.Colspan = 1;
                        CelDadosInicio.PaddingBottom = 5;
                        CelDadosInicio.Phrase = new Phrase(alturadalinha2, "Início Previsto \n" + _turma.DataInicio.Value.ToShortDateString(),
                                                  new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        tabelaDados.AddCell(CelDadosInicio);

                        string Se;
                        string Te;
                        string Qu;
                        string Qui;
                        string Sex;
                        string Sa;
                        string Do;

                        if (_turma.Segunda == true) { Se = "Segunda-Feira - " + _turma.Horario + "\n"; } else { Se = ""; };
                        if (_turma.Terca == true) { Te = "Terça-Feira - " + _turma.Horario + "\n"; } else { Te = ""; };
                        if (_turma.Quarta == true) { Qu = "Quarta-Feira - " + _turma.Horario + "\n"; } else { Qu = ""; };
                        if (_turma.Quinta == true) { Qui = "Quinta-Feira - " + _turma.Horario + "\n"; } else { Qui = ""; };
                        if (_turma.Sexta == true) { Sex = "Sexta-Feira - " + _turma.Horario + "\n"; } else { Sex = ""; };
                        if (_turma.Sabado == true) { Sa = "Sábado - " + _turma.Horario + "\n"; } else { Sa = ""; };
                        if (_turma.Domingo == true) { Do = "Domingo - " + _turma.Horario; } else { Do = ""; };

                        CelDadosInicio = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosInicio.Colspan = 3;
                        CelDadosInicio.Rowspan = 2;
                        CelDadosInicio.Phrase = new Phrase(alturadalinha2, "Dia da Semana e Horários \n" + Se + Te + Qu + Qui + Sex + Sa + Do,
                                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                        tabelaDados.AddCell(CelDadosInicio);

                        iTextSharp.text.pdf.PdfPCell CelDadosTermino = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosTermino.Colspan = 1;
                        CelDadosTermino.PaddingBottom = 5;
                        CelDadosTermino.Phrase = new Phrase(alturadalinha2, "Término Previsto \n" + _turma.DataFim.Value.ToShortDateString(),
                                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                        tabelaDados.AddCell(CelDadosTermino);

                        decimal HorasPrevista = _unidadecurricular.CargaHoraria;

                        if (IdUnidadeCurricular == 0)
                        {
                            HorasPrevista = _turma.Curso.CargaHoraria;
                        }

                        iTextSharp.text.pdf.PdfPCell CelDadosHora = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosHora.Colspan = 1;
                        CelDadosHora.PaddingBottom = 5;
                        CelDadosHora.Phrase = (new Phrase(alturadalinha2, "Horas (Previsto)\n" + HorasPrevista,
                                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                        tabelaDados.AddCell(CelDadosHora);

                        CelDadosHora = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosHora.Colspan = 3;
                        CelDadosHora.Phrase = (new Phrase(50, "Responsável na Escola \n" + _turma.Responsavel,
                                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosHora.HorizontalAlignment = Element.ALIGN_LEFT;
                        CelDadosHora.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaDados.AddCell(CelDadosHora);


                        iTextSharp.text.pdf.PdfPCell CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosCliente.Colspan = 4;
                        CelDadosCliente.PaddingBottom = 5;
                        CelDadosCliente.Phrase = (new Phrase(alturadalinha2, "Cliente",
                                                  new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                        CelDadosCliente.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosCliente);

                        CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosCliente.Colspan = 4;
                        CelDadosCliente.VerticalAlignment = Image.MIDDLE_ALIGN;
                        CelDadosCliente.Phrase = (new Phrase(10, _turma.Cliente + "\n\n\n\n\n",
                                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                        tabelaDados.AddCell(CelDadosCliente);

                        CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosCliente.Colspan = 4;
                        CelDadosCliente.PaddingBottom = 5;
                        CelDadosCliente.Phrase = (new Phrase(alturadalinha2, "Informações para Pagamento do Docente",
                                                  new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                        CelDadosCliente.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosCliente);

                        iTextSharp.text.pdf.PdfPCell CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento.PaddingBottom = 5;

                        CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Período",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento.HorizontalAlignment = 1;
                        CelDadosPagamento.PaddingBottom = 5;
                        tabelaDados.AddCell(CelDadosPagamento);

                        CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Total de Horas",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento);

                        CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Valor Hora",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento);

                        CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Representante Empresa",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento);

                        int a = 5;
                        int i = 0;
                        while (i < a)
                        {
                            i++;

                            PdfPCell CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                            CelDadosPagamento1.AddElement(new Phrase(20, " ",
                                                      new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                            CelDadosPagamento1.HorizontalAlignment = 1;
                            tabelaDados.AddCell(CelDadosPagamento1);

                            CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                            CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                      new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                            CelDadosPagamento1.HorizontalAlignment = 1;
                            tabelaDados.AddCell(CelDadosPagamento1);

                            CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                            CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                      new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                            CelDadosPagamento1.HorizontalAlignment = 1;
                            tabelaDados.AddCell(CelDadosPagamento1);

                            CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                            CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                      new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                            CelDadosPagamento1.HorizontalAlignment = 1;
                            tabelaDados.AddCell(CelDadosPagamento1);

                        };

                        PdfPCell CelDadosRodape1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosRodape1.Colspan = 4;
                        CelDadosRodape1.Phrase = (new Phrase(10, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosRodape1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosRodape1);


                        PdfPCell CelDadosRodape = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosRodape.Colspan = 1;
                        CelDadosRodape.PaddingBottom = 5;
                        CelDadosRodape.Phrase = (new Phrase(10, "Início Efetivo\n" + lDiario.First().Data.ToShortDateString(),
                                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosRodape.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosRodape);

                        CelDadosRodape = new PdfPCell();
                        CelDadosRodape.Colspan = 2;
                        CelDadosRodape.Phrase = (new Phrase(10, "Término Efetivo\n" + lDiario.Last().Data.ToShortDateString(),
                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosRodape.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosRodape);

                        CelDadosRodape = new PdfPCell();
                        CelDadosRodape.Colspan = 1;


                        if (IdUnidadeCurricular != 0)
                        {
                            CelDadosRodape.Phrase = (new Phrase(10, "Horas (Realizado)\n" + lDiario.Where(x => x.IdUnidadeCurricular == IdUnidadeCurricular).Sum(x => x.HoraAulaDia),
                                      new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                        }
                        else
                        {
                            CelDadosRodape.Phrase = (new Phrase(10, "Horas (Realizado)\n" + lDiario.Sum(x => x.HoraAulaDia),
                                      new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                        };

                        CelDadosRodape.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosRodape);

                        #endregion

                        #region Tabela Geral
                        /*Tabela Geral*/
                        /////////////////////////////////////////////////////////////
                        iTextSharp.text.pdf.PdfPTable tabelaGeral = new iTextSharp.text.pdf.PdfPTable(2);
                        tabelaGeral.WidthPercentage = 100;

                        iTextSharp.text.pdf.PdfPCell tabConteudo = new iTextSharp.text.pdf.PdfPCell();
                        tabConteudo.Border = Image.RIGHT_BORDER;
                        tabConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                        tabConteudo.AddElement(tabelaConteudo);

                        tabelaGeral.AddCell(tabConteudo);

                        iTextSharp.text.pdf.PdfPCell tabDados = new iTextSharp.text.pdf.PdfPCell();
                        tabDados.Border = Image.NO_BORDER;
                        tabDados.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                        tabDados.AddElement(tabelaDadosCabecalho);
                        tabDados.AddElement(tabelaDados);


                        tabelaGeral.AddCell(tabDados);

                        //////////////////////////////////////////////////////////////////////////////
                        #endregion

                        /*Tabela Frequencia*/
                        ///////////////////////////////////////////////////////// 
                        PdfPTable tabelaFrequencia = new PdfPTable(32);
                        tabelaFrequencia.TotalWidth = 815;
                        tabelaFrequencia.LockedWidth = true;
                        float[] widthf = new float[] { 
                            15, 200, 10, 
                            12, 12, 12, 12, 12,
                            12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                            12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 
                            30, 15, 30, 40 };
                        tabelaFrequencia.SetWidths(widthf);
                        float alturaLinha3 = 10;

                        #region 1 Linha Cabeçalho
                        //1º Linha Cabeçalho//////////////////////////////////////////////

                        PdfPCell CelFrequencia = new iTextSharp.text.pdf.PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nº", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.Rowspan = 2;
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);

                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nome do Treinando", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.Rowspan = 2;
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);

                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "M", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);

                        int j = kinicio;
                        int b = kfim;


                        while (j < b)
                        {
                            if (j <= TotalRegistros)
                            {
                                CelFrequencia = new PdfPCell();
                                if (IdUnidadeCurricular != 0)
                                {
                                    if (j < TotalRegistros)
                                    {
                                        CelFrequencia.Phrase = new Phrase(alturaLinha3, lDiario[j].Data.Month.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                    }
                                    else
                                    {
                                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                    }
                                }
                                else
                                {
                                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                };

                            }
                            else
                            {
                                CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            }
                            CelFrequencia.PaddingBottom = 3;
                            CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                            CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tabelaFrequencia.AddCell(CelFrequencia);
                            j++;
                        }


                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "Faltas\n" + "(Hrs)", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.Rowspan = 2;
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);

                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nº", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.Rowspan = 2;
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);

                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nota", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.Rowspan = 2;
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);

                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "Resultado\n" + "Final", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.Rowspan = 2;
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);
                        #endregion

                        #region 2 Linha Cabeçalho                                               

                        //2º Linha Cabeçalho
                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "D", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);


                        j = kinicio;
                        b = kfim;
                        while (j < b)
                        {
                            CelFrequencia = new PdfPCell();
                            if (j <= TotalRegistros)
                            {
                                if (IdUnidadeCurricular != 0)
                                {
                                    if (j < lDiario.Count())
                                    {
                                        CelFrequencia.Phrase = new Phrase(alturaLinha3, lDiario[j].Data.Day.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                    }
                                    else
                                    {
                                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                    };
                                }
                                else
                                {
                                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));

                                };
                            }
                            else
                            {
                                CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            }
                            CelFrequencia.PaddingBottom = 3;
                            CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                            CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tabelaFrequencia.AddCell(CelFrequencia);
                            j++;
                        }

                        #endregion

                        #region Nome Alunos                        

                        /////////////////////Alunos/////////////////////////
                        Matricula _matricula = new Matricula();
                        MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);

                        Notas _notas = new Notas();
                        NotasDAO _notasDAO = new NotasDAO(ref _db);
                        List<Notas> lNotas = new List<Models.Notas>();

                        Frequencia _frequencia = new Frequencia();
                        FrequenciaDAO _frequenciaDAO = new FrequenciaDAO(ref _db);
                        List<Frequencia> lFrequencia = new List<Frequencia>();

                        k = 0;

                        while (k < (lMatriculas.Count() + lm))
                        {
                            n = k + 1;
                            PdfPCell CelFrequenciaAlunos = new iTextSharp.text.pdf.PdfPCell();
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, n.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            CelFrequenciaAlunos.PaddingBottom = 3;
                            CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                            CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                            CelFrequenciaAlunos = new PdfPCell();

                            if (k < _turma.ListaMatriculas.Count())
                            {
                                //CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[k].Aluno.Nome.ToUpper().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                if (lMatriculas[k].Situacao == "Ativo")
                                {
                                                                       
                                    lAproveitamento = _aproveitamentoDAO.LoadByMatricula(lMatriculas[k]);
                                    
                                    string ListaAproveitamentos = "";

                                    if (lAproveitamento.Count() != 0)
                                    {
                                        for (int m = 0; m < lAproveitamento.Count(); m++)
                                        {
                                            ListaAproveitamentos =  ListaAproveitamentos + "(" + lAproveitamento[m].UnidadeCurricular.Sigla.ToString() + ")";
                                        }
                                    }
                                    if (ListaAproveitamentos != ""){
                                        ListaAproveitamentos = " **" + ListaAproveitamentos.ToString();
                                    }
                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[k].Aluno.Nome.ToUpper().ToString() + ListaAproveitamentos.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                }

                                if (lMatriculas[k].Situacao == "Evadido")
                                {
                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[k].Aluno.Nome.ToUpper().ToString() + " *" + lMatriculas[k].DataSituacao.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                }
                                  
                                if (lMatriculas[k].Situacao == "Transferido (E)"){
                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[k].Aluno.Nome.ToUpper().ToString() + " ****" + lMatriculas[k].DataSituacao.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                }

                                if (lMatriculas[k].Situacao == "Transferido (S)")
                                {
                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[k].Aluno.Nome.ToUpper().ToString() + " ****" + lMatriculas[k].DataSituacao.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                }




                                lFrequencia = _frequenciaDAO.LoadByMatricula(lMatriculas[k]).Where(x => x.Diario.UnidadeCurricular.IdUnidadeCurricular == IdUnidadeCurricular).OrderBy(x => x.Diario.Data).ToList();

                                if (lMatriculas[k].Situacao == "Evadido")
                                {
                                    tFaltas = lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "A").Sum(x => x.Diario.HoraAulaDia) + (lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TS").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TS").Sum(x => x.HoraAula)) + (lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TE").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TE").Sum(x => x.HoraAula));
                                }
                                else
                                {
                                    tFaltas = lFrequencia.Where(x => x.Presenca == "A").Sum(x => x.Diario.HoraAulaDia) + (lFrequencia.Where(x => x.Presenca == "TS").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Presenca == "TS").Sum(x => x.HoraAula)) + (lFrequencia.Where(x => x.Presenca == "TE").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Presenca == "TE").Sum(x => x.HoraAula));
                                    //tFaltas = (_unidadeCurricularDAO.LoadById(IdUnidadeCurricular).CargaHoraria - _frequenciaDAO.LoadByMatricula(lMatriculas[k]).Where(x => x.Diario.UnidadeCurricular.IdUnidadeCurricular == IdUnidadeCurricular).Sum(x => x.HoraAula));
                                }

                                if (_notasDAO.LoadByIdMIdUC(lMatriculas[k].IdMatricula, IdUnidadeCurricular) == null)
                                {
                                    tNota = 0;
                                }
                                else
                                {
                                    tNota = _notasDAO.LoadByIdMIdUC(lMatriculas[k].IdMatricula, IdUnidadeCurricular).Nota;
                                }

                                tResultado = "10000";


                            }
                            else
                            {
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                lFrequencia = null;
                                tFaltas = 10000;
                                tNota = 10000;
                                tResultado = "10000";
                            }
                            CelFrequenciaAlunos.PaddingBottom = 3;
                            CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_LEFT;
                            CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                            CelFrequenciaAlunos = new PdfPCell();
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            CelFrequenciaAlunos.PaddingBottom = 3;
                            CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                            CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                            /////////////////////Frequencia/////////////////////
                            j = kinicio;
                            b = kfim;

                            if (lFrequencia != null)
                            {
                                while (j < b)
                                {
                                    if (j <= TotalRegistros)
                                    {

                                        CelFrequenciaAlunos = new PdfPCell();
                                        if (IdUnidadeCurricular != 0)
                                        {
                                            if (j < lFrequencia.Count())
                                            {

                                                if (lFrequencia[j].Matricula.Situacao == "Evadido")
                                                {
                                                    if (lFrequencia[j].Matricula.DataSituacao <= lFrequencia[j].Diario.Data)
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));

                                                    }
                                                    else if (lFrequencia[j].Presenca == "TE")
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "T", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));

                                                    }
                                                    else if (lFrequencia[j].Presenca == "AP")
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                    }
                                                    else if (lFrequencia[j].Presenca == "TS")
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "T", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                    }
                                                    else if (lFrequencia[j].Presenca == "A")
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "|", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                    }
                                                    else if (lFrequencia[j].Presenca == "P")
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "•", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                    };

                                                }

                                                else
                                                {
                                                    lAproveitamento = _aproveitamentoDAO.LoadByMatricula(lMatriculas[k]);

                                                    if (lFrequencia[j].Presenca == "TE")
                                                    {                                                                                                                
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "T", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                        
                                                    }
                                                    else if (lFrequencia[j].Presenca == "TS")
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "T", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                    }
                                                    else if (lFrequencia[j].Presenca == "AP")
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                    }
                                                    else if (lFrequencia[j].Presenca == "A")
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "|", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                    }
                                                    else if (lFrequencia[j].Presenca == "P")
                                                    {
                                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "•", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                    };
                                                };

                                            }
                                            else
                                            {
                                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                            }
                                        }
                                        else
                                        {
                                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                        }
                                    }
                                    else
                                    {
                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                    }
                                    CelFrequenciaAlunos.PaddingBottom = 3;
                                    CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                                    CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    tabelaFrequencia.AddCell(CelFrequenciaAlunos);
                                    j++;
                                }
                            }
                            else
                            {
                                while (j < b)
                                {

                                    CelFrequenciaAlunos = new PdfPCell();
                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                    CelFrequenciaAlunos.PaddingBottom = 3;
                                    CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                                    CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    tabelaFrequencia.AddCell(CelFrequenciaAlunos);
                                    j++;
                                }
                            }
                            #endregion

                        /////////////////////FALTAS/////////////////////////

                        #region Frequencia - FALTAS - Somatória

                        CelFrequenciaAlunos = new PdfPCell();
                        //////////////////////
                        if (h != Paginas)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else if (tFaltas == 10000)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tFaltas.ToString("0"), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, n.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequencia = new PdfPCell();
                        #endregion FALTAS

                        /////////////////////NOTAS/////////////////////////

                        #region Frequencia NOTAS - Atribuição

                        if (h != Paginas)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else if (tNota == 10000)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tNota.ToString("0"), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        //if (tResultado == "10000")
                        //{
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        //}
                        //else
                        //{
                        //    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tResultado, new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        //}

                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        k = k + 1;
                    }

                        #endregion Notas


                        #region Frequencia - Rodapé

                        ////////////*Tabela Frequencia - RODAPÉ*////////////////////////////////////////////////////////// 

                        PdfPTable tabelaFrequenciaRodape = new PdfPTable(5);
                        tabelaFrequenciaRodape.TotalWidth = 815;
                        tabelaFrequenciaRodape.LockedWidth = true;
                        float[] widthfr = new float[] { 
                            235, 100, 160, 160, 160  };
                        tabelaFrequenciaRodape.SetWidths(widthfr);

                        PdfPCell CelFrequenciaRadape = new PdfPCell();
                        CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "Representante do Cliente", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape.PaddingBottom = 5;
                        CelFrequenciaRadape.PaddingTop = 15;
                        CelFrequenciaRadape.Border = Image.NO_BORDER;
                        tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                        CelFrequenciaRadape = new PdfPCell();
                        CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape.Rowspan = 4;
                        CelFrequenciaRadape.Border = Image.NO_BORDER;
                        tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                        CelFrequenciaRadape = new PdfPCell();
                        CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Responsável da \nEmpresa", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                        CelFrequenciaRadape.Rowspan = 4;
                        CelFrequenciaRadape.Border = Image.NO_BORDER;
                        tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                        CelFrequenciaRadape = new PdfPCell();
                        CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Docente\n ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                        CelFrequenciaRadape.Rowspan = 4;
                        CelFrequenciaRadape.Border = Image.NO_BORDER;
                        tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                        CelFrequenciaRadape = new PdfPCell();
                        CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Responsável da \nEscola", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                        CelFrequenciaRadape.Rowspan = 4;
                        CelFrequenciaRadape.Border = Image.NO_BORDER;
                        tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                        CelFrequenciaRadape = new PdfPCell();
                        CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "O curso foi realizado conforme apontado neste relatório.\n", new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                        CelFrequenciaRadape.Border = Image.NO_BORDER;
                        tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                        CelFrequenciaRadape = new PdfPCell();
                        CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n   Nome: __________________________________\n", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                        CelFrequenciaRadape.Border = Image.NO_BORDER;
                        tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                        CelFrequenciaRadape = new PdfPCell();
                        CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "   RG:     __________________________________", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                        CelFrequenciaRadape.Border = Image.NO_BORDER;
                        CelFrequenciaRadape.PaddingBottom = 25;
                        tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                        ///////////////////Tabela Rodapé 1////////////////////////////

                        PdfPTable tabelaFrequenciaRodape1 = new PdfPTable(1);
                        tabelaFrequenciaRodape1.WidthPercentage = 100;
                        tabelaFrequenciaRodape1.HorizontalAlignment = Element.ALIGN_LEFT;

                        PdfPCell CelFrequenciaRadape1 = new PdfPCell();
                        CelFrequenciaRadape1.Phrase = new Phrase(alturaLinha3, "      *Aluno Evadido    **Aluno Dispensado    ***Trancamento    ****Aluno Transferido", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape1.Border = Image.TOP_BORDER;
                        CelFrequenciaRadape1.PaddingTop = 5;
                        //CelFrequenciaRadape1.Border = Image.NO_BORDER;
                        tabelaFrequenciaRodape1.AddCell(CelFrequenciaRadape1);

                        CelFrequenciaRadape1 = new PdfPCell();
                        CelFrequenciaRadape1.Phrase = new Phrase(alturaLinha3, "      Resutado Final:       C = Certificado,     N = Não Certificado,    E = Evadido,    T = Transferido", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaRadape1.Border = Image.NO_BORDER;
                        tabelaFrequenciaRodape1.AddCell(CelFrequenciaRadape1);


                        ////////////////////////////////////////////////////////////////

                        #endregion


                        //////////////////////////////////////////////////////////////////////////////////////////
                        ////////////Elementos do Documento PDF a ser Gerado///////////////////////////////////////
                        //////////////////////////////////////////////////////////////////////////////////////////

                        document.Add(tabelaGeral);
                        document.NewPage();
                        document.Add(tabelaFrequencia);
                        document.Add(tabelaFrequenciaRodape);
                        document.Add(tabelaFrequenciaRodape1);
                        document.NewPage();
                        kinicio = kfim;
                        kfim = kfim + 25;
                        inicio = fim;
                        fim = fim + 25;
                        h++;
                    }
                }
                else
                    //Caso a unidade curricular não existe, o usuário escolheu fechamento para imprimir
                {

                    #region Tabela Conteúdo
                    /*Tabela Conteudo*/
                    iTextSharp.text.pdf.PdfPTable tabelaConteudo = new iTextSharp.text.pdf.PdfPTable(3);
                    tabelaConteudo.TotalWidth = 400;
                    tabelaConteudo.LockedWidth = true;
                    float[] widths = new float[] { 50, 50, 300 };
                    tabelaConteudo.SetWidths(widths);
                    float alturaLinha = 11;

                    /*Titulo da Tabela Conteudo*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPCell celTitulo = new iTextSharp.text.pdf.PdfPCell();
                    celTitulo.Phrase = new Phrase(alturaLinha, "DATA", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                    celTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                    celTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celTitulo.PaddingBottom = 5;
                    tabelaConteudo.AddCell(celTitulo);

                    celTitulo = new iTextSharp.text.pdf.PdfPCell();
                    celTitulo.Phrase = new Phrase(alturaLinha, "HORAS", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                    celTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    celTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    celTitulo.PaddingBottom = 5;
                    tabelaConteudo.AddCell(celTitulo);

                    celTitulo = new iTextSharp.text.pdf.PdfPCell();
                    celTitulo.Phrase = new Phrase(alturaLinha, "CONTEÚDO FORMATIVO", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                    celTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    celTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    celTitulo.PaddingBottom = 5;
                    tabelaConteudo.AddCell(celTitulo);

                    /*Conteudo da TabelaConteudo*/
                    for (int e = 0; e < 35; e++)
                    {
                        iTextSharp.text.pdf.PdfPCell celConteudo = new iTextSharp.text.pdf.PdfPCell();
                        celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        celConteudo.PaddingBottom = 5;
                        tabelaConteudo.AddCell(celConteudo);

                        celConteudo = new iTextSharp.text.pdf.PdfPCell();
                        celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        celConteudo.PaddingBottom = 5;
                        tabelaConteudo.AddCell(celConteudo);

                        celConteudo = new iTextSharp.text.pdf.PdfPCell();
                        celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                        celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        celConteudo.PaddingBottom = 5;
                        tabelaConteudo.AddCell(celConteudo);
                    };

                    #endregion

                    #region Tabela Cabeçalho
                    /*Tabela Dados Cabeçalho*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPTable tabelaDadosCabecalho = new iTextSharp.text.pdf.PdfPTable(2);
                    //tabelaConteudo.HorizontalAlignment = Image.ALIGN_LEFT;
                    tabelaDadosCabecalho.TotalWidth = 400;
                    tabelaDadosCabecalho.LockedWidth = true;
                    float[] widthtdc = new float[] { 150, 250 };
                    tabelaDadosCabecalho.SetWidths(widthtdc);

                    /*Inserindo Imagem*/
                    string imageURL = Server.MapPath(".") + "../../../Content/img/" + "logo-senai.jpg";
                    Image logoSenai = Image.GetInstance(imageURL);
                    logoSenai.ScaleToFit(130f, 130f);
                    float alturadalinha2 = 14;

                    iTextSharp.text.pdf.PdfPCell CelDadosTitulo = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTitulo.Border = Image.NO_BORDER;
                    CelDadosTitulo.AddElement(logoSenai);
                    CelDadosTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    CelDadosTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    tabelaDadosCabecalho.AddCell(CelDadosTitulo);

                    CelDadosTitulo = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTitulo.Border = Image.NO_BORDER;
                    CelDadosTitulo.Phrase = new Phrase(8, "ESCOLA SENAI - LENÇÓIS PAULISTA \n" + "Rua Aristeu Rodrigues Sampaio, 271 \n" + "(14) 3269-3969",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelDadosTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tabelaDadosCabecalho.AddCell(CelDadosTitulo);
                    #endregion

                    #region Tabela Dados
                    /*Tabela Dados*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPTable tabelaDados = new iTextSharp.text.pdf.PdfPTable(4);
                    //tabelaConteudo.HorizontalAlignment = Image.ALIGN_LEFT;
                    tabelaDados.TotalWidth = 400;
                    tabelaDados.LockedWidth = true;
                    float[] width = new float[] { 110, 110, 70, 110 };
                    tabelaDados.SetWidths(width);

                    iTextSharp.text.pdf.PdfPCell CelDadosTitulo1 = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTitulo1.Colspan = 4; // either 1 if you need to insert one cell
                    CelDadosTitulo1.Border = Image.NO_BORDER;
                    CelDadosTitulo1.PaddingBottom = 5;
                    CelDadosTitulo1.Phrase = new Phrase(30, "\n\nDIÁRIO DE CLASSE POR UNIDADE CURRICULAR\n" + "FORMAÇÃO INICIAL CONTINUADA\n\n",
                                              new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.BLACK));
                    CelDadosTitulo1.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosTitulo1);


                    iTextSharp.text.pdf.PdfPCell CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.PaddingBottom = 5;
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Nº da Proposta \n" + _turma.PropostaAno,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Local de Realização \n" + _turma.LocalRealizacao,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Atendimento \n" + _turma.Atendimento,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Turma \n" + _turma.Descricao,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    iTextSharp.text.pdf.PdfPCell CelDadosCurso = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCurso.Colspan = 4;
                    CelDadosCurso.PaddingBottom = 5;
                    CelDadosCurso.Phrase = new Phrase(alturadalinha2, "Curso \n" + _turma.Curso.CursoNome,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCurso);

                    iTextSharp.text.pdf.PdfPCell CelDadosUC = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosUC.Colspan = 4;
                    CelDadosUC.PaddingBottom = 5;
                    CelDadosUC.Phrase = new Phrase(alturadalinha2, "Unidade Curricular \n" + _turma.Curso.CursoNome,
                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosUC);

                    iTextSharp.text.pdf.PdfPCell CelDadosDocente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosDocente.Colspan = 4;
                    CelDadosDocente.PaddingBottom = 5;
                    CelDadosDocente.Phrase = new Phrase(alturadalinha2, "Docente \n" + _turma.Funcionario.NomeCompleto,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosDocente);

                    iTextSharp.text.pdf.PdfPCell CelDadosInicio = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosInicio.Colspan = 1;
                    CelDadosInicio.PaddingBottom = 5;
                    CelDadosInicio.Phrase = new Phrase(alturadalinha2, "Início Previsto \n" + _turma.DataInicio.Value.ToShortDateString(),
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosInicio);

                    string Se;
                    string Te;
                    string Qu;
                    string Qui;
                    string Sex;
                    string Sa;
                    string Do;

                    if (_turma.Segunda == true) { Se = "Segunda-Feira - " + _turma.Horario + "\n"; } else { Se = ""; };
                    if (_turma.Terca == true) { Te = "Terça-Feira - " + _turma.Horario + "\n"; } else { Te = ""; };
                    if (_turma.Quarta == true) { Qu = "Quarta-Feira - " + _turma.Horario + "\n"; } else { Qu = ""; };
                    if (_turma.Quinta == true) { Qui = "Quinta-Feira - " + _turma.Horario + "\n"; } else { Qui = ""; };
                    if (_turma.Sexta == true) { Sex = "Sexta-Feira - " + _turma.Horario + "\n"; } else { Sex = ""; };
                    if (_turma.Sabado == true) { Sa = "Sábado - " + _turma.Horario + "\n"; } else { Sa = ""; };
                    if (_turma.Domingo == true) { Do = "Domingo - " + _turma.Horario; } else { Do = ""; };

                    CelDadosInicio = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosInicio.Colspan = 3;
                    CelDadosInicio.Rowspan = 2;
                    CelDadosInicio.Phrase = new Phrase(alturadalinha2, "Dia da Semana e Horários \n" + Se + Te + Qu + Qui + Sex + Sa + Do,
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosInicio);

                    iTextSharp.text.pdf.PdfPCell CelDadosTermino = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTermino.Colspan = 1;
                    CelDadosTermino.PaddingBottom = 5;
                    CelDadosTermino.Phrase = new Phrase(alturadalinha2, "Término Previsto \n" + _turma.DataFim.Value.ToShortDateString(),
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosTermino);

                    decimal HorasPrevista = _turma.Curso.CargaHoraria;

                    iTextSharp.text.pdf.PdfPCell CelDadosHora = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosHora.Colspan = 1;
                    CelDadosHora.PaddingBottom = 5;
                    CelDadosHora.Phrase = (new Phrase(alturadalinha2, "Horas (Previsto)\n" + HorasPrevista,
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    tabelaDados.AddCell(CelDadosHora);

                    CelDadosHora = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosHora.Colspan = 3;
                    CelDadosHora.Phrase = (new Phrase(50, "Responsável na Escola \n" + _turma.Responsavel,
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosHora.HorizontalAlignment = Element.ALIGN_LEFT;
                    CelDadosHora.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaDados.AddCell(CelDadosHora);


                    iTextSharp.text.pdf.PdfPCell CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCliente.Colspan = 4;
                    CelDadosCliente.PaddingBottom = 5;
                    CelDadosCliente.Phrase = (new Phrase(alturadalinha2, "Cliente",
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    CelDadosCliente.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosCliente);

                    CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCliente.Colspan = 4;
                    CelDadosCliente.VerticalAlignment = Image.MIDDLE_ALIGN;
                    CelDadosCliente.Phrase = (new Phrase(10, _turma.Cliente + "\n\n\n\n\n",
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    tabelaDados.AddCell(CelDadosCliente);

                    CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCliente.Colspan = 4;
                    CelDadosCliente.PaddingBottom = 5;
                    CelDadosCliente.Phrase = (new Phrase(alturadalinha2, "Informações para Pagamento do Docente",
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    CelDadosCliente.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosCliente);

                    iTextSharp.text.pdf.PdfPCell CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.PaddingBottom = 5;

                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Período",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    CelDadosPagamento.PaddingBottom = 5;
                    tabelaDados.AddCell(CelDadosPagamento);

                    CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Total de Horas",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosPagamento);

                    CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Valor Hora",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosPagamento);

                    CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Representante Empresa",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosPagamento);

                    int a = 5;
                    int i = 0;
                    while (i < a)
                    {
                        i++;

                        PdfPCell CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.AddElement(new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                        CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                        CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                        CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                    };

                    PdfPCell CelDadosRodape1 = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosRodape1.Colspan = 4;
                    CelDadosRodape1.Phrase = (new Phrase(10, " ",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosRodape1.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape1);


                    PdfPCell CelDadosRodape = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosRodape.Colspan = 1;
                    CelDadosRodape.PaddingBottom = 5;
                    CelDadosRodape.Phrase = (new Phrase(10, "Início Efetivo\n" + lDiario.First().Data.ToShortDateString(),
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosRodape.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape);

                    CelDadosRodape = new PdfPCell();
                    CelDadosRodape.Colspan = 2;
                    CelDadosRodape.Phrase = (new Phrase(10, "Término Efetivo\n" + lDiario.Last().Data.ToShortDateString(),
                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosRodape.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape);

                    CelDadosRodape = new PdfPCell();
                    CelDadosRodape.Colspan = 1;
                    CelDadosRodape.Phrase = (new Phrase(10, "Horas (Realizado)\n" + lDiario.Sum(x => x.HoraAulaDia),
                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));

                    CelDadosRodape.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape);

                    #endregion

                    #region Tabela Geral
                    /*Tabela Geral*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPTable tabelaGeral = new iTextSharp.text.pdf.PdfPTable(2);
                    tabelaGeral.WidthPercentage = 100;

                    iTextSharp.text.pdf.PdfPCell tabConteudo = new iTextSharp.text.pdf.PdfPCell();
                    tabConteudo.Border = Image.RIGHT_BORDER;
                    tabConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tabConteudo.AddElement(tabelaConteudo);

                    tabelaGeral.AddCell(tabConteudo);

                    iTextSharp.text.pdf.PdfPCell tabDados = new iTextSharp.text.pdf.PdfPCell();
                    tabDados.Border = Image.NO_BORDER;
                    tabDados.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tabDados.AddElement(tabelaDadosCabecalho);
                    tabDados.AddElement(tabelaDados);


                    tabelaGeral.AddCell(tabDados);

                    //////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Tabela de Frequencia

                    /*Tabela Frequencia*/
                    ///////////////////////////////////////////////////////// 
                    PdfPTable tabelaFrequencia = new PdfPTable(35);
                    tabelaFrequencia.TotalWidth = 815;
                    tabelaFrequencia.LockedWidth = true;
                    float[] widthf = new float[] { 
                            15, 150, 10, 
                            12, 12, 12, 12, 12, 12, 12, 12,
                            12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                            12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 
                            30, 15, 30, 40 };
                    tabelaFrequencia.SetWidths(widthf);
                    float alturaLinha3 = 10;

                    //1º Linha Cabeçalho
                    PdfPCell CelFrequencia = new iTextSharp.text.pdf.PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nº", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nome do Treinando", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "M", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    int j = 0;
                    int b = 28;
                    //int n = (b - lDiario.Count());
                    while (j < b)
                    {
                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);
                        j++;
                    }

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Faltas\n" + "(Hrs)", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nº", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nota", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Resultado\n" + "Final", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    //2º Linha Cabeçalho
                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "D", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);


                    j = 0;
                    b = 28;
                    while (j < b)
                    {
                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);
                        j++;
                    }


                    ///////////////////////ALunos////////////////////////////////////////////////
                    Matricula _matricula = new Matricula();
                    MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);

                    Notas _notas = new Notas();
                    NotasDAO _notasDAO = new NotasDAO(ref _db);
                    List<Notas> lNotas = new List<Models.Notas>();

                    Frequencia _frequencia = new Frequencia();
                    FrequenciaDAO _frequenciaDAO = new FrequenciaDAO(ref _db);
                    List<Frequencia> lFrequencia = new List<Frequencia>();

                    k = 0;
                    n = 0;
                    tFaltas = 0;
                    tNota = 0;
                    tResultado = " ";
                    Parametro = 25;
                    Perc = 0;
                    lm = (32 - lMatriculas.Count());

                    while (k < (lMatriculas.Count() + lm))
                    {
                        n = k + 1;
                        PdfPCell CelFrequenciaAlunos = new iTextSharp.text.pdf.PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, n.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        
                        int totalHoraAproveitamento = 0;

                        if (k < _turma.ListaMatriculas.Count())
                        {
                            if (lMatriculas[k].Situacao == "Evadido")
                            {
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[k].Aluno.Nome.ToUpper().ToString() + " *" + lMatriculas[k].DataSituacao.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            }
                            else
                            {
                                //Verificar Aproveitamentos no Fechamento
                                lAproveitamento = _aproveitamentoDAO.LoadByMatricula(lMatriculas[k]);
                                    
                                    string ListaAproveitamentos = "";
                                    
                                    if (lAproveitamento.Count() != 0)
                                    {
                                        for (int m = 0; m < lAproveitamento.Count(); m++)
                                        {
                                            ListaAproveitamentos =  ListaAproveitamentos + "(" + lAproveitamento[m].UnidadeCurricular.Sigla.ToString() + ")";
                                            totalHoraAproveitamento = totalHoraAproveitamento + lAproveitamento[m].UnidadeCurricular.CargaHoraria;
                                        }
                                    }
                                    if (ListaAproveitamentos != ""){
                                        ListaAproveitamentos = " **" + ListaAproveitamentos.ToString();
                                    }
                                  CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[k].Aluno.Nome.ToUpper().ToString() + ListaAproveitamentos.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));                               
                                //CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[k].Aluno.Nome.ToUpper().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            }
                            if (IdUnidadeCurricular == 0)
                            {
                                lFrequencia = _frequenciaDAO.LoadByMatricula(lMatriculas[k]).ToList();

                                //tFaltas = (_turma.Curso.CargaHoraria - _frequenciaDAO.LoadByMatricula(lMatriculas[k]).Sum(x => x.HoraAula));

                                //tFaltas = _frequenciaDAO.LoadByMatricula(lMatriculas[k]).Where(x => x.Presenca == "A").Sum(x => x.Diario.HoraAulaDia);

                                if (lMatriculas[k].Situacao == "Evadido")
                                {
                                    tFaltas = lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "A").Sum(x => x.Diario.HoraAulaDia) + (lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TS").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TS").Sum(x => x.HoraAula)) + (lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TE").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TE").Sum(x => x.HoraAula));
                                }
                                else
                                {
                                    tFaltas = lFrequencia.Where(x => x.Presenca == "A").Sum(x => x.Diario.HoraAulaDia) + (lFrequencia.Where(x => x.Presenca == "TS").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Presenca == "TS").Sum(x => x.HoraAula)) + (lFrequencia.Where(x => x.Presenca == "TE").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Presenca == "TE").Sum(x => x.HoraAula));
                                    //tFaltas = (_unidadeCurricularDAO.LoadById(IdUnidadeCurricular).CargaHoraria - _frequenciaDAO.LoadByMatricula(lMatriculas[k]).Where(x => x.Diario.UnidadeCurricular.IdUnidadeCurricular == IdUnidadeCurricular).Sum(x => x.HoraAula));
                                }


                                lNotas = _notasDAO.LoadByMatricula(lMatriculas[k]).ToList();
                                tNota = lNotas.Sum(x => x.Nota) / lUnidadeCurricular.Count();

                                if (lNotas == null)
                                {
                                    tNota = 0;
                                }

                                Perc = (tFaltas / (_turma.Curso.CargaHoraria - totalHoraAproveitamento)) * 100;

                                if (lMatriculas[k].Situacao == "Evadido")
                                {
                                    tResultado = "E";
                                }
                                else if (Perc > Parametro)
                                {
                                    tResultado = "N";
                                }
                                else if (tNota < 50)
                                {
                                    tResultado = "N";
                                }
                                else
                                {
                                    tResultado = "C";
                                };

                            };

                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            lFrequencia = null;
                            tFaltas = 10000;
                            tNota = 10000;
                            tResultado = "10000";
                        }
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_LEFT;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        /////////////////////////////Frequencia//

                        j = 0;
                        b = 28;

                        if (lFrequencia != null)
                        {
                            while (j < b)
                            {

                                CelFrequenciaAlunos = new PdfPCell();
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                CelFrequenciaAlunos.PaddingBottom = 3;
                                CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                                CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tabelaFrequencia.AddCell(CelFrequenciaAlunos);
                                j++;
                            }
                        }
                        else
                        {
                            while (j < b)
                            {

                                CelFrequenciaAlunos = new PdfPCell();
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                CelFrequenciaAlunos.PaddingBottom = 3;
                                CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                                CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tabelaFrequencia.AddCell(CelFrequenciaAlunos);
                                j++;
                            }
                        }

                        CelFrequenciaAlunos = new PdfPCell();
                        if (tFaltas == 10000)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tFaltas.ToString("0"), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, n.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequencia = new PdfPCell();
                        if (tNota == 10000)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tNota.ToString("0"), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        if (tResultado == "10000")
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tResultado, new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }

                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        k = k + 1;
                    }

                    #endregion

                    #region tabela de Frequencia - Rodapé

                    ////////////*Tabela Frequencia - RODAPÉ*////////////////////////////////////////////////////////// 

                    PdfPTable tabelaFrequenciaRodape = new PdfPTable(5);
                    tabelaFrequenciaRodape.TotalWidth = 815;
                    tabelaFrequenciaRodape.LockedWidth = true;
                    float[] widthfr = new float[] { 
                            235, 100, 160, 160, 160  };
                    tabelaFrequenciaRodape.SetWidths(widthfr);

                    PdfPCell CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "Representante do Cliente", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.PaddingBottom = 5;
                    CelFrequenciaRadape.PaddingTop = 15;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Responsável da \nEmpresa", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Docente\n ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Responsável da \nEscola", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "O curso foi realizado conforme apontado neste relatório.\n", new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n   Nome: __________________________________\n", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "   RG:     __________________________________", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    CelFrequenciaRadape.PaddingBottom = 25;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    ///////////////////Tabela Rodapé 1////////////////////////////

                    PdfPTable tabelaFrequenciaRodape1 = new PdfPTable(1);
                    tabelaFrequenciaRodape1.WidthPercentage = 100;
                    tabelaFrequenciaRodape1.HorizontalAlignment = Element.ALIGN_LEFT;

                    PdfPCell CelFrequenciaRadape1 = new PdfPCell();
                    CelFrequenciaRadape1.Phrase = new Phrase(alturaLinha3, "      *Aluno Evadido    **Aluno Dispensado    ***Trancamento    ****Aluno Transferido", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape1.Border = Image.TOP_BORDER;
                    CelFrequenciaRadape1.PaddingTop = 5;
                    //CelFrequenciaRadape1.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape1.AddCell(CelFrequenciaRadape1);

                    CelFrequenciaRadape1 = new PdfPCell();
                    CelFrequenciaRadape1.Phrase = new Phrase(alturaLinha3, "      Resutado Final:       C = Certificado,     N = Não Certificado,    E = Evadido,    T = Transferido", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape1.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape1.AddCell(CelFrequenciaRadape1);


                    ////////////////////////////////////////////////////////////////

                    #endregion


                    //////////////////////////////////////////////////////////////////////////////////////////
                    ////////////Elementos do Documento PDF a ser Gerado///////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////

                    document.Add(tabelaGeral);
                    document.NewPage();
                    document.Add(tabelaFrequencia);
                    document.Add(tabelaFrequenciaRodape);
                    document.Add(tabelaFrequenciaRodape1);


                }
                document.Close();
                writer.Close();
                Response.Clear();

                string Descricao = _unidadecurricular.Descricao;

                if (IdUnidadeCurricular == 0)
                {
                    Descricao = "Fechamento";
                }

                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition", "attachment;filename=" + _turma.Descricao + "_" + Descricao + ".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.End();
                ms.Close();
            }


        }

        //
        //Gerar PDF Download para a Pasta Download do Usuário do Sistema
        private void DownloadAsPDF2(Turma _turma, int IdUnidadeCurricular)
        {

            UnidadeCurricular _unidadecurricular = new UnidadeCurricular();
            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);

            DiarioDAO diarioDAO = new DiarioDAO(ref _db);
            List<Diario> lDiario = new List<Diario>();

            List<Matricula> lMatriculas = _turma.ListaMatriculas.ToList();

            List<UnidadeCurricular> lUnidadeCurricular = _unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList();

            /////////////////////////////////////////////////////////////////////////
            //Gerar PDF e Fazer DOWNLOAD para o Usuário            
            /////////////////////////////////////////////////////////////////////////
            using (MemoryStream ms = new MemoryStream())
            {

                iTextSharp.text.Phrase texto = new Phrase();
                texto = new Phrase(10, "CAPACIDADES TÉCNICAS\n", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));

                iTextSharp.text.Phrase texto2 = new Phrase();
                texto2 = new Phrase(10, "Carlos Cavalheiro\n", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));

                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                document.Open();
                int i = 0;
                int a = 50;
                document.Add(texto2);
                while (i < a)
                {
                    i++;

                    document.Add(texto);
                    if (i == 20)
                    {
                        a = a - i;
                        i = 0;
                        document.NewPage();
                        document.Add(texto2);

                    };
                };

                document.Close();
                writer.Close();
                Response.Clear();

                string Descricao = _unidadecurricular.Descricao;
                if (IdUnidadeCurricular == 0)
                {
                    Descricao = "Fechamento";
                }

                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition", "attachment;filename=" + _turma.Descricao + "_" + Descricao + ".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.End();
                ms.Close();
            }


        }


        //Gerar PDF Download para a Pasta Download do Usuário do Sistema
        private void DownloadAsPDF_Fechamento(int idTurma)
        {
            Turma _turma = new Turma();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);

            _turma = _turmaDAO.LoadById2(idTurma);
            _turma.Diario = _diarioDAO.LoadByConteudo(_turma);

            Aproveitamentos _aproveitamentos = new Aproveitamentos();
            AproveitamentosDAO _aproveitamentoDAO = new AproveitamentosDAO(ref _db);
            List<Aproveitamentos> lAproveitamento = new List<Aproveitamentos>();

            List<Matricula> lMatriculas = _turma.ListaMatriculas.OrderBy(x => x.Aluno.Nome).ToList();
                                    

            /////////////////////////////////////////////////////////////////////////
            //Gerar PDF e Fazer DOWNLOAD para o Usuário            
            /////////////////////////////////////////////////////////////////////////
            using (MemoryStream ms = new MemoryStream())
            {

                Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                int Paginas = lMatriculas.Count() / 32;
                int nAlunos = 0;
                int k = 0;
                int n = 0;
                decimal tFaltas = 0;
                decimal tNota = 0;
                string tResultado = " ";
                int Parametro = 25;
                decimal Perc = 0;
                int lm = lMatriculas.Count();

                for(int h = 0; h <= Paginas; h++)
                {
                    #region Tabela Conteúdo
                    /*Tabela Conteudo*/
                    iTextSharp.text.pdf.PdfPTable tabelaConteudo = new iTextSharp.text.pdf.PdfPTable(3);
                    tabelaConteudo.TotalWidth = 400;
                    tabelaConteudo.LockedWidth = true;
                    float[] widths = new float[] { 50, 50, 300 };
                    tabelaConteudo.SetWidths(widths);
                    float alturaLinha = 11;

                    /*Titulo da Tabela Conteudo*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPCell celTitulo = new iTextSharp.text.pdf.PdfPCell();
                    celTitulo.Phrase = new Phrase(alturaLinha, "DATA", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                    celTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                    celTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celTitulo.PaddingBottom = 5;
                    tabelaConteudo.AddCell(celTitulo);

                    celTitulo = new iTextSharp.text.pdf.PdfPCell();
                    celTitulo.Phrase = new Phrase(alturaLinha, "HORAS", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                    celTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    celTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    celTitulo.PaddingBottom = 5;
                    tabelaConteudo.AddCell(celTitulo);

                    celTitulo = new iTextSharp.text.pdf.PdfPCell();
                    celTitulo.Phrase = new Phrase(alturaLinha, "CONTEÚDO FORMATIVO", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                    celTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    celTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    celTitulo.PaddingBottom = 5;
                    tabelaConteudo.AddCell(celTitulo);

                    /*Conteudo da TabelaConteudo*/
                    for (int e = 0; e < 35; e++)
                    {
                        iTextSharp.text.pdf.PdfPCell celConteudo = new iTextSharp.text.pdf.PdfPCell();
                        celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        celConteudo.PaddingBottom = 5;
                        tabelaConteudo.AddCell(celConteudo);

                        celConteudo = new iTextSharp.text.pdf.PdfPCell();
                        celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        celConteudo.PaddingBottom = 5;
                        tabelaConteudo.AddCell(celConteudo);

                        celConteudo = new iTextSharp.text.pdf.PdfPCell();
                        celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                        celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        celConteudo.PaddingBottom = 5;
                        tabelaConteudo.AddCell(celConteudo);
                    };

                    #endregion

                    #region Tabela Cabeçalho
                    /*Tabela Dados Cabeçalho*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPTable tabelaDadosCabecalho = new iTextSharp.text.pdf.PdfPTable(2);
                    //tabelaConteudo.HorizontalAlignment = Image.ALIGN_LEFT;
                    tabelaDadosCabecalho.TotalWidth = 400;
                    tabelaDadosCabecalho.LockedWidth = true;
                    float[] widthtdc = new float[] { 150, 250 };
                    tabelaDadosCabecalho.SetWidths(widthtdc);

                    /*Inserindo Imagem*/
                    string imageURL = Server.MapPath(".") + "../../../Content/img/" + "logo-senai.jpg";
                    Image logoSenai = Image.GetInstance(imageURL);
                    logoSenai.ScaleToFit(130f, 130f);
                    float alturadalinha2 = 14;

                    iTextSharp.text.pdf.PdfPCell CelDadosTitulo = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTitulo.Border = Image.NO_BORDER;
                    CelDadosTitulo.AddElement(logoSenai);
                    CelDadosTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    CelDadosTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    tabelaDadosCabecalho.AddCell(CelDadosTitulo);

                    CelDadosTitulo = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTitulo.Border = Image.NO_BORDER;
                    CelDadosTitulo.Phrase = new Phrase(8, "ESCOLA SENAI - LENÇÓIS PAULISTA \n" + "Rua Aristeu Rodrigues Sampaio, 271 \n" + "(14) 3269-3969",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelDadosTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tabelaDadosCabecalho.AddCell(CelDadosTitulo);
                    #endregion

                    #region Tabela Dados
                    /*Tabela Dados*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPTable tabelaDados = new iTextSharp.text.pdf.PdfPTable(4);
                    //tabelaConteudo.HorizontalAlignment = Image.ALIGN_LEFT;
                    tabelaDados.TotalWidth = 400;
                    tabelaDados.LockedWidth = true;
                    float[] width = new float[] { 110, 110, 70, 110 };
                    tabelaDados.SetWidths(width);

                    iTextSharp.text.pdf.PdfPCell CelDadosTitulo1 = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTitulo1.Colspan = 4; // either 1 if you need to insert one cell
                    CelDadosTitulo1.Border = Image.NO_BORDER;
                    CelDadosTitulo1.PaddingBottom = 5;
                    CelDadosTitulo1.Phrase = new Phrase(30, "\n\nDIÁRIO DE CLASSE POR UNIDADE CURRICULAR\n" + "FORMAÇÃO INICIAL CONTINUADA\n\n",
                                              new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.BLACK));
                    CelDadosTitulo1.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosTitulo1);


                    iTextSharp.text.pdf.PdfPCell CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.PaddingBottom = 5;
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Nº da Proposta \n" + _turma.PropostaAno,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Local de Realização \n" + _turma.LocalRealizacao,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Atendimento \n" + _turma.Atendimento,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Turma \n" + _turma.Descricao,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    iTextSharp.text.pdf.PdfPCell CelDadosCurso = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCurso.Colspan = 4;
                    CelDadosCurso.PaddingBottom = 5;
                    CelDadosCurso.Phrase = new Phrase(alturadalinha2, "Curso \n" + _turma.Curso.CursoNome,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCurso);

                    iTextSharp.text.pdf.PdfPCell CelDadosUC = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosUC.Colspan = 4;
                    CelDadosUC.PaddingBottom = 5;
                    CelDadosUC.Phrase = new Phrase(alturadalinha2, "Unidade Curricular \n" + _turma.Curso.CursoNome,
                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosUC);

                    iTextSharp.text.pdf.PdfPCell CelDadosDocente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosDocente.Colspan = 4;
                    CelDadosDocente.PaddingBottom = 5;
                    CelDadosDocente.Phrase = new Phrase(alturadalinha2, "Docente \n" + _turma.Funcionario.NomeCompleto,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosDocente);

                    iTextSharp.text.pdf.PdfPCell CelDadosInicio = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosInicio.Colspan = 1;
                    CelDadosInicio.PaddingBottom = 5;
                    CelDadosInicio.Phrase = new Phrase(alturadalinha2, "Início Previsto \n" + _turma.DataInicio.Value.ToShortDateString(),
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosInicio);

                    string Se;
                    string Te;
                    string Qu;
                    string Qui;
                    string Sex;
                    string Sa;
                    string Do;

                    if (_turma.Segunda == true) { Se = "Segunda-Feira - " + _turma.Horario + "\n"; } else { Se = ""; };
                    if (_turma.Terca == true) { Te = "Terça-Feira - " + _turma.Horario + "\n"; } else { Te = ""; };
                    if (_turma.Quarta == true) { Qu = "Quarta-Feira - " + _turma.Horario + "\n"; } else { Qu = ""; };
                    if (_turma.Quinta == true) { Qui = "Quinta-Feira - " + _turma.Horario + "\n"; } else { Qui = ""; };
                    if (_turma.Sexta == true) { Sex = "Sexta-Feira - " + _turma.Horario + "\n"; } else { Sex = ""; };
                    if (_turma.Sabado == true) { Sa = "Sábado - " + _turma.Horario + "\n"; } else { Sa = ""; };
                    if (_turma.Domingo == true) { Do = "Domingo - " + _turma.Horario; } else { Do = ""; };

                    CelDadosInicio = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosInicio.Colspan = 3;
                    CelDadosInicio.Rowspan = 2;
                    CelDadosInicio.Phrase = new Phrase(alturadalinha2, "Dia da Semana e Horários \n" + Se + Te + Qu + Qui + Sex + Sa + Do,
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosInicio);

                    iTextSharp.text.pdf.PdfPCell CelDadosTermino = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTermino.Colspan = 1;
                    CelDadosTermino.PaddingBottom = 5;
                    CelDadosTermino.Phrase = new Phrase(alturadalinha2, "Término Previsto \n" + _turma.DataFim.Value.ToShortDateString(),
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosTermino);

                    decimal HorasPrevista = _turma.Curso.CargaHoraria;

                    iTextSharp.text.pdf.PdfPCell CelDadosHora = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosHora.Colspan = 1;
                    CelDadosHora.PaddingBottom = 5;
                    CelDadosHora.Phrase = (new Phrase(alturadalinha2, "Horas (Previsto)\n" + HorasPrevista,
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    tabelaDados.AddCell(CelDadosHora);

                    CelDadosHora = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosHora.Colspan = 3;
                    CelDadosHora.Phrase = (new Phrase(50, "Responsável na Escola \n" + _turma.Responsavel,
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosHora.HorizontalAlignment = Element.ALIGN_LEFT;
                    CelDadosHora.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaDados.AddCell(CelDadosHora);


                    iTextSharp.text.pdf.PdfPCell CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCliente.Colspan = 4;
                    CelDadosCliente.PaddingBottom = 5;
                    CelDadosCliente.Phrase = (new Phrase(alturadalinha2, "Cliente",
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    CelDadosCliente.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosCliente);

                    CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCliente.Colspan = 4;
                    CelDadosCliente.VerticalAlignment = Image.MIDDLE_ALIGN;
                    CelDadosCliente.Phrase = (new Phrase(10, _turma.Cliente + "\n\n\n\n\n",
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    tabelaDados.AddCell(CelDadosCliente);

                    CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCliente.Colspan = 4;
                    CelDadosCliente.PaddingBottom = 5;
                    CelDadosCliente.Phrase = (new Phrase(alturadalinha2, "Informações para Pagamento do Docente",
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    CelDadosCliente.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosCliente);

                    iTextSharp.text.pdf.PdfPCell CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.PaddingBottom = 5;

                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Período",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    CelDadosPagamento.PaddingBottom = 5;
                    tabelaDados.AddCell(CelDadosPagamento);

                    CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Total de Horas",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosPagamento);

                    CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Valor Hora",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosPagamento);

                    CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Representante Empresa",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosPagamento);

                    int a = 5;
                    int i = 0;
                    while (i < a)
                    {
                        i++;

                        PdfPCell CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.AddElement(new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                        CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                        CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                        CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                    };

                    PdfPCell CelDadosRodape1 = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosRodape1.Colspan = 4;
                    CelDadosRodape1.Phrase = (new Phrase(10, " ",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosRodape1.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape1);


                    PdfPCell CelDadosRodape = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosRodape.Colspan = 1;
                    CelDadosRodape.PaddingBottom = 5;
                    CelDadosRodape.Phrase = (new Phrase(10, "Início Efetivo\n" + _turma.Diario.First().Data.ToShortDateString(),
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosRodape.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape);

                    CelDadosRodape = new PdfPCell();
                    CelDadosRodape.Colspan = 2;
                    CelDadosRodape.Phrase = (new Phrase(10, "Término Efetivo\n" + _turma.Diario.Last().Data.ToShortDateString(),
                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosRodape.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape);

                    CelDadosRodape = new PdfPCell();
                    CelDadosRodape.Colspan = 1;
                    CelDadosRodape.Phrase = (new Phrase(10, "Horas (Realizado)\n" + _turma.Diario.Sum(x => x.HoraAulaDia),
                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));

                    CelDadosRodape.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape);

                    #endregion

                    #region Tabela Geral
                    /*Tabela Geral*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPTable tabelaGeral = new iTextSharp.text.pdf.PdfPTable(2);
                    tabelaGeral.WidthPercentage = 100;

                    iTextSharp.text.pdf.PdfPCell tabConteudo = new iTextSharp.text.pdf.PdfPCell();
                    tabConteudo.Border = Image.RIGHT_BORDER;
                    tabConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tabConteudo.AddElement(tabelaConteudo);

                    tabelaGeral.AddCell(tabConteudo);

                    iTextSharp.text.pdf.PdfPCell tabDados = new iTextSharp.text.pdf.PdfPCell();
                    tabDados.Border = Image.NO_BORDER;
                    tabDados.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tabDados.AddElement(tabelaDadosCabecalho);
                    tabDados.AddElement(tabelaDados);


                    tabelaGeral.AddCell(tabDados);

                    //////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Tabela de Frequencia

                    /*Tabela Frequencia*/
                    ///////////////////////////////////////////////////////// 
                    PdfPTable tabelaFrequencia = new PdfPTable(35);
                    tabelaFrequencia.TotalWidth = 815;
                    tabelaFrequencia.LockedWidth = true;
                    float[] widthf = new float[] {
                            15, 150, 10,
                            12, 12, 12, 12, 12, 12, 12, 12,
                            12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                            12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                            30, 15, 30, 40 };
                    tabelaFrequencia.SetWidths(widthf);
                    float alturaLinha3 = 10;

                    //1º Linha Cabeçalho
                    PdfPCell CelFrequencia = new iTextSharp.text.pdf.PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nº", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nome do Treinando", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "M", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    int j = 0;
                    int b = 28;
                    //int n = (b - lDiario.Count());

                    while (j < b)
                    {
                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);
                        j++;
                    }

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Faltas\n" + "(Hrs)", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nº", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nota", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Resultado\n" + "Final", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    //2º Linha Cabeçalho
                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "D", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);


                    j = 0;
                    b = 28;

                    while (j < b)
                    {
                        CelFrequencia = new PdfPCell();
                        CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);
                        j++;
                    }
                    #endregion

                    #region Alunos, Notas, Faltas

                    Matricula _matricula = new Matricula();
                    MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);

                    Notas _notas = new Notas();
                    NotasDAO _notasDAO = new NotasDAO(ref _db);
                    List<Notas> lNotas = new List<Models.Notas>();

                    Frequencia _frequencia = new Frequencia();
                    FrequenciaDAO _frequenciaDAO = new FrequenciaDAO(ref _db);
                    List<Frequencia> lFrequencia = new List<Frequencia>();

                    k = 0;
                    n = 0;
                    tFaltas = 0;
                    tNota = 0;
                    tResultado = " ";
                    Parametro = 25;
                    Perc = 0;
                    //lm = lMatriculas.Count();

                    while (k < 32)
                    {
                        n = nAlunos + 1;
                        PdfPCell CelFrequenciaAlunos = new iTextSharp.text.pdf.PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, n.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();

                        int totalHoraAproveitamento = 0;

                        if (nAlunos < _turma.ListaMatriculas.Count())
                        {
                            if (lMatriculas[nAlunos].Situacao == "Evadido")
                            {
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[nAlunos].Aluno.Nome.ToUpper().ToString() + " *" + lMatriculas[nAlunos].DataSituacao.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            }
                            else
                            {
                                //Verificar Aproveitamentos no Fechamento
                                lAproveitamento = _aproveitamentoDAO.LoadByMatricula(lMatriculas[nAlunos]);

                                string ListaAproveitamentos = "";

                                if (lAproveitamento.Count() != 0)
                                {
                                    for (int m = 0; m < lAproveitamento.Count(); m++)
                                    {
                                        ListaAproveitamentos = ListaAproveitamentos + "(" + lAproveitamento[m].UnidadeCurricular.Sigla.ToString() + ")";
                                        totalHoraAproveitamento = totalHoraAproveitamento + lAproveitamento[m].UnidadeCurricular.CargaHoraria;
                                    }
                                }
                                if (ListaAproveitamentos != "")
                                {
                                    ListaAproveitamentos = " **" + ListaAproveitamentos.ToString();
                                }
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[nAlunos].Aluno.Nome.ToUpper().ToString() + ListaAproveitamentos.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            }

                            lFrequencia = _frequenciaDAO.LoadByMatricula(lMatriculas[nAlunos]).ToList();

                            if (lMatriculas[nAlunos].Situacao == "Evadido")
                            {
                                tFaltas = lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "A").Sum(x => x.Diario.HoraAulaDia) + (lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TS").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TS").Sum(x => x.HoraAula)) + (lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TE").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TE").Sum(x => x.HoraAula));
                            }
                            else
                            {
                                tFaltas = lFrequencia.Where(x => x.Presenca == "A").Sum(x => x.Diario.HoraAulaDia) + (lFrequencia.Where(x => x.Presenca == "TS").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Presenca == "TS").Sum(x => x.HoraAula)) + (lFrequencia.Where(x => x.Presenca == "TE").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Presenca == "TE").Sum(x => x.HoraAula));
                            }

                            
                            //NOTAS

                            lNotas = _notasDAO.LoadByMatricula(lMatriculas[nAlunos]).ToList();
                            tNota = lNotas.Sum(x => x.Nota) / _turma.Curso.lUnidadesCurriculares.Count();

                            if (lNotas == null)
                            {
                                tNota = 0;
                            }

                            Perc = (tFaltas / (_turma.Curso.CargaHoraria - totalHoraAproveitamento)) * 100;

                            if (lMatriculas[nAlunos].Situacao == "Evadido")
                            {
                                tResultado = "E";
                            }
                            else if (Perc > Parametro)
                            {
                                tResultado = "N";
                            }
                            else if (tNota < 50)
                            {
                                tResultado = "N";
                            }
                            else
                            {
                                tResultado = "C";
                            };

                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            lFrequencia = null;
                            tFaltas = 10000;
                            tNota = 10000;
                            tResultado = "10000";
                        }

                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_LEFT;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        //Frequencia

                        j = 0;
                        b = 28;
                        if (lFrequencia != null)
                        {
                            while (j < b)
                            {

                                CelFrequenciaAlunos = new PdfPCell();
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                CelFrequenciaAlunos.PaddingBottom = 3;
                                CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                                CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tabelaFrequencia.AddCell(CelFrequenciaAlunos);
                                j++;
                            }
                        }
                        else
                        {
                            while (j < b)
                            {

                                CelFrequenciaAlunos = new PdfPCell();
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                CelFrequenciaAlunos.PaddingBottom = 3;
                                CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                                CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tabelaFrequencia.AddCell(CelFrequenciaAlunos);
                                j++;
                            }
                        }

                        CelFrequenciaAlunos = new PdfPCell();
                        if (tFaltas == 10000)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tFaltas.ToString("0"), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, n.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequencia = new PdfPCell();
                        if (tNota == 10000)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tNota.ToString("0"), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        if (tResultado == "10000")
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tResultado, new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }

                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        k++;
                        nAlunos++;
                    }
                    #endregion

                    #region tabela de Frequencia - Rodapé

                    ////////////*Tabela Frequencia - RODAPÉ*////////////////////////////////////////////////////////// 

                    PdfPTable tabelaFrequenciaRodape = new PdfPTable(5);
                    tabelaFrequenciaRodape.TotalWidth = 815;
                    tabelaFrequenciaRodape.LockedWidth = true;
                    float[] widthfr = new float[] {
                            235, 100, 160, 160, 160  };
                    tabelaFrequenciaRodape.SetWidths(widthfr);

                    PdfPCell CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "Representante do Cliente", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.PaddingBottom = 5;
                    CelFrequenciaRadape.PaddingTop = 15;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Responsável da \nEmpresa", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Docente\n ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Responsável da \nEscola", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "O curso foi realizado conforme apontado neste relatório.\n", new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n   Nome: __________________________________\n", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "   RG:     __________________________________", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    CelFrequenciaRadape.PaddingBottom = 25;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    ///////////////////Tabela Rodapé 1////////////////////////////

                    PdfPTable tabelaFrequenciaRodape1 = new PdfPTable(1);
                    tabelaFrequenciaRodape1.WidthPercentage = 100;
                    tabelaFrequenciaRodape1.HorizontalAlignment = Element.ALIGN_LEFT;

                    PdfPCell CelFrequenciaRadape1 = new PdfPCell();
                    CelFrequenciaRadape1.Phrase = new Phrase(alturaLinha3, "      *Aluno Evadido    **Aluno Dispensado    ***Trancamento    ****Aluno Transferido", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape1.Border = Image.TOP_BORDER;
                    CelFrequenciaRadape1.PaddingTop = 5;
                    //CelFrequenciaRadape1.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape1.AddCell(CelFrequenciaRadape1);

                    CelFrequenciaRadape1 = new PdfPCell();
                    CelFrequenciaRadape1.Phrase = new Phrase(alturaLinha3, "      Resutado Final:       C = Certificado,     N = Não Certificado,    E = Evadido,    T = Transferido", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape1.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape1.AddCell(CelFrequenciaRadape1);


                    ////////////////////////////////////////////////////////////////

                    #endregion


                    //////////////////////////////////////////////////////////////////////////////////////////
                    ////////////Elementos do Documento PDF a ser Gerado///////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////

                    document.Add(tabelaGeral);
                    document.NewPage();
                    document.Add(tabelaFrequencia);
                    document.Add(tabelaFrequenciaRodape);
                    document.Add(tabelaFrequenciaRodape1);
                }
                    

                document.Close();
                writer.Close();
                Response.Clear();

                string Descricao = "Fechamento";                

                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition", "attachment;filename=" + _turma.Descricao + "_" + Descricao + ".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.End();
                ms.Close();
            }


        }

        //Gerar PDF Download para a Pasta Download do Usuário do Sistema
        private void DownloadAsPDF_Unidade(int idTurma, int IdUnidadeCurricular)
        {
            Turma _turma = new Turma();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

            UnidadeCurricular _unidadecurricular = new UnidadeCurricular();
            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);

            _unidadecurricular = _unidadeCurricularDAO.LoadById(IdUnidadeCurricular);

            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);
            List<Diario> lDiario = new List<Diario>();

            _turma = _turmaDAO.LoadById2(idTurma);
            _turma.Diario = _diarioDAO.LoadByConteudo(_turma);

            lDiario = _diarioDAO.LoadByConteudo(_turma).OrderBy(x => x.Data).Where(x => x.UnidadeCurricular.IdUnidadeCurricular == IdUnidadeCurricular).ToList();
 
            Aproveitamentos _aproveitamentos = new Aproveitamentos();
            AproveitamentosDAO _aproveitamentoDAO = new AproveitamentosDAO(ref _db);
            List<Aproveitamentos> lAproveitamento = new List<Aproveitamentos>();

            List<Matricula> lMatriculas = _turma.ListaMatriculas.OrderBy(x => x.Aluno.Nome).ToList();


            /////////////////////////////////////////////////////////////////////////
            //Gerar PDF e Fazer DOWNLOAD para o Usuário            
            /////////////////////////////////////////////////////////////////////////
            using (MemoryStream ms = new MemoryStream())
            {

                Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                int Paginas = lMatriculas.Count() / 32;
                int nAlunos = 0;
                int k = 0;
                int n = 0;
                int nc = 0;
                decimal tFaltas = 0;
                decimal tNota = 0;
                string tResultado = " ";
                int Parametro = 25;
                decimal Perc = 0;
                int lm = lMatriculas.Count();

                for (int h = 0; h <= Paginas; h++)
                {
                    #region Tabela Conteúdo
                    /*Tabela Conteudo*/
                    iTextSharp.text.pdf.PdfPTable tabelaConteudo = new iTextSharp.text.pdf.PdfPTable(3);
                    tabelaConteudo.TotalWidth = 400;
                    tabelaConteudo.LockedWidth = true;
                    float[] widths = new float[] { 50, 50, 300 };
                    tabelaConteudo.SetWidths(widths);
                    float alturaLinha = 11;

                    /*Titulo da Tabela Conteudo*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPCell celTitulo = new iTextSharp.text.pdf.PdfPCell();
                    celTitulo.Phrase = new Phrase(alturaLinha, "DATA", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                    celTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                    celTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celTitulo.PaddingBottom = 5;
                    tabelaConteudo.AddCell(celTitulo);

                    celTitulo = new iTextSharp.text.pdf.PdfPCell();
                    celTitulo.Phrase = new Phrase(alturaLinha, "HORAS", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                    celTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    celTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    celTitulo.PaddingBottom = 5;
                    tabelaConteudo.AddCell(celTitulo);

                    celTitulo = new iTextSharp.text.pdf.PdfPCell();
                    celTitulo.Phrase = new Phrase(alturaLinha, "CONTEÚDO FORMATIVO", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK));
                    celTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    celTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    celTitulo.PaddingBottom = 5;
                    tabelaConteudo.AddCell(celTitulo);

                    /*Conteudo da TabelaConteudo*/
                    for (int e = 0; e < 35; e++)
                    {

                        if (e < lDiario.Count())
                        {
                            iTextSharp.text.pdf.PdfPCell celConteudo = new iTextSharp.text.pdf.PdfPCell();
                            celConteudo.Phrase = new Phrase(alturaLinha, lDiario[e].Data.Day.ToString() + "/" + lDiario[e].Data.Month.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                            celConteudo.PaddingBottom = 5;
                            tabelaConteudo.AddCell(celConteudo);

                            celConteudo = new iTextSharp.text.pdf.PdfPCell();
                            celConteudo.Phrase = new Phrase(alturaLinha, lDiario[e].HoraAulaDia.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                            celConteudo.PaddingBottom = 5;
                            tabelaConteudo.AddCell(celConteudo);

                            celConteudo = new iTextSharp.text.pdf.PdfPCell();
                            celConteudo.Phrase = new Phrase(alturaLinha, lDiario[e].Conteudo.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                            celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                            celConteudo.PaddingBottom = 5;
                            tabelaConteudo.AddCell(celConteudo);
                        }
                        else
                        {
                            iTextSharp.text.pdf.PdfPCell celConteudo = new iTextSharp.text.pdf.PdfPCell();
                            celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                            celConteudo.PaddingBottom = 5;
                            tabelaConteudo.AddCell(celConteudo);

                            celConteudo = new iTextSharp.text.pdf.PdfPCell();
                            celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                            celConteudo.PaddingBottom = 5;
                            tabelaConteudo.AddCell(celConteudo);

                            celConteudo = new iTextSharp.text.pdf.PdfPCell();
                            celConteudo.Phrase = new Phrase(alturaLinha, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            celConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                            celConteudo.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                            celConteudo.PaddingBottom = 5;
                            tabelaConteudo.AddCell(celConteudo);
                        }
                        nc++;
                    };

                    #endregion

                    #region Tabela Cabeçalho
                    /*Tabela Dados Cabeçalho*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPTable tabelaDadosCabecalho = new iTextSharp.text.pdf.PdfPTable(2);
                    //tabelaConteudo.HorizontalAlignment = Image.ALIGN_LEFT;
                    tabelaDadosCabecalho.TotalWidth = 400;
                    tabelaDadosCabecalho.LockedWidth = true;
                    float[] widthtdc = new float[] { 150, 250 };
                    tabelaDadosCabecalho.SetWidths(widthtdc);

                    /*Inserindo Imagem*/
                    string imageURL = Server.MapPath(".") + "../../../Content/img/" + "logo-senai.jpg";
                    Image logoSenai = Image.GetInstance(imageURL);
                    logoSenai.ScaleToFit(130f, 130f);
                    float alturadalinha2 = 14;

                    iTextSharp.text.pdf.PdfPCell CelDadosTitulo = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTitulo.Border = Image.NO_BORDER;
                    CelDadosTitulo.AddElement(logoSenai);
                    CelDadosTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    CelDadosTitulo.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    tabelaDadosCabecalho.AddCell(CelDadosTitulo);

                    CelDadosTitulo = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTitulo.Border = Image.NO_BORDER;
                    CelDadosTitulo.Phrase = new Phrase(8, "ESCOLA SENAI - LENÇÓIS PAULISTA \n" + "Rua Aristeu Rodrigues Sampaio, 271 \n" + "(14) 3269-3969",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelDadosTitulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tabelaDadosCabecalho.AddCell(CelDadosTitulo);
                    #endregion

                    #region Tabela Dados
                    /*Tabela Dados*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPTable tabelaDados = new iTextSharp.text.pdf.PdfPTable(4);
                    //tabelaConteudo.HorizontalAlignment = Image.ALIGN_LEFT;
                    tabelaDados.TotalWidth = 400;
                    tabelaDados.LockedWidth = true;
                    float[] width = new float[] { 110, 110, 70, 110 };
                    tabelaDados.SetWidths(width);

                    iTextSharp.text.pdf.PdfPCell CelDadosTitulo1 = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTitulo1.Colspan = 4; // either 1 if you need to insert one cell
                    CelDadosTitulo1.Border = Image.NO_BORDER;
                    CelDadosTitulo1.PaddingBottom = 5;
                    CelDadosTitulo1.Phrase = new Phrase(30, "\n\nDIÁRIO DE CLASSE POR UNIDADE CURRICULAR\n" + "FORMAÇÃO INICIAL CONTINUADA\n\n",
                                              new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.BLACK));
                    CelDadosTitulo1.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosTitulo1);


                    iTextSharp.text.pdf.PdfPCell CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.PaddingBottom = 5;
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Nº da Proposta \n" + _turma.PropostaAno,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Local de Realização \n" + _turma.LocalRealizacao,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Atendimento \n" + _turma.Atendimento,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    CelDadosCabecalho = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCabecalho.Phrase = new Phrase(alturadalinha2, "Turma \n" + _turma.Descricao,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCabecalho);

                    iTextSharp.text.pdf.PdfPCell CelDadosCurso = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCurso.Colspan = 4;
                    CelDadosCurso.PaddingBottom = 5;
                    CelDadosCurso.Phrase = new Phrase(alturadalinha2, "Curso \n" + _turma.Curso.CursoNome,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosCurso);

                    iTextSharp.text.pdf.PdfPCell CelDadosUC = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosUC.Colspan = 4;
                    CelDadosUC.PaddingBottom = 5;
                    CelDadosUC.Phrase = new Phrase(alturadalinha2, "Unidade Curricular \n" + _turma.Curso.CursoNome,
                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosUC);

                    iTextSharp.text.pdf.PdfPCell CelDadosDocente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosDocente.Colspan = 4;
                    CelDadosDocente.PaddingBottom = 5;
                    CelDadosDocente.Phrase = new Phrase(alturadalinha2, "Docente \n" + _turma.Funcionario.NomeCompleto,
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosDocente);

                    iTextSharp.text.pdf.PdfPCell CelDadosInicio = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosInicio.Colspan = 1;
                    CelDadosInicio.PaddingBottom = 5;
                    CelDadosInicio.Phrase = new Phrase(alturadalinha2, "Início Previsto \n" + _turma.DataInicio.Value.ToShortDateString(),
                                              new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosInicio);

                    string Se;
                    string Te;
                    string Qu;
                    string Qui;
                    string Sex;
                    string Sa;
                    string Do;

                    if (_turma.Segunda == true) { Se = "Segunda-Feira - " + _turma.Horario + "\n"; } else { Se = ""; };
                    if (_turma.Terca == true) { Te = "Terça-Feira - " + _turma.Horario + "\n"; } else { Te = ""; };
                    if (_turma.Quarta == true) { Qu = "Quarta-Feira - " + _turma.Horario + "\n"; } else { Qu = ""; };
                    if (_turma.Quinta == true) { Qui = "Quinta-Feira - " + _turma.Horario + "\n"; } else { Qui = ""; };
                    if (_turma.Sexta == true) { Sex = "Sexta-Feira - " + _turma.Horario + "\n"; } else { Sex = ""; };
                    if (_turma.Sabado == true) { Sa = "Sábado - " + _turma.Horario + "\n"; } else { Sa = ""; };
                    if (_turma.Domingo == true) { Do = "Domingo - " + _turma.Horario; } else { Do = ""; };

                    CelDadosInicio = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosInicio.Colspan = 3;
                    CelDadosInicio.Rowspan = 2;
                    CelDadosInicio.Phrase = new Phrase(alturadalinha2, "Dia da Semana e Horários \n" + Se + Te + Qu + Qui + Sex + Sa + Do,
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosInicio);

                    iTextSharp.text.pdf.PdfPCell CelDadosTermino = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosTermino.Colspan = 1;
                    CelDadosTermino.PaddingBottom = 5;
                    CelDadosTermino.Phrase = new Phrase(alturadalinha2, "Término Previsto \n" + _turma.DataFim.Value.ToShortDateString(),
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                    tabelaDados.AddCell(CelDadosTermino);

                    decimal HorasPrevista = _turma.Curso.CargaHoraria;

                    iTextSharp.text.pdf.PdfPCell CelDadosHora = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosHora.Colspan = 1;
                    CelDadosHora.PaddingBottom = 5;
                    CelDadosHora.Phrase = (new Phrase(alturadalinha2, "Horas (Previsto)\n" + HorasPrevista,
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    tabelaDados.AddCell(CelDadosHora);

                    CelDadosHora = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosHora.Colspan = 3;
                    CelDadosHora.Phrase = (new Phrase(50, "Responsável na Escola \n" + _turma.Responsavel,
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosHora.HorizontalAlignment = Element.ALIGN_LEFT;
                    CelDadosHora.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaDados.AddCell(CelDadosHora);


                    iTextSharp.text.pdf.PdfPCell CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCliente.Colspan = 4;
                    CelDadosCliente.PaddingBottom = 5;
                    CelDadosCliente.Phrase = (new Phrase(alturadalinha2, "Cliente",
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    CelDadosCliente.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosCliente);

                    CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCliente.Colspan = 4;
                    CelDadosCliente.VerticalAlignment = Image.MIDDLE_ALIGN;
                    CelDadosCliente.Phrase = (new Phrase(10, _turma.Cliente + "\n\n\n\n\n",
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    tabelaDados.AddCell(CelDadosCliente);

                    CelDadosCliente = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosCliente.Colspan = 4;
                    CelDadosCliente.PaddingBottom = 5;
                    CelDadosCliente.Phrase = (new Phrase(alturadalinha2, "Informações para Pagamento do Docente",
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    CelDadosCliente.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosCliente);

                    iTextSharp.text.pdf.PdfPCell CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.PaddingBottom = 5;

                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Período",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    CelDadosPagamento.PaddingBottom = 5;
                    tabelaDados.AddCell(CelDadosPagamento);

                    CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Total de Horas",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosPagamento);

                    CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Valor Hora",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosPagamento);

                    CelDadosPagamento = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosPagamento.Phrase = (new Phrase(alturadalinha2, "Representante Empresa",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosPagamento.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosPagamento);

                    int a = 5;
                    int i = 0;
                    while (i < a)
                    {
                        i++;

                        PdfPCell CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.AddElement(new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                        CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                        CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                        CelDadosPagamento1 = new iTextSharp.text.pdf.PdfPCell();
                        CelDadosPagamento1.Phrase = (new Phrase(20, " ",
                                                  new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                        CelDadosPagamento1.HorizontalAlignment = 1;
                        tabelaDados.AddCell(CelDadosPagamento1);

                    };

                    PdfPCell CelDadosRodape1 = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosRodape1.Colspan = 4;
                    CelDadosRodape1.Phrase = (new Phrase(10, " ",
                                              new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosRodape1.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape1);


                    PdfPCell CelDadosRodape = new iTextSharp.text.pdf.PdfPCell();
                    CelDadosRodape.Colspan = 1;
                    CelDadosRodape.PaddingBottom = 5;
                    CelDadosRodape.Phrase = (new Phrase(10, "Início Efetivo\n" + _turma.Diario.First().Data.ToShortDateString(),
                                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosRodape.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape);

                    CelDadosRodape = new PdfPCell();
                    CelDadosRodape.Colspan = 2;
                    CelDadosRodape.Phrase = (new Phrase(10, "Término Efetivo\n" + _turma.Diario.Last().Data.ToShortDateString(),
                              new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));
                    CelDadosRodape.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape);

                    CelDadosRodape = new PdfPCell();
                    CelDadosRodape.Colspan = 1;
                    CelDadosRodape.Phrase = (new Phrase(10, "Horas (Realizado)\n" + _turma.Diario.Sum(x => x.HoraAulaDia),
                                  new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK)));

                    CelDadosRodape.HorizontalAlignment = 1;
                    tabelaDados.AddCell(CelDadosRodape);

                    #endregion

                    #region Tabela Geral
                    /*Tabela Geral*/
                    /////////////////////////////////////////////////////////////
                    iTextSharp.text.pdf.PdfPTable tabelaGeral = new iTextSharp.text.pdf.PdfPTable(2);
                    tabelaGeral.WidthPercentage = 100;

                    iTextSharp.text.pdf.PdfPCell tabConteudo = new iTextSharp.text.pdf.PdfPCell();
                    tabConteudo.Border = Image.RIGHT_BORDER;
                    tabConteudo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tabConteudo.AddElement(tabelaConteudo);

                    tabelaGeral.AddCell(tabConteudo);

                    iTextSharp.text.pdf.PdfPCell tabDados = new iTextSharp.text.pdf.PdfPCell();
                    tabDados.Border = Image.NO_BORDER;
                    tabDados.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tabDados.AddElement(tabelaDadosCabecalho);
                    tabDados.AddElement(tabelaDados);


                    tabelaGeral.AddCell(tabDados);

                    //////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Tabela de Frequencia e Dias de aula

                    /*Tabela Frequencia*/
                    ///////////////////////////////////////////////////////// 
                    PdfPTable tabelaFrequencia = new PdfPTable(35);
                    tabelaFrequencia.TotalWidth = 815;
                    tabelaFrequencia.LockedWidth = true;
                    float[] widthf = new float[] {
                            15, 150, 10,
                            12, 12, 12, 12, 12, 12, 12, 12,
                            12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                            12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                            30, 15, 30, 40 };
                    tabelaFrequencia.SetWidths(widthf);
                    float alturaLinha3 = 10;

                    #region 1 Linha Cabeçalho
                    //1º Linha Cabeçalho//////////////////////////////////////////////

                    PdfPCell CelFrequencia = new iTextSharp.text.pdf.PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nº", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nome do Treinando", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "M", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    int j = 0;
                    int b = 28;


                    while (j < b)
                    {
                        if (j <= lDiario.Count())
                        {
                            CelFrequencia = new PdfPCell();
                            if (IdUnidadeCurricular != 0)
                            {
                                if (j < lDiario.Count())
                                {
                                    CelFrequencia.Phrase = new Phrase(alturaLinha3, lDiario[j].Data.Month.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                }
                                else
                                {
                                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                }
                            }
                            else
                            {
                                CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            };

                        }
                        else
                        {
                            CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);
                        j++;
                    }


                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Faltas\n" + "(Hrs)", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nº", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Nota", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);

                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "Resultado\n" + "Final", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.Rowspan = 2;
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);
                    #endregion

                    #region 2 Linha Cabeçalho                                               
                    //2º Linha Cabeçalho
                    CelFrequencia = new PdfPCell();
                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "D", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequencia.PaddingBottom = 3;
                    CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabelaFrequencia.AddCell(CelFrequencia);


                    j = 0;
                    b = 28;
                    while (j < b)
                    {
                        CelFrequencia = new PdfPCell();
                        if (j <= lDiario.Count())
                        {
                            if (IdUnidadeCurricular != 0)
                            {
                                if (j < lDiario.Count())
                                {
                                    CelFrequencia.Phrase = new Phrase(alturaLinha3, lDiario[j].Data.Day.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                }
                                else
                                {
                                    CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                                };
                            }
                            else
                            {
                                CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));

                            };
                        }
                        else
                        {
                            CelFrequencia.Phrase = new Phrase(alturaLinha3, "", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequencia.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequencia);
                        j++;
                    }

                    #endregion

                    #endregion

                    #region Alunos, Notas, Faltas

                    Matricula _matricula = new Matricula();
                    MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);

                    Notas _notas = new Notas();
                    NotasDAO _notasDAO = new NotasDAO(ref _db);
                    List<Notas> lNotas = new List<Models.Notas>();

                    Frequencia _frequencia = new Frequencia();
                    FrequenciaDAO _frequenciaDAO = new FrequenciaDAO(ref _db);
                    List<Frequencia> lFrequencia = new List<Frequencia>();

                    k = 0;
                    n = 0;
                    tFaltas = 0;
                    tNota = 0;
                    tResultado = " ";
                    Parametro = 25;
                    Perc = 0;
                    //lm = lMatriculas.Count();

                    while (k < 32)
                    {
                        n = nAlunos + 1;
                        PdfPCell CelFrequenciaAlunos = new iTextSharp.text.pdf.PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, n.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();

                        int totalHoraAproveitamento = 0;

                        if (nAlunos < _turma.ListaMatriculas.Count())
                        {
                            if (lMatriculas[nAlunos].Situacao == "Evadido")
                            {
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[nAlunos].Aluno.Nome.ToUpper().ToString() + " *" + lMatriculas[nAlunos].DataSituacao.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            }
                            else
                            {
                                //Verificar Aproveitamentos no Fechamento
                                lAproveitamento = _aproveitamentoDAO.LoadByMatricula(lMatriculas[nAlunos]);

                                string ListaAproveitamentos = "";

                                if (lAproveitamento.Count() != 0)
                                {
                                    for (int m = 0; m < lAproveitamento.Count(); m++)
                                    {
                                        ListaAproveitamentos = ListaAproveitamentos + "(" + lAproveitamento[m].UnidadeCurricular.Sigla.ToString() + ")";
                                        totalHoraAproveitamento = totalHoraAproveitamento + lAproveitamento[m].UnidadeCurricular.CargaHoraria;
                                    }
                                }
                                if (ListaAproveitamentos != "")
                                {
                                    ListaAproveitamentos = " **" + ListaAproveitamentos.ToString();
                                }
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, lMatriculas[nAlunos].Aluno.Nome.ToUpper().ToString() + ListaAproveitamentos.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            }

                            lFrequencia = _frequenciaDAO.LoadByMatricula(lMatriculas[nAlunos]).ToList();

                            if (lMatriculas[nAlunos].Situacao == "Evadido")
                            {
                                tFaltas = lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "A").Sum(x => x.Diario.HoraAulaDia) + (lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TS").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TS").Sum(x => x.HoraAula)) + (lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TE").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Diario.Data < x.Matricula.DataSituacao).Where(x => x.Presenca == "TE").Sum(x => x.HoraAula));
                            }
                            else
                            {
                                tFaltas = lFrequencia.Where(x => x.Presenca == "A").Sum(x => x.Diario.HoraAulaDia) + (lFrequencia.Where(x => x.Presenca == "TS").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Presenca == "TS").Sum(x => x.HoraAula)) + (lFrequencia.Where(x => x.Presenca == "TE").Sum(x => x.Diario.HoraAulaDia) - lFrequencia.Where(x => x.Presenca == "TE").Sum(x => x.HoraAula));
                            }


                            //NOTAS

                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                            lFrequencia = null;
                            tFaltas = 10000;
                            tNota = 10000;
                            tResultado = "10000";
                        }

                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_LEFT;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);


                        /////////////////////Frequencia/////////////////////
                        j = 0;
                        b = 28;

                        if (lFrequencia != null)
                        {
                            while (j < b)
                            {
                                if (j <= lDiario.Count())
                                {

                                    CelFrequenciaAlunos = new PdfPCell();
                                    if (IdUnidadeCurricular != 0)
                                    {
                                        if (j < lFrequencia.Count())
                                        {

                                            if (lFrequencia[j].Matricula.Situacao == "Evadido")
                                            {
                                                if (lFrequencia[j].Matricula.DataSituacao <= lFrequencia[j].Diario.Data)
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));

                                                }
                                                else if (lFrequencia[j].Presenca == "TE")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "T", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));

                                                }
                                                else if (lFrequencia[j].Presenca == "AP")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                }
                                                else if (lFrequencia[j].Presenca == "TS")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "T", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                }
                                                else if (lFrequencia[j].Presenca == "A")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "|", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                }
                                                else if (lFrequencia[j].Presenca == "P")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "•", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                };

                                            }

                                            else
                                            {
                                                lAproveitamento = _aproveitamentoDAO.LoadByMatricula(lMatriculas[k]);

                                                if (lFrequencia[j].Presenca == "TE")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "T", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));

                                                }
                                                else if (lFrequencia[j].Presenca == "TS")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "T", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                }
                                                else if (lFrequencia[j].Presenca == "AP")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                }
                                                else if (lFrequencia[j].Presenca == "A")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "|", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                }
                                                else if (lFrequencia[j].Presenca == "P")
                                                {
                                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, "•", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                                };
                                            };

                                        }
                                        else
                                        {
                                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                        }
                                    }
                                    else
                                    {
                                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                    }
                                }
                                else
                                {
                                    CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                }
                                CelFrequenciaAlunos.PaddingBottom = 3;
                                CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                                CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tabelaFrequencia.AddCell(CelFrequenciaAlunos);
                                j++;
                            }
                        }
                        else
                        {
                            while (j < b)
                            {

                                CelFrequenciaAlunos = new PdfPCell();
                                CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.BLACK));
                                CelFrequenciaAlunos.PaddingBottom = 3;
                                CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                                CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tabelaFrequencia.AddCell(CelFrequenciaAlunos);
                                j++;
                            }
                        }

                        CelFrequenciaAlunos = new PdfPCell();
                        if (tFaltas == 10000)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tFaltas.ToString("0"), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, n.ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequencia = new PdfPCell();
                        if (tNota == 10000)
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tNota.ToString("0"), new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequencia.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequencia.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        CelFrequenciaAlunos = new PdfPCell();
                        if (tResultado == "10000")
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }
                        else
                        {
                            CelFrequenciaAlunos.Phrase = new Phrase(alturaLinha3, tResultado, new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                        }

                        CelFrequenciaAlunos.PaddingBottom = 3;
                        CelFrequenciaAlunos.HorizontalAlignment = Element.ALIGN_CENTER;
                        CelFrequenciaAlunos.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tabelaFrequencia.AddCell(CelFrequenciaAlunos);

                        k++;
                        nAlunos++;
                    }
                    #endregion

                    #region tabela de Frequencia - Rodapé

                    ////////////*Tabela Frequencia - RODAPÉ*////////////////////////////////////////////////////////// 

                    PdfPTable tabelaFrequenciaRodape = new PdfPTable(5);
                    tabelaFrequenciaRodape.TotalWidth = 815;
                    tabelaFrequenciaRodape.LockedWidth = true;
                    float[] widthfr = new float[] {
                            235, 100, 160, 160, 160  };
                    tabelaFrequenciaRodape.SetWidths(widthfr);

                    PdfPCell CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "Representante do Cliente", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.PaddingBottom = 5;
                    CelFrequenciaRadape.PaddingTop = 15;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, " ", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Responsável da \nEmpresa", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Docente\n ", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n\n__________________________________\nAssinatura do Responsável da \nEscola", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.HorizontalAlignment = Element.ALIGN_CENTER;
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Rowspan = 4;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "O curso foi realizado conforme apontado neste relatório.\n", new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "\n   Nome: __________________________________\n", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    CelFrequenciaRadape = new PdfPCell();
                    CelFrequenciaRadape.Phrase = new Phrase(alturaLinha3, "   RG:     __________________________________", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CelFrequenciaRadape.Border = Image.NO_BORDER;
                    CelFrequenciaRadape.PaddingBottom = 25;
                    tabelaFrequenciaRodape.AddCell(CelFrequenciaRadape);

                    ///////////////////Tabela Rodapé 1////////////////////////////

                    PdfPTable tabelaFrequenciaRodape1 = new PdfPTable(1);
                    tabelaFrequenciaRodape1.WidthPercentage = 100;
                    tabelaFrequenciaRodape1.HorizontalAlignment = Element.ALIGN_LEFT;

                    PdfPCell CelFrequenciaRadape1 = new PdfPCell();
                    CelFrequenciaRadape1.Phrase = new Phrase(alturaLinha3, "      *Aluno Evadido    **Aluno Dispensado    ***Trancamento    ****Aluno Transferido", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape1.Border = Image.TOP_BORDER;
                    CelFrequenciaRadape1.PaddingTop = 5;
                    //CelFrequenciaRadape1.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape1.AddCell(CelFrequenciaRadape1);

                    CelFrequenciaRadape1 = new PdfPCell();
                    CelFrequenciaRadape1.Phrase = new Phrase(alturaLinha3, "      Resutado Final:       C = Certificado,     N = Não Certificado,    E = Evadido,    T = Transferido", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK));
                    CelFrequenciaRadape1.Border = Image.NO_BORDER;
                    tabelaFrequenciaRodape1.AddCell(CelFrequenciaRadape1);


                    ////////////////////////////////////////////////////////////////

                    #endregion


                    //////////////////////////////////////////////////////////////////////////////////////////
                    ////////////Elementos do Documento PDF a ser Gerado///////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////

                    document.Add(tabelaGeral);
                    document.NewPage();
                    document.Add(tabelaFrequencia);
                    document.Add(tabelaFrequenciaRodape);
                    document.Add(tabelaFrequenciaRodape1);
                }


                document.Close();
                writer.Close();
                Response.Clear();

                string Descricao = _unidadecurricular.Descricao;

                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition", "attachment;filename=" + _turma.Descricao + "_" + Descricao + ".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.End();
                ms.Close();
            }


        }

        //
        //GET: /Diario/GerarXLS
        public ActionResult GerarPDF(int id)
        {
            VerificarSessao();
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            ViewBag.Turma = _turma;

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;
            
            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            List<Matricula> lMatriculas = matriculaDAO.LoadByTurma(_turma).OrderBy(x => x.Aluno.Nome).ToList();

            ViewBag.lMatriculas = lMatriculas.ToList();

            return View("~/Views/Sistema/Diario/GerarPDF.cshtml", _turma);

        }

        //
        //POST: /Diario/GerarXLS
        [HttpPost]
        public ActionResult GerarPDF(int id = 0, int IdUnidadeCurricular = 0)
        {            
            if (IdUnidadeCurricular > 0)            
                DownloadAsPDF_Unidade(id, IdUnidadeCurricular);                            
            else
                DownloadAsPDF_Fechamento(id);

            return RedirectToAction("GerarPDF", new { id = id });
        }

        //
        //GET: /Diario/GerarXLS
        public ActionResult RelUsuarios(int id)
        {
            VerificarSessao();

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            List<Matricula> lMatriculas = matriculaDAO.LoadByTurma(_turma).OrderBy(x => x.Aluno.Nome).ToList();

            ViewBag.lMatriculas = lMatriculas.ToList();

            List<Usuario> listaUsuarios = new List<Usuario>();

            listaUsuarios.Add(new Usuario { IdUsuario = 1, Perfil = "professor", Username = "carlos", Password = "000012", NomeCompleto = "Carlos Cavalheiro" });
            listaUsuarios.Add(new Usuario { IdUsuario = 1, Perfil = "professor", Username = "roberta", Password = "000013", NomeCompleto = "Roberta Rodrigues" });

            return View("~/Views/Sistema/Diario/Fechamento/RelUsuarios.aspx", listaUsuarios);

        }

        //
        // GET: /Diario/Conteudo
        public ActionResult Conteudo(int? pagina, int id = 0, int strCriterio = 0, int strCriterioAnterior = 0)
        {
            VerificarSessao();

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById(id);

            ViewBag.Turma = _turma;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            DiarioDAO diarioDAO = new DiarioDAO(ref _db);
            List<Diario> lDiario = new List<Diario>();

            if (strCriterio == 0)
            {
                lDiario = diarioDAO.LoadByConteudo(_turma).OrderByDescending(x => x.Data).ToList();
                strCriterioAnterior = strCriterio;
            }
            else if (strCriterio == strCriterioAnterior)
            {
                lDiario = diarioDAO.LoadByConteudo(_turma).OrderBy(x => x.Data).Where(x => x.UnidadeCurricular.IdUnidadeCurricular == strCriterio).ToList();
                strCriterioAnterior = strCriterio;
            }
            else
            {
                lDiario = diarioDAO.LoadByConteudo(_turma).OrderBy(x => x.Data).Where(x => x.UnidadeCurricular.IdUnidadeCurricular == strCriterio).ToList();
                pagina = 1;
                strCriterioAnterior = strCriterio;
            }

            ViewBag.strCriterio = strCriterio;
            ViewBag.strCriterioAnterior = strCriterioAnterior;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View("~/Views/Sistema/Diario/Conteudo.cshtml", lDiario.ToPagedList(numeroPagina, tamanhoPagina));
        }

        //
        // GET: /Diario/Frequencia
        public ActionResult Frequencia(int? pagina, int id = 0, int strCriterio = 0, int strCriterioAnterior = 0)
        {
            VerificarSessao();
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            //ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;
            //ViewBag.turma_idturma = _turma.Idturma;
            ViewBag.Turma = _turma;

            Diario _diario = new Diario();
            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);
            List<Diario> lDiario = new List<Diario>();

            _diario.Turma = _turma;
            _diario.IdTurma = _turma.Idturma;

            lDiario = _diarioDAO.LoadByDiarioTurma(ref _turma).OrderBy(x => x.Data).ToList();

            if(strCriterio != 0)
            lDiario = lDiario.Where(x => x.UnidadeCurricular.IdUnidadeCurricular == strCriterio).ToList();


            //if (strCriterio == 0)
            //{
            //    lDiario = _diarioDAO.LoadByDiarioTurma(ref _turma).OrderBy(x => x.Data).ToList();
            //}

            //if (strCriterio == strCriterioAnterior)
            //{
            //    lDiario = _diarioDAO.LoadByDiarioTurma(ref _turma).OrderBy(x => x.Data).Where(x => x.UnidadeCurricular.IdUnidadeCurricular == strCriterio).ToList();
            //}
            //else
            //{
            //    lDiario = _diarioDAO.LoadByDiarioTurma(ref _turma).OrderBy(x => x.Data).Where(x => x.UnidadeCurricular.IdUnidadeCurricular == strCriterio).ToList();          
            //}

            ViewBag.strCriterio = strCriterio;
            //ViewBag.strCriterioAnterior = strCriterioAnterior;

            List<Matricula> lMatriculas = _turma.ListaMatriculas.OrderBy(x => x.Aluno.Nome).ToList();
            

            ViewBag.ListaMatriculas = lMatriculas.ToList();
            ViewBag.ListaDiario = lDiario.ToList();

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");



            //return View("~/Views/Sistema/Diario/Frequencia.cshtml", lDiario.ToPagedList(numeroPagina, tamanhoPagina));
            return View("~/Views/Sistema/Diario/Frequencia.cshtml", lDiario.ToPagedList(numeroPagina, tamanhoPagina));


        }

        //
        //GET: /Diario/ConteudoNovo/5
        public ActionResult LancamentoNovo(int id = 0)
        {
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);

            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            Diario diario = null;

            try
            {
                //DBConnect _db = DBConnect.GetInstance();
                DiarioDAO _diarioDAO = new DiarioDAO(ref _db);
                TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

                Turma _t = new Turma();
                _t.Idturma = id;

                diario = _diarioDAO.diarioByTurma(ref _t, DateTime.Now, new UnidadeCurricular());

                //Diário não localizado, carregar lista de matriculas vazias
                if (diario == null)
                {

                    diario = new Diario();

                    FrequenciaDAO _frequenciaDAO = new FrequenciaDAO(ref _db);
                    diario.Frequencias = _frequenciaDAO.LoadByTurma(ref _t).OrderBy(x => x.Matricula.Aluno.Nome).ToList();
                    diario.Turma = _turmaDAO.LoadById2(_t.Idturma);

                }

            }
            catch (Exception _ex)
            {
                //Tratar erro aqui....
                this.Danger("Falha ao consultar registros." + _ex.Message);                

            }
            if (diario == null) { diario = new Diario(); }

            return View("~/Views/Sistema/Diario/LancamentoNovo.cshtml", diario);
        }

        //
        // POST: /Diario/ConteudoNovo/5            
        [HttpPost]
        public ActionResult LancamentoNovo(Diario diario, int id = 0)
        {
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();
            UnidadeCurricularDAO unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);

            _turma = turmaDAO.LoadById2(id);
            diario.Turma = _turma;
            diario.UnidadeCurricular = unidadeCurricularDAO.LoadById(diario.IdUnidadeCurricular);
            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;
            ViewBag.turma_idturma = _turma.Idturma;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);

            if (ModelState.IsValid)
            {
                _diarioDAO.Insert(ref diario);

                this.Success("Conteúdo e Frenquências cadastrados com Sucesso!");                

                return RedirectToAction("Conteudo", new { id = _turma.Idturma });
            }
            else
            {

                this.Danger("Lançamento não cadastrado. Verifique se os obrigatórios estão preenchidos!");                

                //return RedirectToAction("LancamentoNovo", new { id = _turma.Idturma });
                try
                {
                    //DBConnect _db = DBConnect.GetInstance();
                    TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

                    Turma _t = new Turma();
                    _t.Idturma = id;

                    diario = _diarioDAO.diarioByTurma(ref _t, DateTime.Now, new UnidadeCurricular());

                    //Diário não localizado, carregar lista de matriculas vazias
                    if (diario == null)
                    {

                        diario = new Diario();

                        FrequenciaDAO _frequenciaDAO = new FrequenciaDAO(ref _db);
                        diario.Frequencias = _frequenciaDAO.LoadByTurma(ref _t).OrderBy(x => x.Matricula.Aluno.Nome).ToList();
                        diario.Turma = _turmaDAO.LoadById2(_t.Idturma);

                    }

                }
                catch (Exception _ex)
                {
                    //Tratar erro aqui....
                    this.Danger("Falha ao consultar registros." + _ex.Message);

                }
                if (diario == null) { diario = new Diario(); }

                return View("~/Views/Sistema/Diario/LancamentoNovo.cshtml", diario);
            }
        }

        //
        //GET: 
        public ActionResult FrequenciaIndividual(int id = 0)
        {
            VerificarSessao();
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;
            ViewBag.turma_idturma = _turma.Idturma;

            //List<Matricula> lMatriculas = _turma.ListaMatriculas.OrderBy(x => x.Aluno.Nome).ToList();

            ViewBag.matricula_idmatriculas = new SelectList(_turma.ListaMatriculas.OrderBy(x => x.Aluno.Nome).ToList(), "IdMatricula", "Aluno.Nome");

            //ViewBag.matricula_idmatriculas = new SelectList(lMatriculas.ToList(), "IdMatricula", "Nome");

            Diario _diario = new Diario();
            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);

            //List<Diario> lDiario = _diarioDAO.LoadByDiarioTurma(ref _turma).OrderBy(x => x.Data).ToList();

            ViewBag.datadiario_iddiario = new SelectList(_diarioDAO.LoadByDiarioTurma(ref _turma).OrderBy(x => x.Data).ToList(), "IdDiario", "Data");


            return View("~/Views/Sistema/Diario/LancamentoFrequenciaIndividual.cshtml");
        }

        //
        // POST: /Diario/ConteudoNovo/5            
        [HttpPost]
        public ActionResult FrequenciaIndividual(Diario diario, int id = 0)
        {
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();
            UnidadeCurricularDAO unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);

            _turma = turmaDAO.LoadById(id);
            diario.Turma = _turma;
            diario.UnidadeCurricular = unidadeCurricularDAO.LoadById(diario.IdUnidadeCurricular);
            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;
            ViewBag.turma_idturma = _turma.Idturma;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);

            if (ModelState.IsValid)
            {
                _diarioDAO.Insert(ref diario);
                TempData["Erro"] = "0";
                TempData["Mensagem"] = "Conteúdo e Frenquências cadastrados com Sucesso!";

                return RedirectToAction("Conteudo", new { id = _turma.Idturma });
            }
            else
            {

                TempData["Erro"] = "1";
                TempData["Mensagem"] = "Lançamento não cadastrado. Verifique se os obrigatórios estão preenchidos!";

                //return RedirectToAction("LancamentoNovo", new { id = _turma.Idturma });
                try
                {
                    //DBConnect _db = DBConnect.GetInstance();
                    TurmaDAO _turmaDAO = new TurmaDAO(ref _db);

                    Turma _t = new Turma();
                    _t.Idturma = id;

                    diario = _diarioDAO.diarioByTurma(ref _t, DateTime.Now, new UnidadeCurricular());

                    //Diário não localizado, carregar lista de matriculas vazias
                    if (diario == null)
                    {

                        diario = new Diario();

                        FrequenciaDAO _frequenciaDAO = new FrequenciaDAO(ref _db);
                        diario.Frequencias = _frequenciaDAO.LoadByTurma(ref _t).OrderBy(x => x.Matricula.Aluno.Nome).ToList();
                        diario.Turma = _turmaDAO.LoadById(_t.Idturma);

                    }

                }
                catch (Exception _ex)
                {
                    //Tratar erro aqui....
                    ViewBag.Error = "Falha ao consultar registros." + _ex.Message;

                }
                if (diario == null) { diario = new Diario(); }

                return View("~/Views/Sistema/Diario/LancamentoFrequenciaIndividual.cshtml", diario);
            }
        }

        [HttpPost]
        public JsonResult LancamentoIndividual(long idMatricula, long idDiario, decimal horaAulaDia)
        {
            try
            {

                Frequencia prot = new Frequencia();
                FrequenciaDAO frequenciaDAO = new FrequenciaDAO(ref _db);
                DiarioDAO _diarioDAO = new DiarioDAO(ref _db);
                MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);

                prot.Diario = _diarioDAO.LoadById((int)idDiario);
                prot.HoraAula = horaAulaDia;
                prot.Matricula = matriculaDAO.LoadById(idMatricula);
                prot.Presenca = "P";

                frequenciaDAO.Insert(ref prot, idDiario);

                return Json(new
                {
                    msg = "Lançamento Realizado! "
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //
        // GET: /Diario/ConteudoEditar
        public ActionResult LancamentoEditar(int id = 0, int mostrarConteudo = 0, int idDiario = 0, int strCriterio = 0, int strCriterioAnterior = 0)
        {

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(id);

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            ViewBag.mostrarConteudo = mostrarConteudo;
            ViewBag.strCriterio = strCriterio;
            ViewBag.strCriterioAnterior = strCriterioAnterior;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);

            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            Diario _diario = new Diario();
            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);
            _diario = _diarioDAO.LoadById2(idDiario);


            return View("~/Views/Sistema/Diario/LancamentoEditar.cshtml", _diario);
        }

        //
        // POST: /Diario/ConteudoEditar
        [HttpPost]
        public ActionResult LancamentoEditar(Diario diario, int mostrarConteudo = 0, int strCriterio = 0, int strCriterioAnterior = 0)
        {
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById2(diario.IdTurma);

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            int strC = strCriterio;
            int strCA = strCriterioAnterior;

            if (ModelState.IsValid)
            {
                DiarioDAO _diarioDAO = new DiarioDAO(ref _db);
                _diarioDAO.Update(ref diario);
                TempData["Erro"] = "0";
                TempData["Mensagem"] = "Lançamento alterado com com Sucesso!";
                if (mostrarConteudo == 0)
                {
                    return RedirectToAction("Frequencia", new { id = _turma.Idturma, strCriterio = strC, strCriterioAnterior = strCA });
                }
                else
                {
                    return RedirectToAction("Conteudo", new { id = _turma.Idturma, strCriterio = strC, strCriterioAnterior = strCA });
                }
            }

            TempData["Erro"] = "1";
            TempData["Mensagem"] = "Lançamento não Alterado!";
            return View("~/Views/Sistema/Diario/LancamentoEditar.cshtml", new { id = _turma.Idturma, idDiario = diario.IdDiario });
        }

        //
        // GET: /Diario/ConteudoExcluir
        public ActionResult LancamentoExcluir(int id = 0, int idDiario = 0, int strCriterio = 0, int strCriterioAnterior = 0)
        {

            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            _turma = turmaDAO.LoadById(id);

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            ViewBag.strCriterio = strCriterio;
            ViewBag.strCriterioAnterior = strCriterioAnterior;

            UnidadeCurricularDAO _unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);

            ViewBag.unidadecurricular_idunidadecurricular = new SelectList(_unidadeCurricularDAO.LoadByCurso(_turma.Curso).ToList(), "IdUnidadeCurricular", "Descricao");

            Diario _diario = new Diario();
            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);
            _diario = _diarioDAO.LoadById2(idDiario);

            return View("~/Views/Sistema/Diario/LancamentoExcluir.cshtml", _diario);
        }

        //
        // POST: /Diario/LancamentoExcluir
        [HttpPost]
        public ActionResult LancamentoExcluir(Diario diario, int strCriterio = 0, int strCriterioAnterior = 0)
        {
            VerificarSessao();

            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            Turma _turma = new Turma();

            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);
            Diario _diario = new Diario();
            _diario = _diarioDAO.LoadById(Convert.ToInt32(diario.IdDiario));

            _turma = _turmaDAO.LoadById(diario.IdTurma);

            ViewBag.turma_descricao = _turma.Descricao + " | " + _turma.Curso.CursoNome;

            ViewBag.turma_idturma = _turma.Idturma;

            int strC = strCriterio;
            int strCA = strCriterioAnterior;


            if (_diario != null)
            {

                _diarioDAO.Delete(Convert.ToInt16(diario.IdDiario));
                TempData["Erro"] = "0";
                TempData["Mensagem"] = "Lançamento EXCLUÍDO com Sucesso!";
                return RedirectToAction("Conteudo", new { id = _turma.Idturma, strCriterio = strC, strCriterioAnterior = strCA });
            }
            else
            {
                TempData["Erro"] = "1";
                TempData["Mensagem"] = "Lançamentão NÃO Excluído!";
                return View("~/Views/Sistema/Diario/LancamentoExcluir.cshtml", new { id = _turma.Idturma, idDiario = diario.IdDiario });
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
