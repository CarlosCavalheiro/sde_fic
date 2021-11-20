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
    public class FuncionarioDAO
    {
        private DBConnect _db;

        public FuncionarioDAO(ref DBConnect db)
        {
            this._db = db;

        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("  `idfuncionario`,");
            sb.AppendLine("  `nomecompleto`,");
            sb.AppendLine("  `email`,");
            sb.AppendLine("  `telefonecel`,");
            sb.AppendLine("  `telefoneres`,");
            sb.AppendLine("  `especialidade`,");
            sb.AppendLine("  `obs`,");
            sb.AppendLine("  `usuario_idusuario`");
            sb.AppendLine(" FROM ");
            sb.AppendLine("  `funcionario`");

            return sb;

        }

        private Funcionario loadObject(ref DataRow Rows) {

            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);
            var _p = new Funcionario();

            _p.IdFuncionario = (int)Rows["idfuncionario"];
            _p.NomeCompleto = (string)Rows["nomecompleto"].ToString().ToUpper();
            _p.Email = (string)Rows["email"].ToString().ToLower();
            _p.TelefoneCelular = (string)Rows["telefonecel"].ToString();
            _p.TelefoneResidencial = (string)Rows["telefoneres"].ToString();
            _p.Especialidade = (string)Rows["especialidade"].ToString();
            _p.Observacao = (string)Rows["obs"].ToString();
            _p.IdUsuario = (int)Rows["usuario_idusuario"];
            _p.Usuario = _usuarioDAO.LoadById((int)Rows["usuario_idusuario"]);


            return _p;
        }

        public Funcionario LoadById(int IdFuncionario)
        {
            Funcionario _funcionario = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idfuncionario = @idfuncionario ");

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Parameters.Add("@idfuncionario", MySqlDbType.Int16, 0).Value = IdFuncionario;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _funcionario = loadObject(ref dr);
            }

            return _funcionario;
        }

        public Funcionario LoadByNome(string nomecompleto)
        {
            Funcionario _funcionario = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE nomecompleto = @nomecompleto ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Parameters.Add("@nomecompleto", MySqlDbType.VarChar, 100).Value = nomecompleto;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _funcionario = loadObject(ref dr);
            }

            return _funcionario;
        }

        public List<Funcionario> All()
        {
            List<Funcionario> lFuncionario = new List<Funcionario>();

            StringBuilder sb = GetPrefix();

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;

            myCMD.CommandText = sb.ToString();


            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lFuncionario.Add(loadObject(ref dr));
            }

            return lFuncionario;

        }

        /// <summary>
        /// Inserir Novo Registro
        /// </summary>
        /// <param name="_obj"></param>
        public void Insert(Funcionario _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `funcionario` ");
            sb.AppendLine("(");
            sb.AppendLine("  `nomecompleto`,");
            sb.AppendLine("  `email`,");
            sb.AppendLine("  `telefonecel`,");
            sb.AppendLine("  `telefoneres`,");
            sb.AppendLine("  `especialidade`,");
            sb.AppendLine("  `obs`,");
            sb.AppendLine("  `usuario_idusuario`) ");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @nomecompleto,");
            sb.AppendLine("  @email,");
            sb.AppendLine("  @telefonecel,");
            sb.AppendLine("  @telefoneres,");
            sb.AppendLine("  @especialidade,");
            sb.AppendLine("  @obs,");
            sb.AppendLine("  @usuario_idusuario);");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@nomecompleto", MySqlDbType.VarChar, 100).Value = _obj.NomeCompleto.ToUpper();
                myCMD.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = _obj.Email;
                myCMD.Parameters.Add("@telefonecel", MySqlDbType.VarChar, 45).Value = _obj.TelefoneCelular;
                myCMD.Parameters.Add("@telefoneres", MySqlDbType.VarChar, 45).Value = _obj.TelefoneResidencial;
                myCMD.Parameters.Add("@especialidade", MySqlDbType.VarChar, 255).Value = _obj.Especialidade;
                myCMD.Parameters.Add("@obs", MySqlDbType.VarChar, 255).Value = _obj.Observacao;
                myCMD.Parameters.Add("@usuario_idusuario", MySqlDbType.Int32, 0).Value = _obj.IdUsuario;
                myCMD.ExecuteNonQuery();

                //obter o id gerado
                //_obj.IdFuncionario = myCMD.LastInsertedId;

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
        public int Update(Funcionario _obj)
        {
            int iAffectedRows = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE ");
            sb.AppendLine("  `funcionario` ");
            sb.AppendLine(" SET ");
            sb.AppendLine("  `nomecompleto` = @nomecompleto,");
            sb.AppendLine("  `email` = @email,");
            sb.AppendLine("  `telefoneres` = @telefoneres,");
            sb.AppendLine("  `telefonecel` = @telefonecel,");
            sb.AppendLine("  `especialidade` = @especialidade, ");
            sb.AppendLine("  `obs`  = @obs, ");
            sb.AppendLine("  `usuario_idusuario` = @usuario_idusuario");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("  `idfuncionario` = @idfuncionario;");


            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@nomecompleto", MySqlDbType.VarChar, 100).Value = _obj.NomeCompleto.ToUpper();
                myCMD.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = _obj.Email;
                myCMD.Parameters.Add("@telefonecel", MySqlDbType.VarChar, 45).Value = _obj.TelefoneCelular;
                myCMD.Parameters.Add("@telefoneres", MySqlDbType.VarChar, 45).Value = _obj.TelefoneResidencial;
                myCMD.Parameters.Add("@especialidade", MySqlDbType.VarChar, 255).Value = _obj.Especialidade;
                myCMD.Parameters.Add("@obs", MySqlDbType.VarChar, 2555).Value = _obj.Observacao;
                myCMD.Parameters.Add("@usuario_idusuario", MySqlDbType.Int32, 0).Value = _obj.IdUsuario;
                myCMD.Parameters.Add("@idfuncionario", MySqlDbType.Int32, 0).Value = _obj.IdFuncionario;


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
        public void Delete(int IdFuncionario)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM          ");
            sb.AppendLine("  `funcionario`      ");
            sb.AppendLine("WHERE                ");
            sb.AppendLine("  idfuncionario = @idfuncionario ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@idfuncionario", MySqlDbType.Int32).Value = IdFuncionario;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }

    }
}