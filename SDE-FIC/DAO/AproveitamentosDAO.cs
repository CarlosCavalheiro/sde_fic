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
    public class AproveitamentosDAO
    {
        private DBConnect _db;

        public AproveitamentosDAO(ref DBConnect db)
        {
            this._db = db;

        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("  `idaproveitamento`,");
            sb.AppendLine("  `matricula_idmatricula`,");
            sb.AppendLine("  `unidadecurricular_idunidadecurricular`");
            sb.AppendLine("FROM ");
            sb.AppendLine("  `aproveitamento`");

            return sb;
        }

        private Aproveitamentos LoadObject(ref DataRow Rows)
        {
            Aproveitamentos _aproveitamento = new Aproveitamentos();
            MatriculaDAO _matriculaDAO = new MatriculaDAO(ref _db);
            UnidadeCurricularDAO _unidadecurricularDAO = new UnidadeCurricularDAO(ref _db);

            _aproveitamento.IdAproveitamento = (int)Rows["idaproveitamento"];
            _aproveitamento.Matricula = _matriculaDAO.LoadById(Convert.ToInt16(Rows["matricula_idmatricula"]));
            _aproveitamento.UnidadeCurricular = _unidadecurricularDAO.LoadById(Convert.ToInt16(Rows["unidadecurricular_idunidadecurricular"]));

            return _aproveitamento;

        }

        public List<Aproveitamentos> LoadByMatriculaId(int IdMatricula)
        {
            List<Aproveitamentos> lNotas = new List<Aproveitamentos>();

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

        public List<Aproveitamentos> LoadByMatricula(Matricula _matricula)
        {
            List<Aproveitamentos> lAproveitamentos = new List<Aproveitamentos>();

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
                lAproveitamentos.Add(LoadObject(ref dr));

            }

            return lAproveitamentos;

        }
       
        public List<Aproveitamentos> LoadByUnidadeCurricular(UnidadeCurricular _unidadecurricular)
        {
            List<Aproveitamentos> lAproveitamentos = new List<Aproveitamentos>();

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

                lAproveitamentos.Add(LoadObject(ref dr));

            }

            return lAproveitamentos;

        }

        public List<Aproveitamentos> LoadByDiarioId(int id)
        {
            List<Aproveitamentos> lAproveitamentos = new List<Aproveitamentos>();

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
                lAproveitamentos.Add(LoadObject(ref dr));

            }

            return lAproveitamentos;

        }

        public Aproveitamentos LoadByIdMIdUC(long IdMatricula, int IdUnidadeCurricular)
        {
            Aproveitamentos lAproveitamentos = null;

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
                lAproveitamentos = LoadObject(ref dr);
            }

            return lAproveitamentos;

        }


        public List<Aproveitamentos> LoadByTurma(Turma turma)
        {
            List<Aproveitamentos> lAproveitamentos = new List<Aproveitamentos>();

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE matricula_idmatricula = @matricula_idmatricula; ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            //myCMD.Parameters.Add("@turma_idturma", MySqlDbType.Int16, 0).Value = turma.Idturma;
            myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = turma.Matriculas;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = dt.Rows[i];

                lAproveitamentos.Add(LoadObject(ref dr));

            }

            return lAproveitamentos;

        }

        public Aproveitamentos LoadById(int IdAproveitamentos)
        {
            Aproveitamentos lAproveitamentos = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idaproveitamento = @idaproveitamento ");

            MySqlCommand mySQL = new MySqlCommand();
            mySQL.CommandText = sbSQL.ToString();
            mySQL.Connection = _db.Connection;
            mySQL.Parameters.Add("@idaproveitamento", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = IdAproveitamentos;

            DataTable dt = _db.ExecuteQuery(ref mySQL);

            if (dt.Rows.Count == 1)
            {

                DataRow dr = dt.Rows[0];

                lAproveitamentos = LoadObject(ref dr);

            }

            return lAproveitamentos;

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

        public void Insert(int IdMatricula , int idUnidadeCurricular)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `aproveitamento`");
            sb.AppendLine("(");

            sb.AppendLine("  `matricula_idmatricula`,");
            sb.AppendLine("  `unidadecurricular_idunidadecurricular`) ");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @matricula_idmatricula,");
            sb.AppendLine("  @unidadecurricular_idunidadecurricular);");


            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();

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


        public void Update(ref Aproveitamentos _Aproveitamentos)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE ");
            sb.AppendLine("  `aproveitamento` ");
            sb.AppendLine("SET ");
            sb.AppendLine("  `matricula_idmatricula` = @matricula_idmatricula, ");
            sb.AppendLine("  `unidadecurricular_idunidadecurricular` = @unidadecurricular_idunidadecurricular ");
            sb.AppendLine("WHERE");
            sb.AppendLine("  `idaproveitamento` = @idaproveitamento");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.Transaction = _db.Transaction;
            myCMD.CommandText = sb.ToString();
            myCMD.Parameters.Add("@idaproveitamento", MySqlDbType.Int16, 0).Value = _Aproveitamentos.IdAproveitamento;
            myCMD.Parameters.Add("@matricula_idmatricula", MySqlDbType.Int16, 0).Value = _Aproveitamentos.Matricula.IdMatricula;
            myCMD.Parameters.Add("@unidadecurricular_idunidadecurricular", MySqlDbType.Int16, 0).Value = _Aproveitamentos.UnidadeCurricular.IdUnidadeCurricular;


            myCMD.ExecuteNonQuery();

        }

        public void Delete(Aproveitamentos _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM                                  ");
            sb.AppendLine("  `aproveitamento` ");
            sb.AppendLine("WHERE                                        ");
            sb.AppendLine("  idaproveitamento = @idaproveitamento ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@idaproveitamento", MySqlDbType.Int16).Value = _obj.IdAproveitamento;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }
    }
}