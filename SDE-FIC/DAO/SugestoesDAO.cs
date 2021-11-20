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
    public class SugestoesDAO
    {
        private DBConnect _db;

        public SugestoesDAO(ref DBConnect db)
        {
            this._db = db;
        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT                       ");
            sb.AppendLine(" `idsugestao`,      ");
            sb.AppendLine(" `tipo`,        ");
            sb.AppendLine(" `titulo`,        ");
            sb.AppendLine(" `descricao`,             ");
            sb.AppendLine(" `status`,             ");
            sb.AppendLine(" `usuario_idusuario`             ");
            sb.AppendLine("FROM                         ");
            sb.AppendLine(" `sugestoes`         ");

            return sb;
        }

        private Sugestoes loadObject(ref DataRow Rows)
        {
            Sugestoes _sugestoes = new Sugestoes();
            UsuarioDAO _usuarioDAO = new UsuarioDAO(ref _db);

            _sugestoes.IdSugestao = (int)Rows["idsugestao"];
            _sugestoes.Tipo = (string)Rows["tipo"];
            _sugestoes.Titulo = (string)Rows["titulo"];
            _sugestoes.Descricao = (string)Rows["descricao"];
            _sugestoes.Status = (string)Rows["status"];
            _sugestoes.IdUsuario = (int)Rows["usuario_idusuario"];
            _sugestoes.Usuario = _usuarioDAO.LoadById((int)Rows["usuario_idusuario"]);

            return _sugestoes;
        }
       
        public Sugestoes LoadById(int idsugestao)
        {
            Sugestoes _u = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idsugestao = @idsugestao ");

            MySql.Data.MySqlClient.MySqlCommand myCMD = new MySql.Data.MySqlClient.MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@idsugestao", MySql.Data.MySqlClient.MySqlDbType.Int16, 0).Value = idsugestao;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];

                _u = loadObject(ref dr);

            }

            return _u;

        }

        public List<Sugestoes> LoadBySugestoes(ref Sugestoes sugestoes)
        {
            List<Sugestoes> lSugestoes = new List<Sugestoes>();

            StringBuilder sb = GetPrefix();

            return lSugestoes;

        }

        /// <summary>
        /// Listar
        /// </summary>
        /// <returns></returns>
        public List<Sugestoes> All()
        {
            List<Sugestoes> lSugestoes = new List<Sugestoes>();

            StringBuilder sb = GetPrefix();

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;

            myCMD.CommandText = sb.ToString();


            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lSugestoes.Add(loadObject(ref dr));
            }

            return lSugestoes;

        }

        /// <summary>
        /// Inserir novo registro
        /// </summary>
        /// <param name="_obj"></param>
        public void Insert(Sugestoes _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `sugestoes` ");
            sb.AppendLine("(");
            sb.AppendLine("  `tipo`,");
            sb.AppendLine("  `titulo`,");
            sb.AppendLine("  `descricao`,");
            sb.AppendLine("  `status`,");
            sb.AppendLine("  `usuario_idusuario`) ");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @tipo,");
            sb.AppendLine("  @titulo,");
            sb.AppendLine("  @descricao,");
            sb.AppendLine("  @status,");
            sb.AppendLine("  @usuario_idusuario);");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@tipo", MySqlDbType.VarChar, 45).Value = _obj.Tipo.ToUpper();
                myCMD.Parameters.Add("@titulo", MySqlDbType.VarChar, 45).Value = _obj.Titulo.ToUpper();
                myCMD.Parameters.Add("@descricao", MySqlDbType.Text).Value = _obj.Descricao.ToUpper();
                myCMD.Parameters.Add("@status", MySqlDbType.VarChar, 45).Value = _obj.Status;
                myCMD.Parameters.Add("@usuario_idusuario", MySqlDbType.Int16, 0).Value = _obj.IdUsuario;

                myCMD.ExecuteNonQuery();

                //obter o id gerado
                //_obj.IdSugestao = myCMD.LastInsertedId;

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
        public int Update(Sugestoes _obj)
        {
            int iAffectedRows = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE ");
            sb.AppendLine("  `sugestoes`  ");
            sb.AppendLine("SET ");
            sb.AppendLine("  `tipo` = @tipo,");
            sb.AppendLine("  `titulo` = @titulo,");
            sb.AppendLine("  `descricao` = @descricao,");
            sb.AppendLine("  `status` = @status,");

            sb.AppendLine("  `usuario_idusuario` = @usuario_idusuario ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("  `idsugestao` = @idsugestao;");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@tipo", MySqlDbType.VarChar, 45).Value = _obj.Tipo.ToUpper();
                myCMD.Parameters.Add("@titulo", MySqlDbType.VarChar, 45).Value = _obj.Titulo.ToUpper();
                myCMD.Parameters.Add("@descricao", MySqlDbType.Text).Value = _obj.Descricao.ToUpper();
                myCMD.Parameters.Add("@status", MySqlDbType.VarChar, 45).Value = _obj.Status.ToUpper();
                myCMD.Parameters.Add("@usuario_idusuario", MySqlDbType.Int16, 0).Value = _obj.IdUsuario;
                myCMD.Parameters.Add("@idsugestao", MySqlDbType.Int16, 0).Value = _obj.IdSugestao;

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
        public void Delete(int IdSugestao) 
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM                                  ");
            sb.AppendLine("  `sugestoes`                        ");
            sb.AppendLine("WHERE                                        ");
            sb.AppendLine("  idsugestao = @idsugestao ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@idsugestao", MySqlDbType.Int16).Value = IdSugestao;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }   

        }




    }
}