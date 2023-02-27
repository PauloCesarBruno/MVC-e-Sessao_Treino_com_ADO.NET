using System;
//
using System.Data;
using System.Data.SqlClient;

namespace Treinando_MVC_e_Sessao.Data_Access_Layer
{
    public class DAL
    {
        public static readonly String Server = "HOME";
        public static readonly String Database = "TreinoASPCore2_2";
        public static readonly String User = "sa";
        public static readonly String Password = "Paradoxo22";

        public static readonly String StrSql = $"Server = {Server}; Database = {Database}; Uid = {User}; Pwd = {Password}";

        public SqlConnection Conexao ()
        {
            return new SqlConnection(StrSql);
        }

        public void FecharConexao()
        {
            SqlConnection conn = Conexao();
            conn.Close();
        }

#pragma warning disable IDE0044 // Adicionar modificador somente leitura
        private SqlParameterCollection Colecao = new SqlCommand().Parameters;
#pragma warning restore IDE0044 // Adicionar modificador somente leitura

        public void LimparParametro()
        {
            Colecao.Clear();
        }

        public void AddParametros(String nome, Object valor)
        {
            Colecao.Add(new SqlParameter(nome, valor));
        }

        public Object ExecutarManipulacao(CommandType commandType, String Sp_Ou_Texto)
        {
            try
            {
                SqlConnection conn = Conexao();
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = commandType;
                cmd.CommandText = Sp_Ou_Texto;
                cmd.CommandTimeout = 3600;

                foreach (SqlParameter param in Colecao)
                {
                    cmd.Parameters.Add(new SqlParameter(param.ParameterName, param.Value));
                }
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable ExecutarConsulta(CommandType commandType, String Sp_Ou_Texto)
        {
            try
            {
                SqlConnection conn = Conexao();
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = commandType;
                cmd.CommandText = Sp_Ou_Texto;
                cmd.CommandTimeout = 3600;

                foreach (SqlParameter param in Colecao)
                {
                    cmd.Parameters.Add(new SqlParameter(param.ParameterName, param.Value));
                }
#pragma warning disable IDE0090 // Usar 'new(...)'
                DataTable dt = new DataTable();
#pragma warning restore IDE0090 // Usar 'new(...)'
#pragma warning disable IDE0090 // Usar 'new(...)'
                SqlDataAdapter da = new SqlDataAdapter(cmd);
#pragma warning restore IDE0090 // Usar 'new(...)'
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable RetDatatable (String sql)
        {
            try
            {
#pragma warning disable IDE0090 // Usar 'new(...)'
                DataTable dt = new DataTable();
#pragma warning restore IDE0090 // Usar 'new(...)'
#pragma warning disable IDE0090 // Usar 'new(...)'
                SqlCommand cmd = new SqlCommand(sql, Conexao());
#pragma warning restore IDE0090 // Usar 'new(...)'
#pragma warning disable IDE0090 // Usar 'new(...)'
                SqlDataAdapter da = new SqlDataAdapter(cmd);
#pragma warning restore IDE0090 // Usar 'new(...)'
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Polimorfismo para evitar ataque de injecão de SQL...
        public DataTable RetDatatable(SqlCommand cmd)
        {
            try
            {
#pragma warning disable IDE0090 // Usar 'new(...)'
                DataTable dt = new DataTable();
#pragma warning restore IDE0090 // Usar 'new(...)'
                cmd.Connection = Conexao();
#pragma warning disable IDE0090 // Usar 'new(...)'
                SqlDataAdapter da = new SqlDataAdapter(cmd);
#pragma warning restore IDE0090 // Usar 'new(...)'
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}