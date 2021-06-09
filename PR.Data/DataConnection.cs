using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace PR.Data
{
    public class DataConnection
    {
        private SqlConnection _connection = null;
        private string _connectionString = string.Empty;
        public DataConnection()
        {
            _connectionString = "AQUI LA CONEXION";
        }

        public DataTable ExecuteToTable(string storedProcedure, List<SqlParameter> parameters)
        {
            DateTime inicio = DateTime.Now;
            DataTable _Datos = null;
            try
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
                SqlCommand _Command = new SqlCommand(storedProcedure);

                _Command.CommandType = CommandType.StoredProcedure;
                _Command.CommandTimeout = 0;
                _Command.Connection = _connection;
                if (parameters != null)
                    if (parameters.Count > 0)
                        _Command.Parameters.AddRange(parameters.ToArray());
                SqlDataAdapter _Adapter = new SqlDataAdapter(_Command);
                _Datos = new DataTable();
                _Adapter.Fill(_Datos);
                return _Datos;
            }
            catch (SqlException)
            {
                return _Datos;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}
