using Elmah;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace CreamBell_DMS_WebApps.App_Code
{
    public class CreamBellFramework
    {
        static readonly string CONNECTION_STRING = string.Empty;
        static bool customLoggingEnabled = false;
        static CreamBellFramework()
        {
            CONNECTION_STRING = ConfigurationManager.ConnectionStrings["CreamBell"].ToString();
            customLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["CustomLogingEnabled"].ToString());
        }
        public static void BindToDropDown(DropDownList dropDownList, string strQuery, string textField, string valueField)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(strQuery, conn))
                    {
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            System.Data.DataSet dataSet = new System.Data.DataSet();
                            sqlDataAdapter.Fill(dataSet);
                            dropDownList.Items.Clear();
                            dropDownList.DataSource = dataSet.Tables[0];
                            dropDownList.DataTextField = textField;
                            dropDownList.DataValueField = valueField;
                            dropDownList.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public static void BindToDropDown(DropDownList dropDownList, string strQuery, string textField, string valueField, string zeroIndexText)
        {
            BindToDropDown(dropDownList, strQuery, textField, valueField);
            if (!string.IsNullOrEmpty(zeroIndexText))
            {
                dropDownList.Items.Insert(0, new ListItem(zeroIndexText));
            }
        }

        public static DataTable GetDataFromStoredProcedure(string spName, SqlParameter[] sqlParameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(spName, conn))
                    {
                        sqlCommand.CommandTimeout = 3600;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        if (sqlParameters != null)
                        {
                            sqlCommand.Parameters.AddRange(sqlParameters);
                        }
                        conn.Open();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = sqlCommand;
                        DataSet dataSet = new DataSet();
                        sqlDataAdapter.Fill(dataSet);
                        dt = dataSet.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            return dt;
        }

        public static DataSet GetDataSetFromStoredProcedure(string spName, SqlParameter[] sqlParameters = null)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(spName, conn))
                    {
                        sqlCommand.CommandTimeout = 3600;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        if (sqlParameters != null)
                        {
                            sqlCommand.Parameters.AddRange(sqlParameters);
                        }
                        conn.Open();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = sqlCommand;
                        sqlDataAdapter.Fill(dataSet);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            return dataSet;
        }

        public static int InsertRecord(string spName, SqlParameter[] sqlParameters = null)
        {
            int recordsAffected = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(spName, conn))
                    {
                        sqlCommand.CommandTimeout = 3600;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        if (sqlParameters != null)
                        {
                            sqlCommand.Parameters.AddRange(sqlParameters);
                        }
                        conn.Open();
                        recordsAffected = sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            return recordsAffected;
        }

        public static void LogMessage(string message, string pageName, string functionName, TimeSpan elapsedTime, int order)
        {
            if (customLoggingEnabled)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                SqlParameter paramMessage = new SqlParameter("@p_Message", SqlDbType.NVarChar, 250);
                paramMessage.Value = message;
                sqlParameters.Add(paramMessage);
                SqlParameter paramPage = new SqlParameter("@p_Page", SqlDbType.NVarChar, 100);
                paramPage.Value = pageName;
                sqlParameters.Add(paramPage);
                SqlParameter paramFunctionName = new SqlParameter("@p_FunctionName", SqlDbType.NVarChar, 50);
                paramFunctionName.Value = functionName;
                sqlParameters.Add(paramFunctionName);
                SqlParameter paramTime = new SqlParameter("@p_TimeElapsed", SqlDbType.Time);
                paramTime.Value = elapsedTime;
                sqlParameters.Add(paramTime);
                SqlParameter paramOrder = new SqlParameter("@p_Order", SqlDbType.Int);
                paramOrder.Value = order;
                sqlParameters.Add(paramOrder);
                InsertRecord("sp_CustomLoggerForTI", sqlParameters.ToArray());
            }
        }
    }
}