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
    public class NotasDAO
    {
        private DBConnect _db;

        public NotasDAO(ref DBConnect db)
        {
            this._db = db;

        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("  `idnotas`,");
            sb.AppendLine("  `nota`,");
            sb.AppendLine("  `matricula_idmatricula`,");
            sb.AppendLine("  `unidadecurricular_idunidadecurricular`");
            sb.AppendLine("FROM ");
            sb.AppendLine("  `notas`");

            return sb;
        }

        private Notas LoadObject(ref DataRow Rows)
        {
            Notas _notas = new Notas();
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);
            UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);

            _notas.IdNotas = (int)Rows["idnotas"];
            _notas.Nota = (decimal)Rows["nota"];
            _notas.Matricula = _matriculaDAO.LoadById(Convert.ToInt16(Rows["matricula_idmatricula"]));
            _notas.UnidadeCurricular = _unidadecurricularDAO.LoadById(Convert.ToInt16(Rows["unidadecurricular_idunidadecurricular"]));

            return _notas;

        }

        public List<Notas> LoadByMatriculaId(int IdMatricula)
        {
            List<Notas> lNotas = new List<Notas>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE matricula_idmatricula = @matricula_idmatricula; ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = IdMatricula;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = dt.Rows[i];
                lNotas.Add(LoadObject(ref dr));

            }

            return lNotas;

        }

        public List<Notas> LoadByMatricula(Matricula _matricula)
        {
            List<Notas> lNotas = new List<Notas>();

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
                lNotas.Add(LoadObject(ref dr));

            }

            return lNotas;

        }
       
        public List<Notas> LoadByUnidadeCurricular(UnidadeCurricular _unidadecurricular)
        {
            List<Notas> lNotas = new List<Notas>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE unidadecurricular_idunidadecurricular = @unidadecurricular_idunidadecurricular; ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@unidadecurricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = _unidadecurricular.IdUnidadeCurricular;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = dt.Rows[i];

                lNotas.Add(LoadObject(ref dr));

            }

            return lNotas;

        }

        public List<Notas> LoadByDiarioId(int id)
        {
            List<Notas> lNotas = new List<Notas>();

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
                lNotas.Add(LoadObject(ref dr));

            }

            return lNotas;

        }

        public Notas LoadByIdMIdUC(long IdMatricula, int IdUnidadeCurricular)
        {
            Notas _notas = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE matricula_idmatricula = @matricula_idmatricula ");
            sbSQL.AppendLine(" AND unidadecurricular_idunidadecurricular = @unidadecurricular_idunidadecurricular; ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = IdMatricula;
            myCMD.Parameters.Add("@unidadecurricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = IdUnidadeCurricular;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                _notas = LoadObject(ref dr);
            }

            return _notas;

        }


        public Notas LoadById(int IdNotas)
        {
            Notas _notas = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idnotas = @idnotas ");

            MySqlCommand mySQL = new MySqlCommand();
            mySQL.CommandText = sbSQL.ToString();
            mySQL.Connection = _db.Connection;
            mySQL.Parameters.Add("@idnotas", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = IdNotas;

            DataTable dt = _db.ExecuteQuery(ref mySQL);

            if (dt.Rows.Count == 1)
            {

                DataRow dr = dt.Rows[0];

                _notas = LoadObject(ref dr);

            }

            return _notas;

        }
        
        //public void Insert(ref Notas _notas, int idUnidadeCurricular)
        //{
   
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine("INSERT INTO ");
        //    sb.AppendLine("  `notas`");
        //    sb.AppendLine("(");
        //    sb.AppendLine("  `nota`,");
        //    sb.AppendLine("  `matricula_idmatricula`,");
        //    sb.AppendLine("  `unidadecurricular_idunidadecurricular`) ");
        //    sb.AppendLine("VALUE (");
        //    sb.AppendLine("  @nota,");
        //    sb.AppendLine("  @matricula_idmatricula,");
        //    sb.AppendLine("  @unidadecurricular_idunidadecurricular);");


        //    try
        //    {
        //        MySqlCommand myCMD = new MySqlCommand();
        //        myCMD.Connection = _db.Connection;
        //        myCMD.Transaction = _db.Transaction;
        //        myCMD.CommandText = sb.ToString();

        //        myCMD.Parameters.Add("@nota", MySqlDbType.Decimal, 0).Value = _notas.Nota;
        //        myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = _notas.Matricula.IdMatricula;
        //        myCMD.Parameters.Add("@unidadecurricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = idUnidadeCurricular;

        //        myCMD.ExecuteNonQuery();

        //        //obter o id gerado
        //        _notas.IdNotas = myCMD.LastInsertedId;
        //    }
        //    catch (Exception _ex)
        //    {
        //        throw _ex;

        //    }
            
        //}

        public void Insert(int IdMatricula , string Nota, int idUnidadeCurricular)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `notas`");
            sb.AppendLine("(");
            sb.AppendLine("  `nota`,");
            sb.AppendLine("  `matricula_idmatricula`,");
            sb.AppendLine("  `unidadecurricular_idunidadecurricular`) ");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @nota,");
            sb.AppendLine("  @matricula_idmatricula,");
            sb.AppendLine("  @unidadecurricular_idunidadecurricular);");


            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();

                myCMD.Parameters.Add("@nota", MySqlDbType.Int16, 0).Value = Convert.ToInt16(Nota);
                myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = IdMatricula;
                myCMD.Parameters.Add("@unidadecurricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = idUnidadeCurricular;

                myCMD.ExecuteNonQuery();

                //obter o id gerado
                //_nota.IdNotas = myCMD.LastInsertedId;
            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }


        public void Update(ref Notas _notas)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE ");
            sb.AppendLine("  `notas` ");
            sb.AppendLine("SET ");
            sb.AppendLine("  `nota` = @nota, ");
            sb.AppendLine("  `matricula_idmatricula` = @matricula_idmatricula, ");
            sb.AppendLine("  `unidadecurricular_idunidadecurricular` = @unidadecurricular_idunidadecurricular ");
            sb.AppendLine("WHERE");
            sb.AppendLine("  `idnotas` = @idnotas");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.Transaction = _db.Transaction;
            myCMD.CommandText = sb.ToString();
            myCMD.Parameters.Add("@idnotas", MySqlDbType.Int16, 0).Value = _notas.IdNotas;
            myCMD.Parameters.Add("@nota", MySqlDbType.Int16, 0).Value = Convert.ToInt16(_notas.Nota);
            myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = _notas.Matricula.IdMatricula;
            myCMD.Parameters.Add("@unidadecurricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = _notas.UnidadeCurricular.IdUnidadeCurricular;


            myCMD.ExecuteNonQuery();

        }

        public int Delete(Notas _notas)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" DELETE FROM notas ");
            sb.AppendLine(" WHERE idnota = @idnota ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sb.ToString();
            myCMD.Transaction = _db.Transaction;
            myCMD.Parameters.Add("@idnota", MySqlDbType.Int16, 0).Value = _notas.IdNotas;

            int iAffected = myCMD.ExecuteNonQuery();

            return iAffected;

        }
    }
}