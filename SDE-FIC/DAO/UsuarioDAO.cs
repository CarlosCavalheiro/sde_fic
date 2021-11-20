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
    public class UsuarioDAO
    {
        private DBConnect _db;

        public UsuarioDAO(ref DBConnect db)
        {
            // TODO: Complete member initialization
            this._db = db;
        }

        private Usuario loadObject(ref DataRow Rows)
        {
            Usuario _usuario = new Usuario();

            _usuario.IdUsuario = (int)Rows["idusuario"];
            _usuario.Perfil = (string)Rows["perfil"];
            _usuario.Username = (string)Rows["username"];
            _usuario.Password = (string)Rows["password"];
            _usuario.NomeCompleto = (string)Rows["nomecompleto"];

            return _usuario;
        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine(" `idusuario`,");
            sb.AppendLine(" `perfil`,");
            sb.AppendLine(" `username`,");
            sb.AppendLine(" `password`,");
            sb.AppendLine(" `nomecompleto`");
            sb.AppendLine(" FROM ");
            sb.AppendLine(" `usuario`");

            return sb;
        }

        public Usuario FindOrDefault(params object[] Keys)
        {
            Usuario _usuario = null;
            using (MySqlCommand command = new MySqlCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM usuario WHERE idusuario=@idusuario";
                command.Parameters.Add("@idusuario", MySqlDbType.Int32).Value = Keys[0];
                command.ExecuteNonQuery();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        _usuario = new Usuario();
                        reader.Read();
                        _usuario.IdUsuario = reader.GetInt32(0);
                        _usuario.Perfil = reader.GetString(1);
                        _usuario.Username = reader.GetString(2);
                        _usuario.Password = reader.GetString(3);
                        _usuario.NomeCompleto = reader.GetString(4);

                    }
                }                
            }
            return _usuario;
        }

        public Usuario LoadById(int IdUsuario)
        {
            Usuario _usuario = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idusuario = @idusuario ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@idusuario", MySqlDbType.Int16, 0).Value = IdUsuario;


            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                _usuario = loadObject(ref dr);

            }

            return _usuario;

        }

        public Usuario LoadByUsername(string Username)
        {
            Usuario _usuario = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" WHERE username = @Username");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@Username", MySqlDbType.VarChar, 50).Value = Username;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                _usuario = loadObject(ref dr);
            }

            return _usuario;

        }

        /// <summary>
        /// Listar todos os registros
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Usuario> All()
        {
            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine(" ORDER BY username");

            MySqlCommand command = new MySqlCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sbSQL.ToString();
            command.Connection = _db.Connection;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return new Usuario()
                        {
                            IdUsuario = reader.GetInt32(0),
                            Perfil = reader.GetString(1),
                            Username = reader.GetString(2),
                            Password = reader.GetString(3),
                            NomeCompleto = reader.GetString(4),
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Inserir novo registro
        /// </summary>
        /// <param name="_obj"></param>
        public void Insert(Usuario _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `usuario`");
            sb.AppendLine("(");
            sb.AppendLine("  `perfil`,");
            sb.AppendLine("  `username`,");
            sb.AppendLine("  `password`,");
            sb.AppendLine("  `nomecompleto`) ");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @perfil,");
            sb.AppendLine("  @username,");
            sb.AppendLine("  @password,");
            sb.AppendLine("  @nomecompleto);");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@perfil", MySqlDbType.VarChar, 45).Value = _obj.Perfil;
                myCMD.Parameters.Add("@username", MySqlDbType.VarChar, 45).Value = _obj.Username;
                myCMD.Parameters.Add("@password", MySqlDbType.VarChar, 45).Value = _obj.Password;
                myCMD.Parameters.Add("@nomecompleto", MySqlDbType.VarChar, 100).Value = _obj.NomeCompleto.ToUpper();
                myCMD.ExecuteNonQuery();

                //obter o id gerado
                //_obj.IdUsuario = myCMD.LastInsertedId;

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
        public int Update(Usuario _obj)
        {
            int iAffectedRows = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE ");
            sb.AppendLine("  `usuario`  ");
            sb.AppendLine("SET ");
            sb.AppendLine("  `perfil` = @perfil,");
            sb.AppendLine("  `username` = @username,");
            sb.AppendLine("  `password` = @password,");
            sb.AppendLine("  `nomecompleto` = @nomecompleto ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("  `idusuario` = @idusuario;");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@perfil", MySqlDbType.VarChar, 45).Value = _obj.Perfil;
                myCMD.Parameters.Add("@username", MySqlDbType.VarChar, 45).Value = _obj.Username;
                myCMD.Parameters.Add("@password", MySqlDbType.VarChar, 45).Value = _obj.Password;
                myCMD.Parameters.Add("@nomecompleto", MySqlDbType.VarChar, 100).Value = _obj.NomeCompleto.ToUpper();
                myCMD.Parameters.Add("@idusuario", MySqlDbType.Int16, 0).Value = _obj.IdUsuario;

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
        public void Delete(Usuario _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM                                  ");
            sb.AppendLine("  `usuario`                                  ");
            sb.AppendLine("WHERE                                        ");
            sb.AppendLine("  idusuario = @idusuario                     ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@idusuario", MySqlDbType.Int16).Value = _obj.IdUsuario;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}