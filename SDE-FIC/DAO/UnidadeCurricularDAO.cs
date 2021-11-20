using SDE_FIC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using SDE_FIC.Util;
using MySql;

namespace SDE_FIC.DAO
{
    public class UnidadeCurricularDAO
    {
        private DBConnect _db;

        public UnidadeCurricularDAO(ref DBConnect db)
        {
            this._db = db;
        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT                       ");
            sb.AppendLine(" `idunidadecurricular`,      ");
            sb.AppendLine(" `unidadecurricular`,        ");
            sb.AppendLine(" `sigla`,             ");
            sb.AppendLine(" `cargahoraria`,             ");
            sb.AppendLine(" `curso_idcurso`             ");
            sb.AppendLine("FROM                         ");
            sb.AppendLine(" `unidadecurricular`         ");

            return sb;
        }

        private UnidadeCurricular loadObject(ref DataRow Rows)
        {
            UnidadeCurricular _u = new UnidadeCurricular();
            CursoDAO _cursoDAO = new CursoDAO(ref _db);

            _u.IdUnidadeCurricular = (int)Rows["idunidadecurricular"];
            _u.Descricao = (string)Rows["unidadecurricular"].ToString().ToUpper();
            _u.Sigla = Convert.ToString(ValueUtil.DBNullToString(Rows["sigla"])).ToString().ToUpper();
            _u.CargaHoraria = (int)Rows["cargahoraria"];
            _u.Curso = _cursoDAO.LoadById((int)Rows["curso_idcurso"]);


            return _u;
        }
       
        public List<UnidadeCurricular> LoadByCurso(Curso _curso)
        {
            List<UnidadeCurricular> lUnidadeCurricular = new List<UnidadeCurricular>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE curso_idcurso = @curso_idcurso ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Parameters.Add("@curso_idcurso", MySqlDbType.Int16, 0).Value = _curso.IdCurso;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                lUnidadeCurricular.Add(loadObject(ref dr));

            }

            return lUnidadeCurricular;

        }

        public UnidadeCurricular LoadById(int idunidadecurricular)
        {
            UnidadeCurricular _u = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idunidadecurricular = @idunidadecurricular ");

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@idunidadecurricular", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = idunidadecurricular;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _u = loadObject(ref dr);

            }

            return _u;

        }

        public List<UnidadeCurricular> LoadByUnidadeCurricular(ref UnidadeCurricular unidadecurricular)
        {
            List<UnidadeCurricular> lUnidadeCurricular = new List<UnidadeCurricular>();

            StringBuilder sb = GetPrefix();

            return lUnidadeCurricular;

        }

        /// <summary>
        /// Listar
        /// </summary>
        /// <returns></returns>
        public List<UnidadeCurricular> All()
        {
            List<UnidadeCurricular> lUnidadeCurricular = new List<UnidadeCurricular>();

            StringBuilder sb = GetPrefix();

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;

            myCMD.CommandText = sb.ToString();


            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lUnidadeCurricular.Add(loadObject(ref dr));
            }

            return lUnidadeCurricular;

        }

        /// <summary>
        /// Inserir novo registro
        /// </summary>
        /// <param name="_obj"></param>
        public void Insert(UnidadeCurricular _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `unidadecurricular` ");
            sb.AppendLine("(");
            sb.AppendLine("  `unidadecurricular`,");
            sb.AppendLine("  `sigla`,");
            sb.AppendLine("  `cargahoraria`,");
            sb.AppendLine("  `curso_idcurso`) ");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @unidadecurricular,");
            sb.AppendLine("  @sigla,");
            sb.AppendLine("  @cargahoraria,");
            sb.AppendLine("  @curso_idcurso);");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@unidadecurricular", MySqlDbType.VarChar, 45).Value = _obj.Descricao.ToUpper();
                myCMD.Parameters.Add("@sigla", MySqlDbType.VarChar, 10).Value = _obj.Sigla;
                myCMD.Parameters.Add("@cargahoraria", MySqlDbType.Int16, 0).Value = _obj.CargaHoraria;
                myCMD.Parameters.Add("@curso_idcurso", MySqlDbType.Int16, 0).Value = _obj.Curso.IdCurso;

                myCMD.ExecuteNonQuery();

                //obter o id gerado
                _obj.IdUnidadeCurricular = myCMD.LastInsertedId;

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
        public int Update(UnidadeCurricular _obj)
        {
            int iAffectedRows = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE ");
            sb.AppendLine("  `unidadecurricular`  ");
            sb.AppendLine("SET ");
            sb.AppendLine("  `unidadecurricular` = @unidadecurricular,");
            sb.AppendLine("  `sigla` = @sigla,");
            sb.AppendLine("  `cargahoraria` = @cargahoraria,");
            sb.AppendLine("  `curso_idcurso` = @curso_idcurso ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("  `idunidadecurricular` = @idunidadecurricular;");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@unidadecurricular", MySqlDbType.VarChar, 45).Value = _obj.Descricao.ToUpper();
                if (_obj.Sigla == null)
                {
                    myCMD.Parameters.Add("@sigla", MySqlDbType.VarChar, 10).Value = " ";
                }
                else {
                    myCMD.Parameters.Add("@sigla", MySqlDbType.VarChar, 10).Value = _obj.Sigla.ToUpper();
                }                
                myCMD.Parameters.Add("@cargahoraria", MySqlDbType.Int16, 0).Value = _obj.CargaHoraria;
                myCMD.Parameters.Add("@curso_idcurso", MySqlDbType.Int16, 0).Value = _obj.Curso.IdCurso;
                myCMD.Parameters.Add("@idunidadecurricular", MySqlDbType.Int16, 0).Value = _obj.IdUnidadeCurricular;

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
        public void Delete(UnidadeCurricular _obj) 
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM                                  ");
            sb.AppendLine("  `unidadecurricular`                        ");
            sb.AppendLine("WHERE                                        ");
            sb.AppendLine("  idunidadecurricular = @idunidadecurricular ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@idunidadecurricular", MySqlDbType.Int16).Value = _obj.IdUnidadeCurricular;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }   

        }




    }
}