using SDE_FIC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using SDE_FIC.Util;
using MySql.Data.MySqlClient;

namespace SDE_FIC.DAO
{
    public class AlunoDAO
    {
        private DBConnect _db;

        public AlunoDAO(ref DBConnect db)
        {
            this._db = db;

        }

        private StringBuilder GetPrefix()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("  `idaluno`,");
            sb.AppendLine("  `datacadastro`,");
            sb.AppendLine("  `nome`,");
            sb.AppendLine("  `telefoneres`,");
            sb.AppendLine("  `telefonecel`,");
            sb.AppendLine("  `email`,");
            sb.AppendLine("  `observacao`,");
            sb.AppendLine("  `cpf`");
            sb.AppendLine("FROM ");
            sb.AppendLine("  `aluno`");

            return sb;
        }

        private Aluno loadObject(ref DataRow Rows)
        {
            Aluno _aluno = new SDE_FIC.Models.Aluno();

            _aluno.IdAluno = Convert.ToInt16(Rows["idaluno"]);
            _aluno.DataCadastro = Convert.ToDateTime(Rows["datacadastro"].ToString());
            _aluno.Nome = Convert.ToString(ValueUtil.DBNullToString(Rows["nome"]));
            _aluno.TelefoneResidencial = Convert.ToString(Rows["telefoneres"]);
            _aluno.TelefoneCelular = Convert.ToString(Rows["telefonecel"]);
            _aluno.Email = Convert.ToString(Rows["email"]);
            _aluno.Observacao = Convert.ToString(Rows["observacao"]);
            _aluno.CPF = Convert.ToString(Rows["cpf"]);

            return _aluno;
        }

        public Aluno LoadById(int IdAluno)
        {
            Aluno _aluno = null;

            StringBuilder sbSQL = GetPrefix();
            sbSQL.AppendLine("WHERE idaluno = @idaluno ");

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.CommandText = sbSQL.ToString();
            myCMD.Connection = _db.Connection;
            myCMD.Parameters.Add("@idaluno", MySqlDbType.Int16, 0).Value = IdAluno;

            DataTable dt = _db.ExecuteQuery(ref myCMD);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                _aluno = loadObject(ref dr);

            }

            return _aluno;

        }

        public List<Aluno> All()
        {
            List<Aluno> lAluno = new List<Aluno>();

            StringBuilder sb = GetPrefix();

            MySqlCommand myCMD = new MySqlCommand();
            myCMD.Connection = _db.Connection;
            myCMD.CommandText = sb.ToString();


            DataTable dt = _db.ExecuteQuery(ref myCMD);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lAluno.Add(loadObject(ref dr));
            }

            return lAluno;

        }
        
        public void Insert(Aluno _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `aluno`");
            sb.AppendLine("(");
            sb.AppendLine("  `datacadastro`,");
            sb.AppendLine("  `nome`,");
            sb.AppendLine("  `telefoneres`,");
            sb.AppendLine("  `telefonecel`,");
            sb.AppendLine("  `email`,");
            sb.AppendLine("  `observacao`,");
            sb.AppendLine("  `cpf`");
            sb.AppendLine(")");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @datacadastro,");
            sb.AppendLine("  @nome,");
            sb.AppendLine("  @telefoneres, ");
            sb.AppendLine("  @telefonecel, ");
            sb.AppendLine("  @email, ");
            sb.AppendLine("  @observacao, ");
            sb.AppendLine("  @cpf );");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@datacadastro", MySqlDbType.Date).Value = _obj.DataCadastro;
                myCMD.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = _obj.Nome.ToUpper();
                myCMD.Parameters.Add("@telefoneres", MySqlDbType.VarChar, 50).Value = _obj.TelefoneResidencial;
                myCMD.Parameters.Add("@telefonecel", MySqlDbType.VarChar, 50).Value = _obj.TelefoneCelular;
                if (_obj.Email != null)
                {
                    myCMD.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = _obj.Email.ToLower();
                }
                else
                {
                    myCMD.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = _obj.Email;
                }
                if (_obj.Observacao != null)
                {
                    myCMD.Parameters.Add("@observacao", MySqlDbType.VarChar, 255).Value = _obj.Observacao.ToUpper();
                }
                else
                {
                    myCMD.Parameters.Add("@observacao", MySqlDbType.VarChar, 255).Value = _obj.Observacao;
                }
                myCMD.Parameters.Add("@cpf", MySqlDbType.VarChar, 45).Value = _obj.CPF;

                myCMD.ExecuteNonQuery();
                
                //Obter o id inserido
                _obj.IdAluno = Convert.ToInt32(myCMD.LastInsertedId);


            }
            catch (Exception _ex)
            {
                throw _ex;

            }


        }

        public void InsertColecao(Aluno _obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("  `aluno`");
            sb.AppendLine("(");
            sb.AppendLine("  `datacadastro`,");
            sb.AppendLine("  `nome`,");
            sb.AppendLine("  `telefoneres`,");
            sb.AppendLine("  `telefonecel`,");
            sb.AppendLine("  `email`,");
            sb.AppendLine("  `observacao`,");
            sb.AppendLine("  `cpf`");
            sb.AppendLine(")");
            sb.AppendLine("VALUE (");
            sb.AppendLine("  @datacadastro,");
            sb.AppendLine("  @nome,");
            sb.AppendLine("  @telefoneres, ");
            sb.AppendLine("  @telefonecel, ");
            sb.AppendLine("  @email, ");
            sb.AppendLine("  @observacao, ");
            sb.AppendLine("  @cpf );");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@datacadastro", MySqlDbType.Date).Value = _obj.DataCadastro;
                myCMD.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = _obj.Nome.ToUpper();
                myCMD.Parameters.Add("@telefoneres", MySqlDbType.VarChar, 50).Value = _obj.TelefoneResidencial;
                myCMD.Parameters.Add("@telefonecel", MySqlDbType.VarChar, 50).Value = _obj.TelefoneCelular;
                if (_obj.Email != null)
                {
                    myCMD.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = _obj.Email.ToLower();
                }
                else
                {
                    myCMD.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = _obj.Email;
                }
                if (_obj.Observacao != null)
                {
                    myCMD.Parameters.Add("@observacao", MySqlDbType.VarChar, 255).Value = _obj.Observacao.ToUpper();
                }
                else
                {
                    myCMD.Parameters.Add("@observacao", MySqlDbType.VarChar, 255).Value = _obj.Observacao;
                }
                myCMD.Parameters.Add("@cpf", MySqlDbType.VarChar, 45).Value = _obj.CPF;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }


        }

        public int Update(Aluno _obj)
        {
            int iAffectedRows = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE ");
            sb.AppendLine("  `aluno` ");
            sb.AppendLine(" SET ");
            sb.AppendLine("  `datacadastro` = @datacadastro, ");
            sb.AppendLine("  `nome` = @nome,");
            sb.AppendLine("  `telefoneres` = @telefoneres,");
            sb.AppendLine("  `telefonecel` = @telefonecel,");
            sb.AppendLine("  `email` = @email, ");
            sb.AppendLine("  `cpf` = @cpf, ");
            sb.AppendLine("  `observacao` = @observacao");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("  `idaluno` = @idaluno;");


            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@datacadastro", MySqlDbType.Date).Value = _obj.DataCadastro;
                myCMD.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = _obj.Nome.ToUpper();
                myCMD.Parameters.Add("@telefoneres", MySqlDbType.VarChar, 50).Value = _obj.TelefoneResidencial;
                myCMD.Parameters.Add("@telefonecel", MySqlDbType.VarChar, 50).Value = _obj.TelefoneCelular;
                if (_obj.Email != null)
                {
                    myCMD.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = _obj.Email.ToLower();
                }
                else {
                    myCMD.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = _obj.Email;
                }
                if (_obj.Observacao != null)
                {
                    myCMD.Parameters.Add("@observacao", MySqlDbType.VarChar, 255).Value = _obj.Observacao.ToUpper();
                }
                else {
                    myCMD.Parameters.Add("@observacao", MySqlDbType.VarChar, 255).Value = _obj.Observacao;
                }
                myCMD.Parameters.Add("@cpf", MySqlDbType.VarChar, 45).Value = _obj.CPF;

                myCMD.Parameters.Add("@idaluno", MySqlDbType.Int32, 0).Value = _obj.IdAluno;


                iAffectedRows = myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

            return iAffectedRows;

        }

        public void Delete(int IdAluno)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE FROM                                  ");
            sb.AppendLine("  `aluno`                        ");
            sb.AppendLine("WHERE                                        ");
            sb.AppendLine("  idaluno = @idaluno ");

            try
            {
                MySqlCommand myCMD = new MySqlCommand();
                myCMD.Connection = _db.Connection;
                myCMD.Transaction = _db.Transaction;
                myCMD.CommandText = sb.ToString();
                myCMD.Parameters.Add("@idaluno", MySqlDbType.Int16).Value = IdAluno;

                myCMD.ExecuteNonQuery();

            }
            catch (Exception _ex)
            {
                throw _ex;

            }

        }
    }
}