using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using FunctionApp6.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp6.DAL
{
    public class DBHelper
    {

        public string connectionString = string.Empty;

        public DBHelper()
        {
//            connectionString = GetSqlAzureConnectionString("AzureDBString");
            connectionString = GetLocalConnectionString();

        }

        public string GetLocalConnectionString()
        {
            string localConnectionString = "Server=tcp:hueno1.database.windows.net,1433;Initial Catalog=hueno1;Persist Security Info=False;User ID=hueno1;Password=Qazplm68@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            return localConnectionString;
        }

        public string GetEnviVariablevalue(string name)
        {
            string configVal = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            return configVal;
        }
        public string GetSqlAzureConnectionString(string name)
        {
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
                string conStr = configuration.GetConnectionString(name);
                return conStr;
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public bool GetRowInsert(string spName, Hashtable inputParameters, ILogger log)
        {
            bool status = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(spName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry key in inputParameters)
                    {
                        cmd.Parameters.AddWithValue(key.Key.ToString(), key.Value);
                    }
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                log.LogError("Error in inserting in spName: " + spName + " Exception message is :" + ex);
                status = false;
            }
            return status;
        }

        public int GetRowInsert(string spName, Hashtable inputParameters, string outputParameters, ILogger log)
        {
            int count = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(spName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry key in inputParameters)
                    {
                        cmd.Parameters.AddWithValue(key.Key.ToString(), key.Value);
                    }
                    cmd.Parameters.Add(outputParameters, SqlDbType.Int);
                    cmd.Parameters[outputParameters].Direction = ParameterDirection.Output;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    count = Convert.ToInt32(cmd.Parameters[outputParameters].Value);

                }
            }
            catch (Exception ex)
            {
                log.LogError("Error in inserting in spName: " + spName + " Exception message is :" + ex);
            }
            return count;

        }

        public string InsertRow(string spName, Hashtable inputParameters, string outputParameters, ILogger log)
        {
            string eIPRID = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(spName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry key in inputParameters)
                    {
                        cmd.Parameters.AddWithValue(key.Key.ToString(), key.Value);
                    }
                    cmd.Parameters.Add(outputParameters, SqlDbType.VarChar, 20);
                    cmd.Parameters[outputParameters].Direction = ParameterDirection.Output;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    eIPRID = cmd.Parameters[outputParameters].Value.ToString();
                    //log.LogInformation(cmd.Parameters[outputParameters].Value);
                }
            }
            catch (Exception ex)
            {
                log.LogError("Error in inserting in spName: " + spName + " Exception message is :" + ex);
            }
            return eIPRID;
        }

        public DataSet GetDataSet(string spName, Hashtable inputParameters, ILogger log)
        {
            bool status = false;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(spName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry key in inputParameters)
                    {
                        cmd.Parameters.AddWithValue(key.Key.ToString(), key.Value);
                    }
                    conn.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                log.LogError("Error in fetching data from spName: " + spName + " Exception message is :" + ex.Message);
            }
            return ds;
        }

        public DataTable GetDataTable(string spName, Hashtable inputParameters, ILogger log)
        {
            bool status = false;
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(spName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry key in inputParameters)
                    {
                        cmd.Parameters.AddWithValue(key.Key.ToString(), key.Value);
                    }
                    conn.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);


                    adp.Fill(dt);

                }
            }
            catch (Exception ex)
            {
                log.LogError("Error in fetching data from spName: " + spName + " Exception message is :" + ex);
            }
            return dt;
        }
        public DataTable ExecuteQueryWithJoin()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT g.GroupId, g.GroupName, p.PartnerId, p.PartnerName
                             FROM Group g
                             JOIN MappingPartnerGroup mpg ON g.GroupId = mpg.GroupId
                             JOIN Partner p ON mpg.PartnerId = p.PartnerId";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }

            return dataTable;
        }
      
        public string GetSPReturnJson(ILogger log, string sp)
        {
            string json = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sp, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        json = reader.GetString(0);
                    }
                    reader.Close();
                }
                return json;
            }
            catch (Exception ex)
            {
                log.LogError("Error in fetching data from sp_JSON. Exception message is: " + ex.Message);
                return string.Empty;
            }
        }

        public string GetSPReturnJson(ILogger log, string sp, SqlParameter[] parameters)
        {
            string json = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sp, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters); // Add the parameters to the command

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        json = reader.GetString(0);
                    }
                    reader.Close();
                }
                return json;
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while executing the stored procedure.");
                throw; // Rethrow the exception to be handled at the caller's level
            }
        }


    }
}
