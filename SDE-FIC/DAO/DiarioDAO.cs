using MySql.Data.MySqlClient;
using SDE_FIC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using MySql;

namespace SDE_FIC.DAO
{
    public class DiarioDAO
    {
        private DBConnect _db;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="db">Banco de dados</param>
        public DiarioDAO(ref DBConnect db)
        {
            this._db = db;
        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT                                       ");
            sb.AppendLine(" `iddiario`,                                 ");
            sb.AppendLine(" `conteudo`,                                 ");
            sb.AppendLine(" `ocorrencia`,                               ");
            sb.AppendLine(" `data`,                                     ");
            sb.AppendLine(" `hora_aula_dia`,                            ");
            sb.AppendLine(" `turma_idturma`,                            ");
            sb.AppendLine(" `unidade_curricular_idunidadecurricular`    ");
            sb.AppendLine("FROM                                         ");
            sb.AppendLine("  `diario`                                   ");

            return sb;

        }

        private Diario loadObject(ref System.Data.DataRow Rows)
        {
            Diario _d = new Diario();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);
            FrequenciaDAO _frequenciaDAO = new FrequenciaDAO(ref _db);

            _d.IdDiario = (int)Rows["iddiario"];
            _d.Conteudo = (String)Rows["conteudo"];
            _d.Ocorrencia = (String)Rows["ocorrencia"];
            _d.Data = (DateTime)Rows["data"];
            _d.HoraAulaDia = (decimal)Rows["hora_aula_dia"];
            _d.IdTurma = (int)Rows["turma_idturma"];
            _d.IdUnidadeCurricular = (int)Rows["unidade_curricular_idunidadecurricular"];
            _d.Turma = _turmaDAO.LoadById((int)Rows["turma_idturma"]);
            _d.UnidadeCurricular = _unidadecurricularDAO.LoadById((int)Rows["unidade_curricular_idunidadecurricular"]);
            //_d.Frequencias = _frequenciaDAO.LoadByDiarioId((int)Rows["iddiario"]);
            //_d.Frequencias = _frequenciaDAO.LoadByDiarioId((int)Rows["iddiario"]);
            return _d;
        }

        private Diario loadObject2(ref System.Data.DataRow Rows)
        {
            Diario _d = new Diario();
            TurmaDAO _turmaDAO = new TurmaDAO(ref _db);
            UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);
            FrequenciaDAO _frequenciaDAO = new FrequenciaDAO(ref _db);

            _d.IdDiario = (int)Rows["iddiario"];
            _d.Conteudo = (String)Rows["conteudo"];
            _d.Ocorrencia = (String)Rows["ocorrencia"];
            _d.Data = (DateTime)Rows["data"];
            _d.HoraAulaDia = (decimal)Rows["hora_aula_dia"];
            _d.IdTurma = (int)Rows["turma_idturma"];
            _d.IdUnidadeCurricular = (int)Rows["unidade_curricular_idunidadecurricular"];

            _d.Turma = _turmaDAO.LoadById2((int)Rows["turma_idturma"]);
            _d.UnidadeCurricular = _unidadecurricularDAO.LoadById((int)Rows["unidade_curricular_idunidadecurricular"]);
            _d.Frequencias = _frequenciaDAO.LoadByDiarioId((int)Rows["iddiario"]);
            
            return _d;
        }

        public List<Diario> LoadByTurma(Turma turma)
        {
            List<Diario> lDiario = new List<Diario>();

            StringBuilder sb = GetPrefix();
            sb.AppendLine("WHERE turma_idturma = @turma_idturma");

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.CommandText = sb.ToString();
            myCMD.Parameters.Add("@turma_idturma", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = turma.Idturma;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                lDiario.Add(loadObject(ref dr));

            }

            return lDiario ;

        }

        public List<Diario> LoadByConteudo(Turma turma)
        {
            List<Diario> lDiario = new List<Diario>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE turma_idturma = @turma_idturma ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = turma.Idturma;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                lDiario.Add(loadObject(ref dr));

            }

            return lDiario;

        }

        public List<Diario> LoadByDiarioTurma(ref Turma _turma)
        {
            List<Diario> lDiario = new List<Diario>();

            StringBuilder sb = GetPrefix();

            sb.AppendLine("WHERE turma_idturma = @turma_idturma; ");

            MySqlCommand myCmd = new MySqlCommand();
            myCmd.Connection = _db.Connection;
            myCmd.CommandText = sb.ToString();
            myCmd.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = _turma.Idturma;

            DataTable _dt = _db.ExecuteQuery(ref myCmd);


            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                DataRow dr = _dt.Rows[i];

                lDiario.Add(loadObject2(ref dr));

            }

            return lDiario;

        }

        public Diario LoadById(int iddiario)
        {
            Diario _diario = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE iddiario = @iddiario ");

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@iddiario", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = iddiario;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _diario = loadObject(ref dr);

            }

            return _diario;

        }

        public Diario LoadById2(int iddiario)
        {
            Diario _diario = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE iddiario = @iddiario ");

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@iddiario", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = iddiario;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _diario = loadObject2(ref dr);

            }

            return _diario;

        }

        public Diario diarioByTurma(ref Turma _t, DateTime _d, UnidadeCurricular _uc)
        {
            Diario _diario = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE                                 ");
            sbSQL.AppendLine("      turma_idturma = @turma_idturma  ");
            sbSQL.AppendLine("AND   data = @data                    ");
            sbSQL.AppendLine("AND   unidade_curricular_idunidadecurricular = @unidade_curricular_idunidadecurricular;");

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Parameters.Add("@turma_idturma", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = _t.Idturma;
            myCMD.Parameters.Add("@data", MySql.Data.MySqlClient.MySqlDbType.Date, 0).Value = _d;
            myCMD.Parameters.Add("@unidade_curricular_idunidadecurricular", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = _uc.IdUnidadeCurricular;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _diario = loadObject2(ref dr);

            }

            return _diario;
        }

        public void Insert(ref Diario d)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO                                  ");
            sb.AppendLine("  `diario`                                   ");
            sb.AppendLine("(                                            ");
            sb.AppendLine("  `conteudo`,                                ");
            sb.AppendLine("  `ocorrencia`,                              ");
            sb.AppendLine("  `data`,                                    ");
            sb.AppendLine("  `hora_aula_dia`,                           ");
            sb.AppendLine("  `turma_idturma`,                           ");
            sb.AppendLine("  `unidade_curricular_idunidadecurricular`)  ");
            sb.AppendLine("VALUE (                                      ");
            sb.AppendLine("  @conteudo,                                 ");
            sb.AppendLine("  @ocorrencia,                               ");
            sb.AppendLine("  @data,                                     ");
            sb.AppendLine("  @hora_aula_dia,                            ");
            sb.AppendLine("  @turma_idturma,                            ");
            sb.AppendLine("  @unidade_curricular_idunidadecurricular);  ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Connection = _db.Connection;
                myCMD.Parameters.Add("@conteudo", MySqlDbType.VarChar, 151).Value = d.Conteudo.ToUpper();
                if (d.Ocorrencia == null)
                {
                    myCMD.Parameters.Add("@ocorrencia", MySqlDbType.VarChar, 300).Value = "";
                }
                else
                {
                    myCMD.Parameters.Add("@ocorrencia", MySqlDbType.VarChar, 300).Value = d.Ocorrencia.ToUpper();
                }
                myCMD.Parameters.Add("@data", MySqlDbType.Date, 0).Value = d.Data;
                myCMD.Parameters.Add("@hora_aula_dia", MySqlDbType.Decimal, 0).Value = d.HoraAulaDia;
                myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = d.IdTurma;
                myCMD.Parameters.Add("@unidade_curricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = d.IdUnidadeCurricular;

                myCMD.ExecuteNonQuery();

                d.IdDiario = myCMD.LastInsertedId;
            }
            catch (Exception)
            {

                throw;
            }
            

            FrequenciaDAO frequenciaDAO = new FrequenciaDAO(ref _db);

            for (int i = 0; i < d.Frequencias.Count; i++)
            {

                Frequencia f = d.Frequencias[i];
                frequenciaDAO.Insert(ref f, d.IdDiario);

            }
        }

        public void Update(ref Diario d)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE                                                                               ");
            sb.AppendLine("  `diario`                                                                           ");
            sb.AppendLine("SET                                                                                  ");
            sb.AppendLine("  `conteudo` = @conteudo,                                                            ");
            sb.AppendLine("  `ocorrencia` = @ocorrencia,                                                         ");
            sb.AppendLine("  `data` = @data,                                                                     ");
            sb.AppendLine("  `hora_aula_dia` = @hora_aula_dia,                                                   ");
            sb.AppendLine("  `turma_idturma` = @turma_idturma,                                                   ");
            sb.AppendLine("  `unidade_curricular_idunidadecurricular` = @unidade_curricular_idunidadecurricular ");
            sb.AppendLine("WHERE                                                                                ");
            sb.AppendLine("     `iddiario` = @iddiario;                                                          ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Transaction = _db.Transaction;
            myCMD.CommandText = sb.ToString();
            myCMD.Connection = _db.Connection;

            myCMD.Parameters.Add("@iddiario", MySqlDbType.Int16, 0).Value = d.IdDiario;
            myCMD.Parameters.Add("@conteudo", MySqlDbType.VarChar, 151).Value = d.Conteudo.ToUpper();
            if (d.Ocorrencia == null)
            {
                myCMD.Parameters.Add("@ocorrencia", MySqlDbType.VarChar, 300).Value = "";
            }
            else
            {
                myCMD.Parameters.Add("@ocorrencia", MySqlDbType.VarChar, 300).Value = d.Ocorrencia.ToUpper();
            }
            myCMD.Parameters.Add("@data", MySqlDbType.Date, 0).Value = d.Data;
            myCMD.Parameters.Add("@hora_aula_dia", MySqlDbType.Decimal, 0).Value = d.HoraAulaDia;
            myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = d.IdTurma;
            myCMD.Parameters.Add("@unidade_curricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = d.IdUnidadeCurricular;

            myCMD.ExecuteNonQuery();

            //d.IdDiario = myCMD.LastInsertedId;

            FrequenciaDAO frequenciaDAO = new FrequenciaDAO(ref _db);

            for (int i = 0; i < d.Frequencias.Count; i++)
            {

                Frequencia f = d.Frequencias[i];
                frequenciaDAO.Update(ref f, d.IdDiario);

            }
        }

        public void Delete(int iddiario)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM                  ");
            sb.AppendLine("  `diario`                ");
            sb.AppendLine("WHERE                        ");
            sb.AppendLine("  iddiario = @iddiario ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@iddiario", MySqlDbType.Int16, 0).Value = iddiario;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }

        //public void Insert(Diario _obj)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine("INSERT INTO                                  ");
        //    sb.AppendLine("  `diario`                                   ");
        //    sb.AppendLine("(                                            ");
        //    sb.AppendLine("  `conteudo`,                                ");
        //    sb.AppendLine("  `ocorrencia`,                              ");
        //    sb.AppendLine("  `data`,                                    ");
        //    sb.AppendLine("  `turma_idturma`,                           ");
        //    sb.AppendLine("  `unidade_curricular_idunidadecurricular`)  ");
        //    sb.AppendLine("VALUE (                                      ");
        //    sb.AppendLine("  @conteudo,                                 ");
        //    sb.AppendLine("  @ocorrencia,                               ");
        //    sb.AppendLine("  @data,                                     ");
        //    sb.AppendLine("  @turma_idturma,                            ");
        //    sb.AppendLine("  @unidade_curricular_idunidadecurricular);  ");
        //    try
        //    {
        //        MySqlCommand myCMD = new MySqlCommand();
        //        myCMD.Transaction = _db.Transaction;
        //        myCMD.CommandText = sb.ToString();
        //        myCMD.Connection = _db.Connection;
        //        myCMD.Parameters.Add("@conteudo", MySqlDbType.VarChar, 200).Value = _obj.Conteudo.ToUpper();
        //        if (_obj.Ocorrencia == null)
        //        {
        //            myCMD.Parameters.Add("@ocorrencia", MySqlDbType.VarChar, 200).Value = " ";
        //        }
        //        else
        //        {
        //            myCMD.Parameters.Add("@ocorrencia", MySqlDbType.VarChar, 200).Value = _obj.Ocorrencia.ToUpper();
        //        }
        //        myCMD.Parameters.Add("@data", MySqlDbType.Date, 0).Value = _obj.Data.ToString("yyyy-MM-dd");
        //        myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = _obj.Turma.Idturma;
        //        myCMD.Parameters.Add("@unidade_curricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = _obj.UnidadeCurricular.IdUnidadeCurricular;

        //        myCMD.ExecuteNonQuery();

        //        //_obj.IdDiario = myCMD.LastInsertedId;
        //    }
        //    catch (Exception _ex)
        //    {
        //        throw _ex;

        //    }
        //}

        //public void Update(Diario _obj)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine("UPDATE                                       ");
        //    sb.AppendLine("  `diario`                                   ");
        //    sb.AppendLine("SET                                          ");
        //    sb.AppendLine("  `conteudo` = @conteudo,                    ");
        //    sb.AppendLine("  `ocorrencia` = @ocorrencia,                ");
        //    sb.AppendLine("  `data` = @data,                            ");
        //    sb.AppendLine("  `turma_idturma` = @turma_idturma,          ");
        //    sb.AppendLine("  `unidade_curricular_idunidadecurricular` = @unidade_curricular_idunidadecurricular ");
        //    sb.AppendLine("WHERE ");
        //    sb.AppendLine("  `iddiario` = @iddiario;");

        //    try
        //    {
        //        MySqlCommand myCMD = new MySqlCommand();
        //        myCMD.Transaction = _db.Transaction;
        //        myCMD.CommandText = sb.ToString();
        //        myCMD.Connection = _db.Connection;
        //        myCMD.Parameters.Add("@conteudo", MySqlDbType.VarChar, 200).Value = _obj.Conteudo.ToUpper();
        //        if (_obj.Ocorrencia == null)
        //        {
        //            myCMD.Parameters.Add("@ocorrencia", MySqlDbType.VarChar, 200).Value = " ";
        //        }
        //        else
        //        {
        //            myCMD.Parameters.Add("@ocorrencia", MySqlDbType.VarChar, 200).Value = _obj.Ocorrencia.ToUpper();
        //        }

        //        myCMD.Parameters.Add("@data", MySqlDbType.Date, 0).Value = _obj.Data.ToString("yyyy-MM-dd");
        //        myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = _obj.Turma.Idturma;
        //        myCMD.Parameters.Add("@unidade_curricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = _obj.UnidadeCurricular.IdUnidadeCurricular;
        //        myCMD.Parameters.Add("@iddiario", MySqlDbType.Int16, 0).Value = _obj.IdDiario;

        //        myCMD.ExecuteNonQuery();

        //    }
        //    catch (Exception _ex)
        //    {
        //        throw _ex;

        //    }
        //}
    }
}