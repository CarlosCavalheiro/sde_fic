using SDE_FIC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using MySql;

namespace SDE_FIC.DAO
{
    public class TurmaDAO
    {
        private DBConnect _db;

        public TurmaDAO(ref DBConnect db)
        {
            this._db = db;

        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("  `idturma`,");
            sb.AppendLine("  `descricao`,");
            sb.AppendLine("  `horario`,");
            sb.AppendLine("  `datainicio`,");
            sb.AppendLine("  `datafim`,");
            sb.AppendLine("  `horasaula`,");
            sb.AppendLine("  `status`,");
            sb.AppendLine("  `segunda`,");
            sb.AppendLine("  `terca`,");
            sb.AppendLine("  `quarta`,");
            sb.AppendLine("  `quinta`,");
            sb.AppendLine("  `sexta`,");
            sb.AppendLine("  `sabado`,");
            sb.AppendLine("  `curso_idcurso`,");
            sb.AppendLine("  `funcionario_idfuncionario`,");
            sb.AppendLine("  `domingo`,");
            sb.AppendLine("  `cliente`,");
            sb.AppendLine("  `responsavel`,");
            sb.AppendLine("  `proposta_ano`,");
            sb.AppendLine("  `local_realizacao`,");
            sb.AppendLine("  `atendimento`");
            sb.AppendLine("FROM ");
            sb.AppendLine("  `turma`");

            return sb;

        }

        private Turma loadObject(ref System.Data.DataRow Rows)
        {
            Turma _t = new Turma();

            FuncionarioDAO funcionarioDAO = new FuncionarioDAO(ref _db);
            CursoDAO cursoDAO = new CursoDAO(ref _db);
            //MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);

            _t.Idturma = (int)Rows["idturma"];
            _t.Descricao = (string)Rows["Descricao"];
            _t.DataInicio = (Nullable<DateTime>)Rows["datainicio"];
            _t.DataFim = (Nullable<DateTime>)Rows["datafim"]; ;
            _t.Horario = (string)Rows["horario"];
            //_t.Matriculas = matriculaDAO.LoadByTurma(_t);
            _t.Curso = cursoDAO.LoadById((int)Rows["curso_idcurso"]);
            _t.Funcionario = funcionarioDAO.LoadById((int)Rows["funcionario_idfuncionario"]);
            _t.HorasAula = (decimal)Rows["horasaula"];
            _t.Domingo = Convert.ToBoolean(Rows["domingo"]);
            _t.Segunda = Convert.ToBoolean(Rows["segunda"]);
            _t.Terca = Convert.ToBoolean(Rows["terca"]);
            _t.Quarta = Convert.ToBoolean(Rows["quarta"]);
            _t.Quinta = Convert.ToBoolean(Rows["quinta"]);
            _t.Sexta = Convert.ToBoolean(Rows["sexta"]);
            _t.Sabado = Convert.ToBoolean(Rows["sabado"]);
            _t.Status = Convert.ToBoolean(Rows["status"]);
            _t.Cliente = (string)Rows["cliente"];
            _t.Responsavel = (string)Rows["responsavel"];
            _t.PropostaAno = (string)Rows["proposta_ano"];
            _t.LocalRealizacao = (string)Rows["local_realizacao"];
            _t.Atendimento = (string)Rows["atendimento"];


            return _t;
        }

        private Turma loadObject2(ref System.Data.DataRow Rows)
        {
            Turma _t = new Turma();

            FuncionarioDAO funcionarioDAO = new FuncionarioDAO(ref _db);
            CursoDAO cursoDAO = new CursoDAO(ref _db);
            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);
            AproveitamentosDAO aproveitamentoDAO = new AproveitamentosDAO(ref _db);

            _t.Idturma = (int)Rows["idturma"];
            _t.Descricao = (String)Rows["Descricao"];
            _t.DataInicio = (Nullable<DateTime>)Rows["datainicio"];
            _t.DataFim = (Nullable<DateTime>)Rows["datafim"]; ;
            _t.Horario = (string)Rows["horario"];
            _t.ListaMatriculas = matriculaDAO.LoadByTurma(_t);            
            _t.Curso = cursoDAO.LoadById2((int)Rows["curso_idcurso"]);
            _t.Funcionario = funcionarioDAO.LoadById((int)Rows["funcionario_idfuncionario"]);
            _t.HorasAula = (decimal)Rows["horasaula"];
            _t.Domingo = Convert.ToBoolean(Rows["domingo"]);
            _t.Segunda = Convert.ToBoolean(Rows["segunda"]);
            _t.Terca = Convert.ToBoolean(Rows["terca"]);
            _t.Quarta = Convert.ToBoolean(Rows["quarta"]);
            _t.Quinta = Convert.ToBoolean(Rows["quinta"]);
            _t.Sexta = Convert.ToBoolean(Rows["sexta"]);
            _t.Sabado = Convert.ToBoolean(Rows["sabado"]);
            _t.Status = Convert.ToBoolean(Rows["status"]);
            _t.Cliente = (string)Rows["cliente"];
            _t.Responsavel = (string)Rows["responsavel"];
            _t.PropostaAno = (string)Rows["proposta_ano"];
            _t.LocalRealizacao = (string)Rows["local_realizacao"];
            _t.Atendimento = (string)Rows["atendimento"];


            return _t;
        }

        private Turma loadObject3(ref System.Data.DataRow Rows)
        {
            Turma _t = new Turma();

            FuncionarioDAO funcionarioDAO = new FuncionarioDAO(ref _db);
            CursoDAO cursoDAO = new CursoDAO(ref _db);
            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);

            _t.Idturma = (int)Rows["idturma"];
            _t.Descricao = (string)Rows["Descricao"];
            _t.DataInicio = (Nullable<DateTime>)Rows["datainicio"];
            _t.DataFim = (Nullable<DateTime>)Rows["datafim"]; ;
            _t.Horario = (string)Rows["horario"];
            _t.ListaMatriculas = matriculaDAO.LoadByTurma2(_t);

            _t.Curso = cursoDAO.LoadById((int)Rows["curso_idcurso"]);
            _t.Funcionario = funcionarioDAO.LoadById((int)Rows["funcionario_idfuncionario"]);
            _t.HorasAula = (decimal)Rows["horasaula"];
            _t.Domingo = Convert.ToBoolean(Rows["domingo"]);
            _t.Segunda = Convert.ToBoolean(Rows["segunda"]);
            _t.Terca = Convert.ToBoolean(Rows["terca"]);
            _t.Quarta = Convert.ToBoolean(Rows["quarta"]);
            _t.Quinta = Convert.ToBoolean(Rows["quinta"]);
            _t.Sexta = Convert.ToBoolean(Rows["sexta"]);
            _t.Sabado = Convert.ToBoolean(Rows["sabado"]);
            _t.Status = Convert.ToBoolean(Rows["status"]);
            _t.Cliente = (string)Rows["cliente"];
            _t.Responsavel = (string)Rows["responsavel"];
            _t.PropostaAno = (string)Rows["proposta_ano"];
            _t.LocalRealizacao = (string)Rows["local_realizacao"];
            _t.Atendimento = (string)Rows["atendimento"];


            return _t;
        }

        public Turma LoadById(int Id)
        {
            Turma _turma = null;

            //Inicializa String SQL
            StringBuilder sbSQL = this.GetPrefix();

            sbSQL.AppendLine(" WHERE idturma = @idturma");

            MySql.Data.MySqlClient.MySqlCommand myCmd = new MySql.Data.MySqlClient.MySqlCommand();
            myCmd.Connection = _db.Connection;
            myCmd.CommandText = sbSQL.ToString();
            myCmd.Parameters.Add("@idturma", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = Id;

            DataTable dt = _db.ExecuteQuery(ref myCmd);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                _turma = this.loadObject(ref dr);
            }
            return _turma;
        }

        public Turma LoadById2(int Id)
        {
            Turma _turma = null;

            //Inicializa String SQL
            StringBuilder sbSQL = this.GetPrefix();

            sbSQL.AppendLine(" WHERE idturma = @idturma");

            MySql.Data.MySqlClient.MySqlCommand myCmd = new MySql.Data.MySqlClient.MySqlCommand();
            myCmd.Connection = _db.Connection;
            myCmd.CommandText = sbSQL.ToString();
            myCmd.Parameters.Add("@idturma", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = Id;

            DataTable dt = _db.ExecuteQuery(ref myCmd);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                _turma = this.loadObject2(ref dr);
            }
            return _turma;
        }

        public List<Turma> LoadByFuncionario(ref Funcionario funcionario, bool somenteAtivo = true)
        {
            List<Turma> lTurma = new List<Turma>();

            StringBuilder sb = GetPrefix();
            sb.AppendLine("WHERE funcionario_idfuncionario = @funcionario_idfuncionario");

            //if (somenteAtivo == true)
            //{
            //    sb.AppendLine("AND status = 1");

            //}

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.CommandText = sb.ToString();
            myCMD.Parameters.Add("@funcionario_idfuncionario",MySql.Data.MySqlClient.MySqlDbType.Int16,0).Value = funcionario.IdFuncionario;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                lTurma.Add(loadObject(ref dr));

            }
            
            return lTurma;

        }

        public List<Turma> All()
        {
            List<Turma> lTurma = new List<Turma>();

            StringBuilder sb = GetPrefix();
 
            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;

            myCMD.CommandText = sb.ToString();
         

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lTurma.Add(loadObject(ref dr));
            }
            
            return lTurma;            

        }

        /// <summary>
        /// Inserir novo registro
        /// </summary>
        /// <param name="_obj"></param>
        public void Insert(Turma _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `turma` ");
            sb.AppendLine("(");
            sb.AppendLine("  `descricao`,");
            sb.AppendLine("  `horario`,");
            sb.AppendLine("  `datainicio`,");
            sb.AppendLine("  `datafim`,");
            sb.AppendLine("  `horasaula`,");
            sb.AppendLine("  `segunda`,");
            sb.AppendLine("  `terca`,");
            sb.AppendLine("  `quarta`,");
            sb.AppendLine("  `quinta`,");
            sb.AppendLine("  `sexta`,");
            sb.AppendLine("  `sabado`,");
            sb.AppendLine("  `domingo`,");
            sb.AppendLine("  `status`,");
            sb.AppendLine("  `cliente`,");
            sb.AppendLine("  `responsavel`,");
            sb.AppendLine("  `proposta_ano`,");
            sb.AppendLine("  `local_realizacao`,");
            sb.AppendLine("  `atendimento`,");
            sb.AppendLine("  `curso_idcurso`,");
            sb.AppendLine("  `funcionario_idfuncionario`) ");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @descricao,");
            sb.AppendLine("  @horario,");
            sb.AppendLine("  @datainicio,");
            sb.AppendLine("  @datafim,");
            sb.AppendLine("  @horasaula,");
            sb.AppendLine("  @segunda,");
            sb.AppendLine("  @terca,");
            sb.AppendLine("  @quarta,");
            sb.AppendLine("  @quinta,");
            sb.AppendLine("  @sexta,");
            sb.AppendLine("  @sabado,");
            sb.AppendLine("  @domingo,");
            sb.AppendLine("  @status,");
            sb.AppendLine("  @cliente,");
            sb.AppendLine("  @responsavel,");
            sb.AppendLine("  @proposta_ano,");
            sb.AppendLine("  @local_realizacao,");
            sb.AppendLine("  @atendimento,");
            sb.AppendLine("  @curso_idcurso,");
            sb.AppendLine("  @funcionario_idfuncionario);");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@descricao", MySqlDbType.VarChar, 255).Value = _obj.Descricao.ToUpper();
                myCMD.Parameters.Add("@horario", MySqlDbType.VarChar, 100).Value = _obj.Horario;
                myCMD.Parameters.Add("@datainicio", MySqlDbType.Date).Value = _obj.DataInicio;
                myCMD.Parameters.Add("@datafim", MySqlDbType.Date).Value = _obj.DataFim;
                myCMD.Parameters.Add("@curso_idcurso", MySqlDbType.Int16, 0).Value = _obj.Curso.IdCurso;
                myCMD.Parameters.Add("@funcionario_idfuncionario", MySqlDbType.Int16, 0).Value = _obj.Funcionario.IdFuncionario;
                myCMD.Parameters.Add("@horasaula", MySqlDbType.Decimal, 0).Value = _obj.HorasAula;
                myCMD.Parameters.Add("@domingo", MySqlDbType.Bit, 0).Value = _obj.Domingo;
                myCMD.Parameters.Add("@segunda", MySqlDbType.Bit, 0).Value = _obj.Segunda;
                myCMD.Parameters.Add("@terca", MySqlDbType.Bit, 0).Value = _obj.Terca;
                myCMD.Parameters.Add("@quarta", MySqlDbType.Bit, 0).Value = _obj.Quarta;
                myCMD.Parameters.Add("@quinta", MySqlDbType.Bit, 0).Value = _obj.Quinta;
                myCMD.Parameters.Add("@sexta", MySqlDbType.Bit, 0).Value = _obj.Sexta;
                myCMD.Parameters.Add("@sabado", MySqlDbType.Bit, 0).Value = _obj.Sabado;
                myCMD.Parameters.Add("@status", MySqlDbType.Bit, 0).Value = _obj.Status;
                myCMD.Parameters.Add("@cliente", MySqlDbType.VarChar, 45).Value = _obj.Cliente;
                myCMD.Parameters.Add("@responsavel", MySqlDbType.VarChar, 45).Value = _obj.Responsavel;
                myCMD.Parameters.Add("@local_realizacao", MySqlDbType.VarChar, 45).Value = _obj.LocalRealizacao;
                if (_obj.PropostaAno != null)
                {
                    myCMD.Parameters.Add("@proposta_ano", MySqlDbType.VarChar, 45).Value = _obj.PropostaAno;
                }
                else {
                    myCMD.Parameters.Add("@proposta_ano", MySqlDbType.VarChar, 45).Value = " ";
                }
                myCMD.Parameters.Add("@atendimento", MySqlDbType.VarChar, 45).Value = _obj.Atendimento;

                myCMD.ExecuteNonQuery();

                //obter o id gerado
                //_obj.Idturma = myCMD.LastInsertedId;

            }
            catch (Exception _ex)
            {
                throw _ex;

            }


        }

        /// <summary>
        /// Atualizar registro
        /// </summary>
        /// <param name="_obj"></param>
        /// <returns></returns>
        public int Update(Turma _obj)
        {
            int iAffectedRows = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE ");
            sb.AppendLine("  `turma` ");
            sb.AppendLine("SET ");
            sb.AppendLine("  `descricao` = @descricao,");
            sb.AppendLine("  `datainicio` = @datainicio,");
            sb.AppendLine("  `datafim` = @datafim,");
            sb.AppendLine("  `horario` = @horario,");
            sb.AppendLine("  `curso_idcurso` = @curso_idcurso,");
            sb.AppendLine("  `funcionario_idfuncionario` = @funcionario_idfuncionario,");
            sb.AppendLine("  `horasaula` = @horasaula,");
            sb.AppendLine("  `domingo` = @domingo,");
            sb.AppendLine("  `segunda` = @segunda,");
            sb.AppendLine("  `terca` = @terca,");
            sb.AppendLine("  `quarta` = @quarta,");
            sb.AppendLine("  `quinta` = @quinta,");
            sb.AppendLine("  `sexta` = @sexta,");
            sb.AppendLine("  `sabado` = @sabado,");
            sb.AppendLine("  `cliente` = @cliente,");
            sb.AppendLine("  `responsavel` = @responsavel,");
            sb.AppendLine("  `proposta_ano` = @proposta_ano,");
            sb.AppendLine("  `local_realizacao` = @local_realizacao,");
            sb.AppendLine("  `atendimento` = @atendimento,");
            sb.AppendLine("  `status` = @status");
            sb.AppendLine("WHERE ");
            sb.AppendLine("  `idturma` = @idturma;");


            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@descricao", MySqlDbType.VarChar, 255).Value = _obj.Descricao.ToUpper();
                myCMD.Parameters.Add("@horario", MySqlDbType.VarChar, 100).Value = _obj.Horario;
                myCMD.Parameters.Add("@datainicio", MySqlDbType.Date).Value = _obj.DataInicio;
                myCMD.Parameters.Add("@datafim", MySqlDbType.Date).Value = _obj.DataFim;
                myCMD.Parameters.Add("@curso_idcurso", MySqlDbType.Int16, 0).Value = _obj.Curso.IdCurso;
                myCMD.Parameters.Add("@funcionario_idfuncionario", MySqlDbType.Int16, 0).Value = _obj.Funcionario.IdFuncionario;
                myCMD.Parameters.Add("@horasaula", MySqlDbType.Decimal, 0).Value = _obj.HorasAula;
                myCMD.Parameters.Add("@domingo", MySqlDbType.Bit, 0).Value = _obj.Domingo;
                myCMD.Parameters.Add("@segunda", MySqlDbType.Bit, 0).Value = _obj.Segunda;
                myCMD.Parameters.Add("@terca", MySqlDbType.Bit, 0).Value = _obj.Terca;
                myCMD.Parameters.Add("@quarta", MySqlDbType.Bit, 0).Value = _obj.Quarta;
                myCMD.Parameters.Add("@quinta", MySqlDbType.Bit, 0).Value = _obj.Quinta;
                myCMD.Parameters.Add("@sexta", MySqlDbType.Bit, 0).Value = _obj.Sexta;
                myCMD.Parameters.Add("@sabado", MySqlDbType.Bit, 0).Value = _obj.Sabado;
                myCMD.Parameters.Add("@status", MySqlDbType.Bit, 0).Value = _obj.Status;
                myCMD.Parameters.Add("@idturma", MySqlDbType.Int16, 0).Value = _obj.Idturma;
                myCMD.Parameters.Add("@cliente", MySqlDbType.VarChar, 45).Value = _obj.Cliente;
                myCMD.Parameters.Add("@responsavel", MySqlDbType.VarChar, 45).Value = _obj.Responsavel;
                if (_obj.PropostaAno != null)
                {
                    myCMD.Parameters.Add("@proposta_ano", MySqlDbType.VarChar, 45).Value = _obj.PropostaAno;
                }
                else
                {
                    myCMD.Parameters.Add("@proposta_ano", MySqlDbType.VarChar, 45).Value = " ";
                }
                myCMD.Parameters.Add("@local_realizacao", MySqlDbType.VarChar, 45).Value = _obj.LocalRealizacao;
                myCMD.Parameters.Add("@atendimento", MySqlDbType.VarChar, 45).Value = _obj.Atendimento;

                iAffectedRows = myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

            return iAffectedRows;

        }

        /// <summary>
        /// Apagar registro
        /// </summary>
        /// <param name="unidadecurricular"></param>
        public void Delete(Turma _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM                                  ");
            sb.AppendLine("  `turma`                        ");
            sb.AppendLine("WHERE                                        ");
            sb.AppendLine("  idturma = @idturma ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@idturma", MySqlDbType.Int16).Value = _obj.Idturma;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }
        
    }
}