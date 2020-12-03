using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace ClaseDatos
{
    public class SqlQuery
    {
        ClaseConexion con = new ClaseConexion();



        public int Estadoconsulta { get; set; }
        public string Descripcionconsulta { get; set; }


        public SqlDataReader ExecuteDataReader(String queryString)
        {
            SqlDataReader reader;

            try
            {

                using (SqlConnection connection = new SqlConnection(con.DB_ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.CommandTimeout = 5000;

                    //clslog.eLog("Sql: " + queryString);

                    reader = command.ExecuteReader();
                    command.Connection.Close();
                }

                Estadoconsulta = 1;
                Descripcionconsulta = "OK";


            }
            catch (Exception)
            {
                Estadoconsulta = 1;
                Descripcionconsulta = "OK";
                reader = null;
            }

            return reader;

        }

        public int ExecuteNonQuery(String queryString)
        {
            int filas = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(con.DB_ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.CommandTimeout = 5000;

                   // clslog.eLog("Sql: " + queryString);

                    filas = command.ExecuteNonQuery();

                    command.Connection.Close();
                }


                Estadoconsulta = 1;
                Descripcionconsulta = "OK";

            }
            catch (SqlException e)
            {
                Estadoconsulta = 99;
                Descripcionconsulta = "Error: " + e.Message;
            }

            return filas;

        }

        public int ExecuteNonQuery(String queryString, Byte[] Bytee)
        {
            int filas = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(con.DB_ConnectionString))
                {

                    SqlCommand command = new SqlCommand(queryString, connection);
                    //command.Parameters.Add("@@imagen", Bytee);
                    command.Parameters.AddWithValue("@@imagen", Bytee);
                    command.Connection.Open();
                    command.CommandTimeout = 5000;

                  //  clslog.eLog("Sql: " + queryString);

                    filas = command.ExecuteNonQuery();
                    command.Connection.Close();
                }
                Estadoconsulta = 1;
                Descripcionconsulta = "OK";

            }
            catch (Exception e)
            {
                Estadoconsulta = 99;
                Descripcionconsulta = "Error: " + e.Message;

            }

            return filas;
        }


        public int ExecuteNonQuery2(String queryString)
        {
            int filas = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(con.DB_ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.CommandTimeout = 5000;

                   // clslog.eLog("Sql: " + queryString);

                    filas = command.ExecuteNonQuery();


                    command.Connection.Close();
                }


                Estadoconsulta = 1;
                Descripcionconsulta = "OK";

            }
            catch (SqlException e)
            {
                Estadoconsulta = 99;
                Descripcionconsulta = "Error: " + e.Message;
            }

            return filas;

        }

        public DataSet ExecuteDataSet(String queryString)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlDataAdapter adapter;

                using (SqlConnection connection = new SqlConnection(con.DB_ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.CommandTimeout = 5000;

                    //clslog.eLog("Sql: " + queryString);

                    adapter = new SqlDataAdapter(command);

                    adapter.Fill(ds);

                    command.Connection.Close();
                }

                Estadoconsulta = 1;
                Descripcionconsulta = "OK";

            }
            catch (SqlException e)
            {
                Estadoconsulta = 99;
                Descripcionconsulta = "Error: " + e.Message;
            }

            return ds;

        }

        public bool ExecuteTransaccion(List<String> queryString)
        {
            //DataSet ds = new DataSet();

            try
            {


                using (SqlConnection connection = new SqlConnection(con.DB_ConnectionString))
                {
                    // Abrimos la conexión
                    connection.Open();

                    // Creamos el objeto Transaction
                    SqlTransaction tran = connection.BeginTransaction();

                    // lo pongo en un Try/Catch para detectar los errores
                    try
                    {

                        foreach (String query in queryString)
                        {
                            // Creamos el objeto SqlCommand y asignamos los datos
                            // a los parámetros
                            SqlCommand cmd = new SqlCommand(query, connection);

                            cmd.CommandTimeout = 5000;

                           // clslog.eLog("Sql: " + query);
                            // Asignamos la transacción al comando
                            cmd.Transaction = tran;

                            // Ejecutamos el comando
                            cmd.ExecuteNonQuery();

                        }

                        // Si llega aquí es que todo fue bien,
                        // por tanto, llamamos al método Commit
                        tran.Commit();

                        Estadoconsulta = 1;
                        Descripcionconsulta = "Se han actualizado los datos";

                    }
                    catch (Exception ex)
                    {
                        // Si hay error, desahacemos lo que se haya hecho
                        tran.Rollback();
                        Estadoconsulta = 99;
                        Descripcionconsulta = "ERROR: \r\n" + ex.Message;

                        return false;
                    }

                    // Cerramos la conexión,
                    // aunque no es necesario ya que al finalizar
                    // el using se cerrará
                    connection.Close();
                }

                Estadoconsulta = 1;
                Descripcionconsulta = "Querys ejecutados exitosa";

                return true;

            }
            catch (SqlException e)
            {
                Estadoconsulta = 99;
                Descripcionconsulta = "Error: " + e.Message;

                return false;
            }

            //return ds;

        }

    }
}
