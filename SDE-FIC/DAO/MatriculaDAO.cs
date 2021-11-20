using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SDE_FIC.Models;
using MySql.Data.MySqlClient;
using System.Data;
using MySql;

namespace SDE_FIC.DAO
{
    public class MatriculaDAO
    {
        private DBConnect _db;

        public MatriculaDAO(ref DBConnect db)
        {
            this._db = db;
        }

        public StringBuilder GetPrefix()
        {
            StringBuilder _sb = new StringBuilder();

            _sb.AppendLine("SELECT ");
            _sb.AppendLine("  `idmatricula`,");
            _sb.AppendLine("  `datamatricula`,");
            _sb.AppendLine("  `situacao`,");
            _sb.AppendLine("  `datasituacao`,");
            _sb.AppendLine("  `aluno_idaluno`,");
            _sb.AppendLine("  `turma_idturma`");
            _sb.AppendLine("FROM ");
            _sb.AppendLine("  `matricula`");

            return _sb;

        }

        private Matricula LoadObject(ref DataRow Rows)
        {
            Matricula _matricula = new SDE_FIC.Models.Matricula();
            AlunoDAO alunoDAO = new AlunoDAO(ref _db);            
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            AproveitamentosDAO aproveitamentoDAO = new AproveitamentosDAO(ref _db);

            _matricula.IdMatricula = Convert.ToInt64(Rows["idmatricula"]);
            _matricula.DataMatricula = (DateTime)Rows["datamatricula"];
            _matricula.Situacao = (string)Rows["situacao"];
            _matricula.DataSituacao = (DateTime)Rows["datasituacao"];
            _matricula.IdAluno = (int)Rows["aluno_idaluno"];
            _matricula.IdTurma = (int)Rows["turma_idturma"];

            _matricula.Aluno = alunoDAO.LoadById(_matricula.IdAluno);
            _matricula.Turma = turmaDAO.LoadById(_matricula.IdTurma);


            return _matricula;
        }

        private Matricula LoadObject2(ref DataRow Rows)
        {
            Matricula _matricula = new SDE_FIC.Models.Matricula();
            AlunoDAO alunoDAO = new AlunoDAO(ref _db);
            TurmaDAO turmaDAO = new TurmaDAO(ref _db);
            NotasDAO notasDAO = new NotasDAO(ref _db);
            AproveitamentosDAO aproveitamentoDAO = new AproveitamentosDAO(ref _db);

            _matricula.IdMatricula = Convert.ToInt64(Rows["idmatricula"]);
            _matricula.DataMatricula = (DateTime)Rows["datamatricula"];
            _matricula.Situacao = (string)Rows["situacao"];
            _matricula.DataSituacao = (DateTime)Rows["datasituacao"];
            _matricula.IdAluno = (int)Rows["aluno_idaluno"];
            _matricula.IdTurma = (int)Rows["turma_idturma"];

            _matricula.Aluno = alunoDAO.LoadById(_matricula.IdAluno);
            _matricula.Turma = turmaDAO.LoadById(_matricula.IdTurma);

            _matricula.listaNotas = notasDAO.LoadByMatriculaId(Convert.ToInt16(Rows["idmatricula"]));

            _matricula.listaAproveitamentos = aproveitamentoDAO.LoadByMatriculaId(Convert.ToInt16(Rows["idmatricula"]));

            return _matricula;
        }

        public Matricula LoadById(long Id)
        {
            Matricula _matricula = new Matricula();

            StringBuilder sbSQL = GetPrefix();

            sbSQL.AppendLine("WHERE                            ");
            sbSQL.AppendLine("     idmatricula = @idmatricula  ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@idmatricula", MySqlDbType.Int16, 0).Value = Id;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _matricula = LoadObject(ref dr);

            }

            return _matricula;
        }

        public List<Matricula> LoadByTurma(Turma turma)
        {
            List<Matricula> lMatricula = new List<Matricula>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE turma_idturma = @turma_idturma; ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            //myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = turma.Idturma;
            myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = turma.Idturma;
            
            //_matricula.listaAproveitamentos = aproveitamentoDAO.LoadByMatriculaId(Convert.ToInt16(Rows["idmatricula"]));

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = dt.Rows[i];

                lMatricula.Add(LoadObject2(ref dr));

            }

            return lMatricula;

        }

        public List<Matricula> LoadByTurma2(Turma turma)//Turma turma)
        {
            List<Matricula> lMatricula = new List<Matricula>();
            

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE turma_idturma = @turma_idturma; ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            //myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = turma.Idturma;
            myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = turma.Idturma;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = dt.Rows[i];

                lMatricula.Add(LoadObject2(ref dr));

            }

            return lMatricula;

        }

        public List<Matricula> All()
        {
            List<Matricula> lMatricula = new List<Matricula>();
            List<Turma> lTurma = new List<Turma>();

            StringBuilder sb = GetPrefix();
            sb.AppendLine(" ORDER BY turma_idturma;");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;

            myCMD.CommandText = sb.ToString();


            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lMatricula.Add(LoadObject(ref dr));
            }

            return lMatricula;

        }

        /// <summary>
        /// Inserir novo registro
        /// </summary>
        /// <param name="_obj"></param>
        public void Insert(Matricula _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `matricula` ");
            sb.AppendLine("(");
            sb.AppendLine("  `datamatricula`,");
            sb.AppendLine("  `situacao`,");
            sb.AppendLine("  `datasituacao`,");
            sb.AppendLine("  `aluno_idaluno`,");
            sb.AppendLine("  `turma_idturma`) ");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @datamatricula,");
            sb.AppendLine("  @situacao,");
            sb.AppendLine("  @datasituacao,");
            sb.AppendLine("  @aluno_idaluno,");
            sb.AppendLine("  @turma_idturma);");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@datamatricula", MySqlDbType.Date).Value = _obj.DataMatricula;
                myCMD.Parameters.Add("@situacao", MySqlDbType.VarChar, 45).Value = _obj.Situacao;
                myCMD.Parameters.Add("@datasituacao", MySqlDbType.Date).Value = _obj.DataSituacao;
                myCMD.Parameters.Add("@aluno_idaluno", MySqlDbType.Int16, 0).Value = _obj.Aluno.IdAluno;
                myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = _obj.Turma.Idturma;

                myCMD.ExecuteNonQuery();

                //obter o id gerado
                _obj.IdMatricula = myCMD.LastInsertedId;


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
        public int Update(Matricula _obj)
        {
            int iAffectedRows = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE ");
            sb.AppendLine("  `matricula`  ");
            sb.AppendLine("SET ");
            sb.AppendLine("  `datamatricula` = @datamatricula,");
            sb.AppendLine("  `situacao` = @situacao,");
            sb.AppendLine("  `datasituacao` = @datasituacao,");
            sb.AppendLine("  `aluno_idaluno` = @aluno_idaluno,");
            sb.AppendLine("  `turma_idturma` = @turma_idturma ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("  `idmatricula` = @idmatricula;");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@datamatricula", MySqlDbType.Date).Value = _obj.DataMatricula;
                myCMD.Parameters.Add("@situacao", MySqlDbType.VarChar, 45).Value = _obj.Situacao;
                myCMD.Parameters.Add("@datasituacao", MySqlDbType.Date).Value = _obj.DataSituacao;
                myCMD.Parameters.Add("@aluno_idaluno", MySqlDbType.Int16, 0).Value = _obj.IdAluno;
                myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = _obj.IdTurma;
                myCMD.Parameters.Add("@idmatricula", MySqlDbType.Int16, 0).Value = _obj.IdMatricula;

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
        public void Delete(Matricula _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM                  ");
            sb.AppendLine("  `matricula`                ");
            sb.AppendLine("WHERE                        ");
            sb.AppendLine("  idmatricula = @idmatricula ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@idmatricula", MySqlDbType.Int16).Value = _obj.IdMatricula;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }

    }
}