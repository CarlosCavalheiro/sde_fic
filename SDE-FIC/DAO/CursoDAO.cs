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
    public class CursoDAO
    {

        private DBConnect _db;
        private UnidadeCurricularDAO unidadeCurricularDAO;

        public CursoDAO(ref DBConnect db)
        {
            this._db = db;
            unidadeCurricularDAO = new UnidadeCurricularDAO(ref _db);
        }

        public CursoDAO()
        {
            // TODO: Complete member initialization
        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" `idcurso`,");
            sb.AppendLine(" `cursonome`,");
            sb.AppendLine(" `descricao`,");
            sb.AppendLine(" `cargahoraria`");
            sb.AppendLine("FROM ");
            sb.AppendLine(" `curso`");

            return sb;
        }

        private Curso loadObject(ref DataRow Rows)
        {
            Curso _c = new Curso();

            _c.IdCurso = (int)Rows["idcurso"];
            _c.CursoNome = (string)Rows["cursonome"].ToString().ToUpper();
            _c.Descricao = (string)Rows["descricao"].ToString().ToUpper();
            _c.CargaHoraria = (int)Rows["cargahoraria"];

            //_c.UnidadesCurriculares = unidadeCurricularDAO.LoadByCurso(_c);

            return _c;
        }

        private Curso loadObject2(ref DataRow Rows)
        {
            Curso _c = new Curso();

            _c.IdCurso = (int)Rows["idcurso"];
            _c.CursoNome = (string)Rows["cursonome"].ToString().ToUpper();
            _c.Descricao = (string)Rows["descricao"].ToString().ToUpper();
            _c.CargaHoraria = (int)Rows["cargahoraria"];

            _c.lUnidadesCurriculares = unidadeCurricularDAO.LoadByCurso(_c);

            return _c;
        }

        public Curso LoadById(int idcurso)
        {
            Curso _curso = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idcurso = @idcurso ");

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@idcurso", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = idcurso;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _curso = loadObject(ref dr);
            
            }

            return _curso;

        }

        public Curso LoadById2(int idcurso)
        {
            Curso _curso = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idcurso = @idcurso ");

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@idcurso", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = idcurso;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _curso = loadObject2(ref dr);

            }

            return _curso;

        }

        /// <summary>
        /// Listar
        /// </summary>
        /// <returns></returns>
        public List<Curso> All()
        {
            List<Curso> lCurso = new List<Curso>();

            StringBuilder sb = GetPrefix();

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;

            myCMD.CommandText = sb.ToString();


            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lCurso.Add(loadObject(ref dr));
            }

            return lCurso;

        }

        /// <summary>
        /// Inserir novo registro
        /// </summary>
        /// <param name="_obj"></param>
        public void Insert(Curso _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `curso` ");
            sb.AppendLine("(");
            sb.AppendLine("  `cursonome`,");
            sb.AppendLine("  `descricao`,");
            sb.AppendLine("  `cargahoraria`) ");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @cursonome,");
            sb.AppendLine("  @descricao,");
            sb.AppendLine("  @cargahoraria);");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@cursonome", MySqlDbType.VarChar, 100).Value = _obj.CursoNome.ToUpper();
                myCMD.Parameters.Add("@descricao", MySqlDbType.VarChar, 255).Value = _obj.Descricao.ToUpper();
                myCMD.Parameters.Add("@cargahoraria", MySqlDbType.Int16, 0).Value = _obj.CargaHoraria;

                myCMD.ExecuteNonQuery();

                //obter o id gerado
                //_obj.IdCurso = myCMD.LastInsertedId;

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
        public int Update(Curso _obj)
        {
            int iAffectedRows = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE ");
            sb.AppendLine("  `curso`  ");
            sb.AppendLine("SET ");
            sb.AppendLine("  `cursonome` = @cursonome,");
            sb.AppendLine("  `descricao` = @descricao,");
            sb.AppendLine("  `cargahoraria` = @cargahoraria ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("  `idcurso` = @idcurso;");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@cursonome", MySqlDbType.VarChar, 100).Value = _obj.CursoNome.ToUpper();
                myCMD.Parameters.Add("@descricao", MySqlDbType.VarChar, 255).Value = _obj.Descricao.ToUpper();
                myCMD.Parameters.Add("@cargahoraria", MySqlDbType.Int16, 0).Value = _obj.CargaHoraria;
                myCMD.Parameters.Add("@idcurso", MySqlDbType.Int16, 0).Value = _obj.IdCurso;

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
        public void Delete(Curso _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM                                  ");
            sb.AppendLine("  `curso`                        ");
            sb.AppendLine("WHERE                                        ");
            sb.AppendLine("  idcurso = @idcurso ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@idcurso", MySqlDbType.Int16).Value = _obj.IdCurso;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }
    }
}