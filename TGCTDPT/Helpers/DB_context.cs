using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace TGCTDPT.Helpers
{
    public class DB_context
    {
        public int Execute_query(string proc_name, Dictionary<object, object> param)
        {
            string constr = ConfigurationManager.ConnectionStrings["TGSTConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            try
            {

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = proc_name;
                if (param != null)
                {
                    if (param.Count > 0)
                    {
                        foreach (KeyValuePair<object, object> entry in param)
                        {
                            com.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);
                        }

                    }
                }

                int i = Convert.ToInt32(com.ExecuteNonQuery());
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        public DataSet Get_query_datatable(string Query)
        {
            string constr = ConfigurationManager.ConnectionStrings["TGSTConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            try
            {

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = Query;


                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;
                DataSet ds = new DataSet();
                da.Fill(ds);

                return ds;


            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

            }

        }

        public DataSet Get_datatable(string proc_name, Dictionary<string, string> param)
        {
            string constr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            try
            {

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = proc_name;
                if (param != null)
                {
                    if (param.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> entry in param)
                        {
                            com.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);
                        }

                    }
                }

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;
                DataSet ds = new DataSet();
                da.Fill(ds);

                return ds;


            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

            }

        }



        public DataSet Get_datatable(string proc_name, Dictionary<string, string> param, string Database)
        {
            string constr = "";
            if (Database.ToUpper().Trim() == "TGST")
            {
                constr = ConfigurationManager.ConnectionStrings["TGSTConnectionString"].ConnectionString;
            }
            else if (Database.ToUpper().Trim() == "PROGST")
            {
                constr = ConfigurationManager.ConnectionStrings["proConnectionString"].ConnectionString;
            }
            else if (Database.ToUpper().Trim() == "CCW")
            {
                constr = ConfigurationManager.ConnectionStrings["CCWConnectionString"].ConnectionString;
            }

            SqlConnection con = new SqlConnection(constr);
            try
            {

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = proc_name;
                if (param != null)
                {
                    if (param.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> entry in param)
                        {
                            com.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);
                        }

                    }
                }

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;

            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

            }

        }

        public int Execute_query(string proc_name, Dictionary<object, object> param, string Database)
        {

            string constr = "";
            if (Database.ToUpper().Trim() == "TGST")
            {
                constr = ConfigurationManager.ConnectionStrings["TGSTConnectionString"].ConnectionString;
            }
            else if (Database.ToUpper().Trim() == "PROGST")
            {
                constr = ConfigurationManager.ConnectionStrings["proConnectionString"].ConnectionString;
            }


            SqlConnection con = new SqlConnection(constr);
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = proc_name;
                if (param != null)
                {
                    if (param.Count > 0)
                    {
                        foreach (KeyValuePair<object, object> entry in param)
                        {
                            com.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);
                        }
                    }
                }
                int i = com.ExecuteNonQuery();
                //int i = Convert.ToInt32(com.ExecuteNonQuery());
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }

        }
        public int Execute_query_Out(string proc_name, Dictionary<object, object> parameters, string connectionName, out string outputMessage)
        {
            outputMessage = string.Empty;
            string constr = "";
            if (connectionName.ToUpper().Trim() == "TGST")
            {
                constr = "TGSTConnectionString";
            }
            else if (connectionName.ToUpper().Trim() == "PROGST")
            {
                constr = "proConnectionString";
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[constr].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(proc_name, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key.ToString(), param.Value ?? DBNull.Value);
                    }

                    SqlParameter outParam = new SqlParameter("@OUT_MSG", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParam);

                    con.Open();
                    int result = cmd.ExecuteNonQuery();

                    outputMessage = outParam.Value.ToString();

                    return result;
                }
            }
        }
    }
}