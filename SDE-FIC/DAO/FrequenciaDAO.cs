using MySql;
using MySql.Data.MySqlClient;
using SDE_FIC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace SDE_FIC.DAO
{
    public class FrequenciaDAO
    {
        private DBConnect _db;

        public FrequenciaDAO(ref DBConnect db)
        {
            this._db = db;

        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("  `idfrequencia`,");
            //sb.AppendLine("  `data`,");
            sb.AppendLine("  `horaaula`,");
            sb.AppendLine("  `presenca`,");
            sb.AppendLine("  `diario_iddiario`,");
            sb.AppendLine("  `matricula_idmatricula`");
            sb.AppendLine("FROM ");
            sb.AppendLine("  `frequencia`");

            return sb;
        }

        private Frequencia LoadObject(ref DataRow Rows)
        {
            Frequencia _frequencia = new Frequencia();
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);
            DiarioDAO _diarioDAO = new DiarioDAO(ref _db);

            _frequencia.IdFrequencia = (long)Rows["idfrequencia"];
            _frequencia.HoraAula = (decimal)Rows["horaaula"];
            _frequencia.Presenca = (string)Rows["presenca"];
            _frequencia.Diario = _diarioDAO.LoadById(Convert.ToInt16(Rows["diario_iddiario"]));
            _frequencia.Matricula = _matriculaDAO.LoadById(Convert.ToInt16(Rows["matricula_idmatricula"]));

            return _frequencia;

        }

        public List<Frequencia> LoadByMatricula(Matricula _matricula)
        {
            List<Frequencia> lFrequencia = new List<Frequencia>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE matricula_idmatricula = @matricula_idmatricula; ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = _matricula.IdMatricula;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = dt.Rows[i];

                lFrequencia.Add(LoadObject(ref dr));

            }

            return lFrequencia;

        }

        public Frequencia LoadById(int IdFrequencia)
        {
            Frequencia _frequencia = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idfrequencia = @idfrequencia ");

            MySqlCommand mySQL = new MySqlCommand();
            mySQL.CommandText = sbSQL.ToString();
            mySQL.Connection = _db.Connection;
            mySQL.Parameters.Add("@idfrequencia", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = IdFrequencia;

            DataTable dt = _db.ExecuteQuery(ref mySQL);

            if (dt.Rows.Count == 1)
            {

                DataRow dr = dt.Rows[0];

                _frequencia = LoadObject(ref dr);

            }

            return _frequencia;

        }

        public List<Frequencia> LoadByFrequencia(ref Frequencia frequencia)
        {
            List<Frequencia> lFrequencia = new List<Frequencia>();

            StringBuilder sbSQL = GetPrefix();

            return lFrequencia;

        }

        public List<Frequencia> LoadByTurma(ref Turma _turma)
        {
            List<Frequencia> lFrequencia = new List<Frequencia>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("  `idmatricula`,");
            sb.AppendLine("  `datamatricula`,");
            sb.AppendLine("  `situacao`,");
            sb.AppendLine("  `datasituacao`,");
            sb.AppendLine("  `aluno_idaluno`,");
            sb.AppendLine("  `turma_idturma`");
            sb.AppendLine("FROM ");
            sb.AppendLine("  `matricula`");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	`turma_idturma` = @turma_idturma    ");

            MySqlCommand myCmd = new MySqlCommand();
            myCmd.Connection = _db.Connection;
            myCmd.CommandText = sb.ToString();
            myCmd.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = _turma.Idturma;

            DataTable _dt = _db.ExecuteQuery(ref myCmd);

            MatriculaDAO matriculaDAO = new MatriculaDAO(ref _db);

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                DataRow dr = _dt.Rows[i];

                Frequencia _f = new Frequencia();
                _f.Matricula = matriculaDAO.LoadById(Convert.ToInt16(dr["idmatricula"]));
                lFrequencia.Add(_f);
                
            }

            return lFrequencia;

        }

        public List<Frequencia> LoadByFrequenciaTurma(ref Turma _turma)
        {
            List<Frequencia> lFrequencia = new List<Frequencia>();

            StringBuilder sb = GetPrefix();

            sb.AppendLine("INNER JOIN matricula On matricula.idmatricula = frequencia.matricula_idmatricula ");
            sb.AppendLine("INNER JOIN diario On diario.iddiario = frequencia.diario_iddiario ");
            sb.AppendLine("INNER JOIN turma On turma.idturma = matricula.turma_idturma");
            sb.AppendLine("WHERE turma.idturma = @turma_idturma; ");

            MySqlCommand myCmd = new MySqlCommand();
            myCmd.Connection = _db.Connection;
            myCmd.CommandText = sb.ToString();
            myCmd.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = _turma.Idturma;

            DataTable _dt = _db.ExecuteQuery(ref myCmd);

       
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                DataRow dr = _dt.Rows[i];
                lFrequencia.Add(LoadObject(ref dr));
            }

            return lFrequencia;

        }

        public List<Frequencia> All()
        {
            List<Frequencia> lFrequencia = new List<Frequencia>();

            StringBuilder sb = GetPrefix();

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;

            myCMD.CommandText = sb.ToString();


            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lFrequencia.Add(LoadObject(ref dr));
            }

            return lFrequencia;

        }
        
        /// <summary>
        /// Consulta lista de frequencia atraves de consulta de diario. Caso nao existe diario, retorna lista de matriculas sem frequencia
        /// </summary>
        /// <param name="_diario"></param>
        /// <param name="_data"></param>
        /// <param name="_unidadeCurricular"></param>
        /// <returns></returns>
        public List<Frequencia> LoadByDiario(Diario _diario)
        {
            List<Frequencia> lFrequencia = new List<Frequencia>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE diario_iddiario = @diario_iddiario ");

            MySqlCommand mySQL = new MySqlCommand();
            mySQL.CommandText = sbSQL.ToString();
            mySQL.Connection = _db.Connection;
            mySQL.Parameters.Add("@diario_iddiario", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = _diario.IdDiario;

            DataTable dt = _db.ExecuteQuery(ref mySQL);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = dt.Rows[i];
                lFrequencia.Add(LoadObject(ref dr));

            }

            return lFrequencia;

        }

        public List<Frequencia> LoadByDiarioId(int id)
        {
            List<Frequencia> lFrequencia = new List<Frequencia>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE diario_iddiario = @diario_iddiario ");

            MySqlCommand mySQL = new MySqlCommand();
            mySQL.CommandText = sbSQL.ToString();
            mySQL.Connection = _db.Connection;
            mySQL.Parameters.Add("@diario_iddiario", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = id;

            DataTable dt = _db.ExecuteQuery(ref mySQL);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = dt.Rows[i];
                lFrequencia.Add(LoadObject(ref dr));

            }

            return lFrequencia;

        }

        public List<Frequencia> LoadByDiarioUpdate(Diario _diario) 
        {
            List<Frequencia> _lFrequencia = new List<Frequencia>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE diario_iddiario = @diario_iddiario ");

            MySqlCommand mySQL = new MySqlCommand();
            mySQL.CommandText = sbSQL.ToString();
            mySQL.Connection = _db.Connection;
            mySQL.Parameters.Add("@diario_iddiario", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = _diario.IdDiario;

            DataTable dt = _db.ExecuteQuery(ref mySQL);

            //if (dt.Rows.Count == 1)
            //{
            //    DataRow dr = dt.Rows[0];
            //    _frequencia = LoadObject(ref dr);
            //}

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                _lFrequencia.Add(LoadObject(ref dr));

            }

            return _lFrequencia;
        }

        public void Insert(ref Frequencia _frequencia, long idDiario)
        {
   
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `frequencia`");
            sb.AppendLine("(");
            //sb.AppendLine("  `data`,");
            sb.AppendLine("  `horaaula`,");
            sb.AppendLine("  `presenca`,");
            sb.AppendLine("  `matricula_idmatricula`,");
            sb.AppendLine("  `diario_iddiario`) ");
            sb.AppendLine("VALUE (");
            //sb.AppendLine("  @data,");
            sb.AppendLine("  @horaaula,");
            sb.AppendLine("  @presenca,");
            sb.AppendLine("  @matricula_idmatricula,");
            sb.AppendLine("  @diario_iddiario);");
            

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
 
                //myCMD.Parameters.Add("@data", MySqlDbType.Date, 0).Value = _frequencia.Data;
                if (_frequencia.Presenca.ToUpper() == "A")
                {
                    myCMD.Parameters.Add("@horaaula", MySqlDbType.Decimal, 0).Value = 0;

                }
                else
                {
                    myCMD.Parameters.Add("@horaaula", MySqlDbType.Decimal, 0).Value = _frequencia.HoraAula;
                }                

                myCMD.Parameters.Add("@presenca", MySqlDbType.VarChar, 2).Value = _frequencia.Presenca.ToUpper();
                myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = _frequencia.Matricula.IdMatricula;
                myCMD.Parameters.Add("@diario_iddiario", MySqlDbType.Int16, 0).Value = idDiario;

                myCMD.ExecuteNonQuery();

                //obter o id gerado
                _frequencia.IdFrequencia = myCMD.LastInsertedId;

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }

        public void Update(ref Frequencia _frequencia, long idDiario)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE ");
            sb.AppendLine("  `frequencia` ");
            sb.AppendLine("SET ");
            sb.AppendLine("  `horaaula` = @horaaula, ");
            sb.AppendLine("  `presenca` = @presenca, ");
            sb.AppendLine("  `matricula_idmatricula` = @matricula_idmatricula, ");
            sb.AppendLine("  `diario_iddiario` = @diario_iddiario ");
            sb.AppendLine("WHERE");
            sb.AppendLine("  `idfrequencia` = @idfrequencia");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.Transaction = _db.Transaction;
            myCMD.CommandText = sb.ToString();
            myCMD.Parameters.Add("@idfrequencia", MySqlDbType.Int16, 0).Value = _frequencia.IdFrequencia;
            if (_frequencia.Presenca.ToUpper() == "A")
            {
                myCMD.Parameters.Add("@horaaula", MySqlDbType.Decimal, 0).Value = 0;

            }
            else
            {
                myCMD.Parameters.Add("@horaaula", MySqlDbType.Decimal, 0).Value = _frequencia.HoraAula;
            } 
            myCMD.Parameters.Add("@presenca", MySqlDbType.VarChar, 2).Value = _frequencia.Presenca.ToUpper();
            myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = _frequencia.Matricula.IdMatricula;
            myCMD.Parameters.Add("@diario_iddiario", MySqlDbType.Int16, 0).Value = idDiario;

            myCMD.ExecuteNonQuery();

            //_frequencia.IdFrequencia = myCMD.LastInsertedId;

        }

        //public void UpdateFrequencias(ref Frequencia _frequencia)
        //{

        //    FrequenciaDAO frequenciaDAO = new FrequenciaDAO(ref _db);

        //    for (int i = 0; i < _frequencia.Diario.Frequencias.Count; i++)
        //    {
        //        Frequencia f = _frequencia.Diario.Frequencias[i];
        //        frequenciaDAO.Update(ref f, f.Diario.IdDiario);
        //    }

        //}

        public int DeleteFromDiario(Diario _d)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" DELETE FROM frequencia ");
            sb.AppendLine(" WHERE diario_iddiario = @diario_iddiario ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sb.ToString();
            myCMD.Transaction = _db.Transaction;
            myCMD.Parameters.Add("@diario_iddiario", MySqlDbType.Int16, 0).Value = _d.IdDiario;

            int iAffected = myCMD.ExecuteNonQuery();

            return iAffected;

        }
    }
}